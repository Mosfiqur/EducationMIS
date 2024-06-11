using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class InstanceIndicator:BaseModel<long>
    {
        public long EntityDynamicColumnId { get; set; }
        public long InstanceId { get; set; }
        public int ColumnOrder { get; set; }

        public EntityDynamicColumn EntityDynamicColumn { get; set; }
        public Instance Instance { get; set; }

    }
}
