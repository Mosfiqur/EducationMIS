using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class CreateIndicatorViewModel
    {
        public long Id { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public int ColumnOrder { get; set; }
        public EntityType EntityTypeId { get; set; }
        public long InstanceId { get; set; }

        public virtual EntityDynamicColumn EntityDynamicColumn { get; set; }
    }
}
