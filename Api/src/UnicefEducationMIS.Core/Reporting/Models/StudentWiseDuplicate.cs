using System.Collections.Generic;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class StudentWiseDuplicate
    {

        public string ProgressId { get; set; }
        public string BeneficiaryName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        
        public int EnrollmentCount { get; set; }

        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public string BlockName { get; set; }
        public string CampName { get; set; }
        public string ProgrammingPartnerName { get; set; }
        public string ImplementationPartnerName { get; set; }

        public string LevelOfStudy { get; set; }

    }
}
