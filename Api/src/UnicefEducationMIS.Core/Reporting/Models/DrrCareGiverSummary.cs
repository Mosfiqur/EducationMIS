namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class DrrCareGiverSummary
    {
        public string CampName { get; set; }
        public string PP { get; set; }
        public string IP { get; set; }
        public int NumOfDrr { get; set; }
        public int NumOfEduCommitee { get; set; }
        public int FemaleRCareGiver { get; set; }
        public int MaleRCareGiver { get; set; }

        public int FemaleHCareGiver { get; set; }
        public int MaleHCareGiver { get; set; }

        public int RGirls3_24 { get; set; }
        public int RBoys3_24 { get; set; }

        public int BenefittingFromFood { get; set; }
        public int BenefittingFromClassRoom{ get; set; }
        public int BenefittingFood { get; set; }

        public int TotalFacility { get; set; }

    }
}
