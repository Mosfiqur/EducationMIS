namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class DisabilitySummary
    {
        public DisabilitySummary()
        {

        }
        public string CampName { get; set; }
        public string AgeGroup { get; set; }
        public int RFemale { get; set; }
        public int RMale { get; set; }
        public int HFemale { get; set; }
        public int HMale { get; set; }
        public int Total => RFemale + RMale + HFemale + HMale;
        public int AgeGroupId { get; set; }
        public int CampId { get; set; }
    }
}
