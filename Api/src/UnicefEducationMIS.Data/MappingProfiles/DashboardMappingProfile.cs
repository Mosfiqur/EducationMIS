using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel.Dashboard;

namespace UnicefEducationMIS.Data.MappingProfiles
{
    public class DashboardMappingProfile : Profile
    {
        public DashboardMappingProfile()
        {
            CreateMap<CampCoordinate, GapMapViewModel>();
        }
    }
}
