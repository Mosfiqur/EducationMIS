using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Service.ApplicationServices
{
    public class CurrentLoginUserService : ICurrentLoginUserService
    {
        private List<Claim> _claims;
        public CurrentLoginUserService()
        {
            _claims = new List<Claim>();
            Esps = new List<EducationSectorPartner>();
        }
        public int UserId => int.Parse(_claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        public string Role => _claims.Single(x => x.Type == ClaimTypes.Role).Value;

        public bool IsAdmin => UserRank == LevelRank.Admin;

        public bool IsTeacher => UserRank == LevelRank.Teacher;
        public int? EducationSectorPartner
        {
            get
            {
                var claim = _claims.SingleOrDefault(x => x.Type == UnicefClaimTypes.EducationSectorPartner);
                return claim != null ? (Nullable<int>)int.Parse(claim.Value) : null;
            }
        }

        public int? ProgramPartner
        {
            get
            {
                var val = _claims.SingleOrDefault(x => x.Type == UnicefClaimTypes.ProgramPartner)?.Value;
                return !string.IsNullOrEmpty(val) ? (Nullable<int>)int.Parse(val) : null;
            }
        }

        public List<int?> ImplementationPartner
        {
            get
            {
                return _claims
                    .Where(x => x.Type == UnicefClaimTypes.ImplementationPartner)
                    .Select(x => (Nullable<int>)int.Parse(x?.Value))
                    .ToList();
            }
        }

        public LevelRank UserRank
        {
            get
            {
                return (LevelRank)Enum.Parse(typeof(LevelRank), _claims.Single(x => x.Type == UnicefClaimTypes.UserRank).Value);
            }
        }

        public void SetClaims(IEnumerable<Claim> claims)
        {
            _claims.AddRange(claims);
        }
        public List<Claim> GetClaims() => _claims;
        public List<EducationSectorPartner> Esps { get; set; }
    }
}
