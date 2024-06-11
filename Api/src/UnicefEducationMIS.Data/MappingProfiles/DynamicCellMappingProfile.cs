using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.MappingProfiles
{
    public class DynamicCellMappingProfile : Profile
    {
        public DynamicCellMappingProfile()
        {
            CreateMap<DynamicCellViewModel, BeneficiaryDynamicCell>();
            CreateMap<BeneficiaryDynamicCellAddViewModel, BeneficiaryDynamicCell>();
           
        }
    }
}
