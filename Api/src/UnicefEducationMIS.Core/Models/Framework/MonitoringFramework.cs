using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models.Framework
{
    public class MonitoringFramework : BaseModel<long>
    {
        public string Objective { get; set; }
        public List<ObjectiveIndicator> ObjectiveIndicators { get; set; }
    }
}
