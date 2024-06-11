using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class IndicatorGetViewModel
    {
       // public long Id { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public int ColumnOrder { get; set; }
        public string ColumnName { get; set; }
        public string ColumnNameInBangla { get; set; }
        public bool? IsMultiValued { get; set; }
        public ColumnDataType ColumnDataType { get; set; }

        public long? ListObjectId { get; set; }
        public string ListObjectName { get; set; }

        public ICollection<ListItemViewModel> ListItems { get; set; }
        public DateTime DataCollectionDate { get; set; }

        public List<string> Values { get; set; }
        public CollectionStatus? Status { get; set; }

    }
}
