using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.MappingProfiles
{
   public class BeneficiaryMappingProfile : Profile
    {
        public BeneficiaryMappingProfile()
        {
            CreateMap<BeneficiaryAddViewModel, Beneficiary>();
            CreateMap<BeneficiaryAddViewModel, BeneficiaryDataCollectionStatus>()
                .ForMember(dest => dest.BeneficiaryId, opt => opt.MapFrom(src => src.Id));

            CreateMap<GridViewViewModel, GridView>();
            CreateMap<GridViewDetailsViewModel, GridViewDetails>();

            CreateMap<BeneficiaryDynamicCellViewModel, BeneficiaryDynamicCell>()
                .ForMember(x => x.EntityDynamicColumnId, y => y.MapFrom(z => z.EntityColumnId));

            CreateMap<BeneficairyObjectViewModel, BeneficiaryViewModel>();
        }
    }
}
