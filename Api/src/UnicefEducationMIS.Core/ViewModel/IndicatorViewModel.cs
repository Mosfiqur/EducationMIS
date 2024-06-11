using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
  public  class IndicatorViewModel
    {
        public long EntityDynamicColumnId { get; set; }
        public int ColumnOrder { get; set; }
        public EntityType EntityTypeId { get; set; }
    }
}
