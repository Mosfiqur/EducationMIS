namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class FacilitatorSummary
    {        
        public int RFemale { get; set; }
        public int RMale { get; set; }
        public int HFemale { get; set; }
        public int HMale { get; set; }
        public int Total => RFemale + RMale + HFemale + HMale;
    }
}
