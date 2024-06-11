using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Reporting.Models
{
    public class FacilityIndicatorWithValue
    {
        public long EntityDynamicColumnId { get; set; }
        public string EntityDynamicColumnName { get; set; }
        public int? CampId { get; set; }
        public string CampName { get; set; }
        public int ProgramPartnerId { get; set; }
        public string ProgramPartenrName { get; set; }
        public int ImplementationPartnerId { get; set; }
        public string ImplementationPartnerName { get; set; }

        public long FacilityId { get; set; }
        public string Value { get; set; }

    }
}
