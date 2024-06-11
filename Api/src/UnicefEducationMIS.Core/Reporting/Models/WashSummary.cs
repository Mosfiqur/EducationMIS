namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class WashSummary
    {
        public int? CampId { get; set; }
        public string CampName { get; set; }
        public string PP { get; set; }
        public string IP { get; set; }
        public int CommunityLatrines { get; set; }
        public int BoysLatrines { get; set; }
        public int GirlsLatrines { get; set; }
        public int HandWashingStations { get; set; }
    }
}
