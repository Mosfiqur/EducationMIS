using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.MappingProfiles
{
    public class GridViewMappingProfile:Profile
    {
        public GridViewMappingProfile()
        {
            CreateMap<GridViewViewModel, GridView>();
        }
    }
}
