using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
   public class GridDetailsViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public int ColumnOrder { get; set; }
    }
}
