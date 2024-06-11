namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class StudyLevelWiseSummary
    {
        public string PP { get; set; }
        public string IP { get; set; }
        public int RGLevel1 { get; set; }
        public int RBLevel1 { get; set; }

        public int RGLevel2 { get; set; }
        public int RBLevel2 { get; set; }

        public int RGLevel3 { get; set; }
        public int RBLevel3 { get; set; }

        public int RGLevel4 { get; set; }
        public int RBLevel4 { get; set; }

        public int OtherStudyStatus { get; set; }
        public int TotalFacility { get; set; }
        public int TotalEnrollment { get; set; }
    }
}
