using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class IndicatorSelectViewModel
    {
        public long Id { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public int ColumnOrder { get; set; }
        public string IndicatorName { get; set; }
        public string IndicatorNameInBangla { get; set; }
        public ColumnDataType ColumnDataType { get; set; }
        public bool? IsMultivalued { get; set; }
        public long? ColumnListId { get; set; }
        public ListDataType ListObject { get; set; }
        public ICollection<ListItem> ListItems { get; set; }


    }
}
