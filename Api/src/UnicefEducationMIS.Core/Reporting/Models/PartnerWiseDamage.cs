namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class PartnerWiseDamage
    {
        public long FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int PpId { get; set; }
        public string PpName { get; set; }
        public int IpId { get; set; }
        public string IpName { get; set; }
        public int Affected { get; set; }
        public int RepairStarted { get; set; }
        public int RepairFinished { get; set; }
        public int RepairOngoing { get; set; }
    }
}
