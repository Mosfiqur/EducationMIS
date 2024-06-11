using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Data.MappingProfiles
{
    public class FrameworkMappingProfile : Profile
    {
        public FrameworkMappingProfile()
        {
            CreateMap<ObjectiveIndicatorDynamicCellViewModel, MonitoringFrameworkDynamicCell>()
                .ReverseMap();
            CreateMap<MonitoringFrameworkViewModel, MonitoringFramework>()
                .ReverseMap();

            CreateMap<MonitoringFrameworkCreateViewModel, MonitoringFramework>();

            CreateMap<MonitoringFramework, MonitoringFrameworkUpdateViewModel>();

            CreateMap<BudgetFrameworkCreateViewModel, BudgetFramework>();
            CreateMap<BudgetFrameworkUpdateViewModel, BudgetFramework>();
            CreateMap<BudgetFramework, BudgetFrameworkViewModel>();
            CreateMap<ProjectViewModel, Project>().ReverseMap();
            CreateMap<DonorViewModel, Donor>().ReverseMap();


            CreateMap<TargetFrameworkViewModel, TargetFramework>().ReverseMap();
            CreateMap<TargetFrameworkCreateViewModel, TargetFramework>().ReverseMap();
            CreateMap<TargetFrameworkUpdateViewModel, TargetFramework>().ReverseMap();

            CreateMap<BudgetDynamicCellViewModel, BudgetFrameworkDynamicCell>()
                .ReverseMap();
            CreateMap<TargetDynamicCellInsertViewModel, TargetFrameworkDynamicCell>()
                .ReverseMap();

            CreateMap<MonitoringFrameworkCreateViewModel, MonitoringFramework>().ReverseMap();


            CreateMap<ObjectiveIndicatorCreateViewModel, ObjectiveIndicator>();
            CreateMap<ObjectiveIndicatorUpdateViewModel, ObjectiveIndicator>();
            CreateMap<ObjectiveIndicator, ObjectiveIndicatorViewModel>();
        }
    }
}
