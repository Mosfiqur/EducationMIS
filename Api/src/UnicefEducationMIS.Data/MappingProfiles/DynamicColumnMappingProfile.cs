using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.MappingProfiles
{
    public class DynamicColumnMappingProfile : Profile
    {
        public DynamicColumnMappingProfile()
        {
            CreateMap<ListDataTypeCreateViewModel, ListDataType>();
            CreateMap<ListDataTypeUpdateViewModel, ListDataType>();
            

            CreateMap<ListItemViewModel, ListItem>().ReverseMap();
            CreateMap<ListDataTypeViewModel, ListDataType>()
                .ForMember(x => x.ListItems, y => y.MapFrom(z => z.ListItems.Select(esp =>
                            new ListItemViewModel
                            {
                                Id = esp.Id,
                                Title = esp.Title,
                                Value = esp.Value
                            }))).ReverseMap();

            CreateMap<DynamicColumnViewModel, EntityDynamicColumn>()
                        .ForMember(x => x.Id, y => y.MapFrom(z => z.EntityColumnId))
                        .ForMember(x => x.Name, y => y.MapFrom(z => z.ColumnName))
                        .ForMember(x => x.NameInBangla, y => y.MapFrom(z => z.ColumnNameInBangla))
                        .ForMember(x => x.ColumnType, y => y.MapFrom(z => z.ColumnDataType));

        }
    }
}
