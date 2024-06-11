using System.Collections.Generic;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class SummaryReport
    {
        public SummaryReport()
        {
            EnrollmentSummary = new List<StudentEnrollmentSummary>();
            FacilityTypewiseStatus = new List<FacilityTypewiseStatus>();
            DisabilitySummary = new List<DisabilitySummary>();
            WashSummary = new List<WashSummary>();
            MonitoringFrameworkSummary = new List<MonitoringFrameworkSummary>();            
        }

        public Instance Instance { get; set; }
        public List<StudentEnrollmentSummary> EnrollmentSummary { get; set; }
        public List<FacilitatorSummary> FacilitatorSummary { get; set; }
        public List<FacilityTypewiseStatus> FacilityTypewiseStatus { get; set; }
        public List<JRP> RefugeeJrps { get; set; }
        public List<JRP> HostJrps { get; set; }
        public List<JRP> JrpSummary { get; set; }
        public List<DisabilitySummary> DisabilitySummary { get; set; }
        public List<WashSummary> WashSummary { get; set; }
        public List<StudyLevelWiseSummary> StudyLevelWiseSummary { get; set; }
        public List<DrrCareGiverSummary> DrrCareGiverSummary { get; set; }

        public List<MonitoringFrameworkSummary> MonitoringFrameworkSummary { get; set; }  
    }
}
