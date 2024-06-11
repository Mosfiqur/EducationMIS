using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class JrpDashboardFacilityRawData
    {
        public int? CampId { get; set; }

        public TargetedPopulation TargetedPopulation { get; set; }
        public int ProgramPartnerId { get; set; }
        public int ImplementationPartnerId { get; set; }

        public string Value { get; set; }
    }
}
