namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class DamageSummary
    {
        public int Affected { get; set; }
        public int RepairStarted { get; set; }
        public int RepairFinished { get; set; }
        public int RepairOngoing { get; set; }
        public int TotalAffected { get; set; }
        public int TotalRepaired { get; set; }

        public string InstanceName { get; set; }
    }
}
