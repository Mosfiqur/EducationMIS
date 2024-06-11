using System.Collections.Generic;

namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class MonitoringFrameworkSummary
    {
        public MonitoringFrameworkSummary()
        {
            Indicators = new List<ObjectiveIndicatorSummary>();
        }
        public string Objective { get; set; }
        public List<ObjectiveIndicatorSummary> Indicators { get; set; }
    }
}
