using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class FacilityObjectViewModel
    {
        public long Id { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }

        public int UnionId { get; set; }
        public string UnionName { get; set; }
        public int UpazilaId { get; set; }
        public string UpazilaName { get; set; }
        public int? CampId { get; set; }
        public string CampName { get; set; }

        public FacilityType? FacilityType { get; set; }
        public FacilityStatus? FacilityStatus { get; set; }
        public TargetedPopulation TargetedPopulation { get; set; }
        public int? BlockId { get; set; }
        public string BlockName { get; set; }
        public string BlockCode { get; set; }
        public string ParaId { get; set; }
        public string ParaName { get; set; }

        public int ProgrammingPartnerId { get; set; }
        public string ProgrammingPartnerName { get; set; }
        public int ImplemantationPartnerId { get; set; }
        public string ImplemantationPartnerName { get; set; }
        public int? TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }

        public string Donors { get; set; }
        public string NonEducationPartner { get; set; }
        public string Latitude { get; set; }
        public string longitude { get; set; }
        public string CampSSID { get; set; }
        public string Remarks { get; set; }
        public CollectionStatus CollectionStatus { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveByUserName { get; set; }
        public string ApproveByUserEmail { get; set; }
        public string ApproveByUserPhone { get; set; }
    }
}
