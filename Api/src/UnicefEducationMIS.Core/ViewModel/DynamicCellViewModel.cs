using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.Extensions;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class DynamicCellViewModel
    {
       // public long Id { get; set; }
        public List<string> Value { get; set; }

        public long EntityDynamicColumnId { get; set; }

        public DynamicCellViewModel()
        {
            Value = new List<string>();
        }
        public DynamicCellViewModel(long columnId, List<string> values)
        {
            Value = new List<string>();
            Value.AddRange(values);
            EntityDynamicColumnId = columnId;
        }

        public DynamicCellViewModel(long columnId, string value):this(columnId, value.AsList()){ }
        public DynamicCellViewModel(long columnId, int value):this(columnId, value.ToString().AsList()) { }
        public DynamicCellViewModel(long columnId, long value) : this(columnId, value.ToString().AsList()) { }

        public int ToIntId()
        {
            return int.Parse(Value.FirstOrDefault() ?? "0");
        }

        public long ToLongId()
        {
            return long.Parse(Value.FirstOrDefault() ?? "0");
        }
    }
}
