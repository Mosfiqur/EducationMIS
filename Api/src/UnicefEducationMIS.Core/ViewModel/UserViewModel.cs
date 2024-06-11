using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel.Framework;
using UnicefEducationMIS.Core.ViewModel.User;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            DynamicCells = new List<UserDynamicCellViewModel>();
            EduSectorPartners = new List<EduSectorPartnerViewModel>();
        }
        public int? Id { get; set; }               

        [Required]
        public string FullName { get; set; }
        
        
        public byte LevelId { get; set; }
        public string LevelName { get; set; }
        public LevelRank LevelRank { get; set; }
        
        public string DesignationName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        [EmailAddress]
        public string Email { get; set; }        
        public string PhoneNumber { get; set; }        
        public List<EduSectorPartnerViewModel> EduSectorPartners { get; set; }
        public List<UserDynamicCellViewModel> DynamicCells { get; set; }

        public string ProgramPartnerName
        {
            get
            {
                var p =
                    EduSectorPartners.FirstOrDefault(x => x.PartnerType == PartnerType.ProgramPartner);
                return p != null ? p.PartnerName : string.Empty;
            }
            
        }

        public string ImplementingPartnerName
        {
            get
            {
                var p =
                    EduSectorPartners.FirstOrDefault(x => x.PartnerType == PartnerType.ImplementationPartner);
                return p != null ? p.PartnerName : string.Empty;
            }

        }
        
        public string EduPartnerName
        {
            get
            {
                var p =
                    EduSectorPartners.FirstOrDefault(x => x.PartnerType == PartnerType.EducationSectorPartner);
                return p != null ? p.PartnerName : string.Empty;
            }

        }

        public static Expression<Func<Models.Identity.User, UserViewModel>> FromModel = x => new UserViewModel
        {
            Id = x.Id,
            FullName = x.FullName,
            DesignationName = x.DesignationName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            LevelId = x.UserRoles.Select(userRole => userRole.Role).First().Level.Id,
            LevelName = x.UserRoles.Select(userRole => userRole.Role).First().Level.LevelName,
            RoleId = x.UserRoles.Single().Role.Id,
            RoleName = x.UserRoles.Single().Role.Name,

            DynamicCells = x.DynamicCells.AsQueryable()
                    .Select(UserDynamicCellViewModel.FromModel).ToList()
        };

        public static Func<UserViewModel, UserViewModel> GroupByEntityDynamicColumn = x => new UserViewModel
        {
            Id = x.Id,
            FullName = x.FullName,
            DesignationName = x.DesignationName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            LevelId = x.LevelId,
            LevelName = x.LevelName,
            RoleId = x.RoleId,
            RoleName = x.RoleName,
            EduSectorPartners = x.EduSectorPartners,
            DynamicCells = x.DynamicCells
                .GroupBy(cell => new
                {
                    cell.EntityDynamicColumnId,
                    cell.UserId,
                    cell.ColumnName,
                    cell.DataType
                })
                .ToList()
                .Select(col => new UserDynamicCellViewModel()
                {
                    ColumnName = col.Key.ColumnName,
                    EntityDynamicColumnId = col.Key.EntityDynamicColumnId,
                    DataType = col.Key.DataType,
                    UserId = col.Key.UserId,
                    Values = col.Select(c => c.Value).ToList(),
                    ListType=col.Select(c=>c.ListType).FirstOrDefault()
                })
                .ToList()
        };
    }
}
