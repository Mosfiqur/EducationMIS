using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public class JrpParameterInfo:BaseModel<int>
    {
        public string Name { get; set; }


        public long TargetId { get; set; }
        public long IndicatorId { get; set; }


        public ObjectiveIndicator ObjectiveIndicator { get; set; }
        public EntityDynamicColumn EntityDynamicColumn { get; set; }
    }
}
