using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class BudgetDynamicCellViewModel : EntityDynamicCellViewModel
    {

        public long BudgetFrameworkId { get; set; }

        public static Expression<Func<BudgetFrameworkDynamicCell, BudgetDynamicCellViewModel>> FromModel = col =>
            new BudgetDynamicCellViewModel
            {
                Id = col.Id,
                ColumnName = col.EntityDynamicColumn.Name,
                EntityDynamicColumnId = col.EntityDynamicColumnId,
                BudgetFrameworkId = col.BudgetFrameworkId,
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
