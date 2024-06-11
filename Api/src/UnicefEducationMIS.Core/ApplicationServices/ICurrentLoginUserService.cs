using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ApplicationServices
{
    public interface ICurrentLoginUserService
    {
        int UserId { get; }  
        LevelRank UserRank { get; }
        bool IsAdmin { get; }                
        string Role { get; }
        bool IsTeacher { get; }
        int? EducationSectorPartner { get; }
        int? ProgramPartner { get; }
        List<int?> ImplementationPartner { get; }
        void SetClaims(IEnumerable<Claim> claims);
        List<Claim> GetClaims(); 

        List<EducationSectorPartner> Esps { get; set; }
    }
}
