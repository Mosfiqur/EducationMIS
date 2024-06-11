namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class GapAnalysisReport
    {
        public string CampName { get; set; }
        public int Target { get; set; }
        public int Outreach { get; set; }
        public int Gap { get; set; }
        public string Ratio { get; set; }
    }
}
