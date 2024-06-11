using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models.Framework
{
    public class MonitoringFrameworkDynamicCell : BaseModel<long>
    {
        public long ObjectiveIndicatorId { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public string Value { get; set; }

        public virtual ObjectiveIndicator ObjectiveIndicator { get; set; }
        public virtual EntityDynamicColumn EntityDynamicColumn { get; set; }
    }
}
