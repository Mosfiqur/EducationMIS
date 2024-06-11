using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class TargetDynamicCellViewModel : EntityDynamicCellViewModel
    {
        public long TargetFrameworkId { get; set; }

        public static Expression<Func<TargetFrameworkDynamicCell, TargetDynamicCellViewModel>> FromModel = col =>
            new TargetDynamicCellViewModel
            {
                Id = col.Id,
                ColumnName = col.EntityDynamicColumn.Name,
                EntityDynamicColumnId = col.EntityDynamicColumnId,
                TargetFrameworkId = col.TargetFrameworkId,
                Value = col.Value,
                DataType = col.EntityDynamicColumn.ColumnType,
                ListType = new ListDataTypeViewModel()
                {
                    Name = col.EntityDynamicColumn.ColumnList.Name,
                    ListItems = col.EntityDynamicColumn
                        .ColumnList
                        .ListItems
                        .Select(item => new ListItemViewModel()
                        {
                            Title = item.Title,
                            Value = item.Value
                        }).ToList()
                }
            };

    }


}
