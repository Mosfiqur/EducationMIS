using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class FailityWiseDuplicate
    {
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public string BlockName { get; set; }
        public string CampName { get; set; }
        public string ProgrammingPartnerName { get; set; }
        public string ImplementationPartnerName { get; set; }
        public int UniqueStudents { get; set; }
        public int DuplicateStudents { get; set; }
        public int TotalStudents { get; set; }        
    }
}
