using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class StudentEnrollmentSummary
    {
        public int AgeGroupId { get; set; }
        public string AgeGroup { get; set; }
        public int RFemale { get; set; }
        public int RMale { get; set; }
        public int HFemale { get; set; }
        public int HMale { get; set; }
        public int Total => RFemale + RMale + HFemale + HMale;
    }

    public class EnrollmentSummaryWithoutGender
    {
        public string AgeGroup { get; set; }
        public int RefugeeTotal { get; set; }
        public int HostTotal { get; set; }
        public int Total { get; set; }
    }
}
