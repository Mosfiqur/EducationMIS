using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models.Views
{
    public class FacilityView
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FacilityCode { get; set; }
        public TargetedPopulation TargetedPopulation { get; set; }
        public FacilityStatus? FacilityStatus { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Donors { get; set; }
        public string NonEducationPartner { get; set; }
        public int ProgramPartnerId { get; set; }
        public int ImplementationPartnerId { get; set; }
        public string Remarks { get; set; }
        public int UpazilaId { get; set; }
        public int UnionId { get; set; }
        public int? CampId { get; set; }
        public string ParaName { get; set; }
        public int? BlockId { get; set; }
        public int? TeacherId { get; set; }
        public FacilityType? FacilityType { get; set; }
        public long InstanceId { get; set; }
    }
}
