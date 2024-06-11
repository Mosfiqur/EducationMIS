using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel.Import
{
    public class FacilityImportViewModel : FacilityExtraColumnsViewModel
    {
        public long Id { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public TargetedPopulation TargetedPopulation { get; set; }
        public FacilityStatus FacilityStatus { get; set; }

        public int ProgramPartnerId { get; set; }
        public string ProgramPartner { get; set; }
        public int ImplementationPartnerId { get; set; }
        public string ImplementationPartner { get; set; }

        public int UpazilaId { get; set; }
        public int UnionId { get; set; }

        public string ParaName { get; set; }
        public string ParaId { get; set; }

        public int? BlockId { get; set; }
        public int? CampId { get; set; }

        public string BlockCode { get; set; }
        public string CampSsId { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Donors { get; set; }
        public string NonEducationPartner { get; set; }

        public FacilityType? FacilityType { get; set; }
        
    }
}
