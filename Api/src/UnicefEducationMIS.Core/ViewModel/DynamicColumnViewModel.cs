using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class DynamicColumnViewModel
    {
        public long EntityColumnId { get; set; }
        public EntityType entityTypeId { get; set; }
        public string ColumnName { get; set; }
        public string ColumnNameInBangla { get; set; }
        public long? ColumnListId { get; set; }
        public ColumnDataType ColumnDataType { get; set; }
        public string Description { get; set; }
        public bool? IsMultiValued { get; set; }
        public ListDataTypeViewModel ColumnList { get; set; }

        public TargetedPopulation? TargetedPopulation { get; set; }

        public static Expression<Func<EntityDynamicColumn, DynamicColumnViewModel>> FromModel = x => new DynamicColumnViewModel
        {
            EntityColumnId = x.Id,
            entityTypeId = x.EntityTypeId,
            ColumnName = x.Name,
            ColumnNameInBangla = x.NameInBangla,
            ColumnListId = x.ColumnListId,
            ColumnDataType = x.ColumnType,
            Description = x.Description,
            IsMultiValued = x.IsMultiValued,
            TargetedPopulation = x.TargetedPopulation,
            ColumnList = new ListDataTypeViewModel()
            {
                Id = x.ColumnList.Id,
                Name = x.ColumnList.Name,
                ListItems = x.ColumnList.ListItems.AsQueryable().Select(a => new ListItemViewModel()
                {

                    Id = a.Id,
                    Title = a.Title,
                    Value = a.Value
                }).ToList()
            }

        };
    }
}
