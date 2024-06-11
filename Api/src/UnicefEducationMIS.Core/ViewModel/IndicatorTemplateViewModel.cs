using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class IndicatorTemplateViewModel
    {
        public long Id { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public int ColumnOrder { get; set; }
        public EntityType EntityTypeId { get; set; }
        public string ColumnName { get; set; }
        public string EntityColumnName { get; set; }
        public bool IsFixed { get; set; }
        public bool? IsMultiValued { get; set; }
        public ColumnDataType DataType { get; set; }
        public ListDataTypeViewModel ListObject { get; set; }
     
    }
}
