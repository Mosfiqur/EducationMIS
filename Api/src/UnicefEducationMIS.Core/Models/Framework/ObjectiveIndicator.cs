using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;

namespace UnicefEducationMIS.Core.Models.Framework
{
    public class ObjectiveIndicator : BaseModel<long>
    {
        public string Indicator { get; set; }
        public string Unit { get; set; }
        public int BaseLine { get; set; }
        public int Target { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int OrganizationId { get; set; }
        public EducationSectorPartner Organization { get; set; }
        public ReportingFrequency ReportingFrequency { get; set; }
        public int ReportingFrequencyId { get; set; }

        public long MonitoringFrameworkId { get; set; }
        public MonitoringFramework MonitoringFramework { get; set; }

        public ObjectiveIndicator()
        {
            DynamicCells = new HashSet<MonitoringFrameworkDynamicCell>();
            JrpParameterInfos = new HashSet<JrpParameterInfo>();
        }
        public ICollection<MonitoringFrameworkDynamicCell> DynamicCells { get; set; }
        public ICollection<JrpParameterInfo> JrpParameterInfos { get; set; }
    }
}