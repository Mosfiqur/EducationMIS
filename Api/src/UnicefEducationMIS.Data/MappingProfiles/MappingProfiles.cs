using AutoMapper;
using System.Linq;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AddInstanceIndicatorViewModel, InstanceIndicator>();

            CreateMap<UserViewModel, User>()
                .ForMember(x => x.UserEduSectorPartners, cfg =>
                        cfg.MapFrom(userVM =>
                            userVM.EduSectorPartners.Select(esp =>
                            new UserEduSectorPartner
                            {
                                PartnerId = esp.Id,
                                PartnerType = esp.PartnerType,
                                UserId = userVM.Id ?? 0
                            })
                            )
                        )                
                .ReverseMap()

                .ForMember(x => x.LevelName,
                    cfg => cfg.MapFrom(x => x.UserRoles.Select(x => x.Role).First().Level.LevelName))
                 .ForMember(x => x.LevelId,
                    cfg => cfg.MapFrom(x => x.UserRoles.Select(x => x.Role).First().Level.Id))
                .ForMember(x => x.RoleName, opt => opt.MapFrom(x => x.UserRoles.Single().Role.Name))
                .ForMember(x => x.RoleId, opt => opt.MapFrom(x => x.UserRoles.Single().Role.Id))                
                .ForMember(x => x.EduSectorPartners, opt => opt.MapFrom(x => x.UserEduSectorPartners.Select(y => y.Partner).ToList()))
                .ForMember(x => x.DynamicCells, opt=> opt.MapFrom(x=> x.DynamicCells))
                ;


            CreateMap<UserLevel, UserLevelViewModel>();

            CreateMap<RoleViewModel, Role>()
                .ForMember(role => role.Name, cfg => cfg.MapFrom(x => x.RoleName))
                .ForMember(role => role.RolePermissions,
                        cfg => cfg.MapFrom(roleVM =>
                                 roleVM.Permissions
                                 .Select(p =>
                                     new RolePermission
                                     {
                                         PermissionId = p.Id,
                                         RoleId = roleVM.Id ?? 0
                                     })
                                )
                       )
                .ReverseMap()
                .ForMember(role => role.LevelName,
                        cfg => cfg.MapFrom(
                                role => role.Level.LevelName))
                .ForMember(roleVM => roleVM.Permissions, cfg =>
                        cfg.MapFrom(role => role.RolePermissions.Select(x => x.Permission).ToList()))
                .ForMember(role => role.LevelRank, cfg => cfg.MapFrom(x => x.Level.Rank))
                ;

            CreateMap<PermissionViewModel, Permission>()
                .ReverseMap();
            CreateMap<PermissionPresetViewModel, PermissionPreset>()
               .ReverseMap();

            CreateMap<EduSectorPartnerViewModel, EducationSectorPartner>().ReverseMap();

           // CreateMap<DesignationViewModel, Designation>().ReverseMap();

            CreateMap<JrpParameterInfo,JrpParameterInfoViewModel>()
                .ReverseMap();
            CreateMap<JrpDashboardFilterViewModel, JrpPropertiesWithFilterViewModel>().ReverseMap();
            CreateMap<JrpPropertiesWithFilterViewModel, JrpPropertiesWithFilterViewModel>();
            CreateMap<EmbeddedDashboardViewModel, EmbeddedDashboard>().ReverseMap();
        }
    }
}
