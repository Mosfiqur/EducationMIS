using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Core.ViewModel.User
{
    public class UserDynamicCellViewModel : EntityDynamicCellViewModel
    {
        public UserDynamicCellViewModel()
        {
            Values = new List<string>();
        }
        public int UserId { get; set; }

        public static Expression<Func<UserDynamicCell, UserDynamicCellViewModel>> FromModel = col =>
            new UserDynamicCellViewModel
            {
                Id = col.Id,
                ColumnName = col.EntityDynamicColumn.Name,
                EntityDynamicColumnId = col.EntityDynamicColumnId,
                UserId = col.UserId,
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