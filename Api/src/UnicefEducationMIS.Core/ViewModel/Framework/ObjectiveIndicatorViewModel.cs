using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class ObjectiveIndicatorViewModel
    {
        public long MonitoringFrameworkId { get; set; }
        public string Indicator { get; set; }
        public long Id { get; set; }
        public string Unit { get; set; }
        public int BaseLine { get; set; }
        public int Target { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationName  { get; set; }
        public int ReportingFrequencyId { get; set; }
        public string FrequencyName { get; set; }

        public List<ObjectiveIndicatorDynamicCellViewModel> DynamicCells { get; set; }
    }
}
