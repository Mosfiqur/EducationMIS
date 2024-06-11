using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.Import;

namespace UnicefEducationMIS.Data.MappingProfiles
{
    public class FacilityMappingProfile : Profile
    {
        public FacilityMappingProfile()
        {
            CreateMap<CreateFacilityViewModel, Facility>();
            CreateMap<FacilityDynamicCellViewModel, FacilityDynamicCell>()
                .ForMember(dest => dest.InstanceId , opt => opt.MapFrom(src => src.InstanceId));

            CreateMap<CreateFacilityViewModel, FacilityDataCollectionStatus>()
                .ForMember(dest => dest.FacilityId, opt => opt.MapFrom(src => src.Id));

            CreateMap<FacilityImportViewModel, CreateFacilityViewModel>()
                .ForMember(x => x.Name, cfg => cfg.MapFrom( x=> x.FacilityName));

            CreateMap<FacilityObjectViewModel, FacilityViewModel>();

        }
    }
}
