using System.Collections.Generic;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class PropertiesInfo
    {
        public long EntityColumnId { get; set; }
        public string ColumnName { get; set; }
        public string Properties { get; set; }
        public string ColumnNameInBangla { get; set; }
        public int ColumnOrder { get; set; }
        public List<string> Values { get; set; }
        public bool IsVersionColumn { get; set; }
        public bool IsFixed { get; set; }
        public bool IsMultiValued { get; set; }
        public ColumnDataType DataType { get; set; }
        public long? ColumnListId { get; set; }
        public string ColumnListName { get; set; }
        public List<ListItemViewModel> ListItem { get; set; }
        public CollectionStatus? Status { get; set; }

    }
}
