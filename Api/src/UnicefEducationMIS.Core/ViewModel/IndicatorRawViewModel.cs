using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class IndicatorRawViewModel
    {
        public long Id { get; set; }
        public int ColumnOrder { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public EntityType EntityTypeId { get; set; }

        public string EntityColumnName { get; set; }
        public bool IsFixed { get; set; }
        public bool? IsMultiValued { get; set; }
        public ColumnDataType DataType { get; set; }
        public long? ListObjectId { get; set; }
        public string ListObjectName { get; set; }

        public long ListItemId { get; set; }
        public string ListItemTitle { get; set; }
        public int ListItemValue { get; set; }

        public long InstanceId { get; set; }
        public DateTime DataCollectionDate { get; set; }

    }
}
