using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class DynamicPropertiesViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string NameInBangla { get; set; }
        public ColumnDataType ColumnDataType { get; set; }
        public string Description { get; set; }
        public bool? IsMultiValued { get; set; }

        public ListDataType ListObject { get; set; }
        public ICollection<ListItem> ListItems { get; set; }

    }
}
