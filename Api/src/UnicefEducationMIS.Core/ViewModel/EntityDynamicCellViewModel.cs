using System.Collections.Generic;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class EntityDynamicCellViewModel
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public string ColumnName { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public ColumnDataType DataType { get; set; }
        public List<string> Values { get; set; }
        public ListDataTypeViewModel ListType { get; set; }
    }
}