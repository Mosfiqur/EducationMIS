using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Common;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class CreateFacilityViewModel : FacilityExtraColumnsViewModel
    {
        public long? Id { get; set; }
        [Required]
        public string Name { get; set; }
     
        public string FacilityCode { get; set; }
        [Required]
        public TargetedPopulation TargetedPopulation { get; set; }

        public FacilityStatus? FacilityStatus { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Donors { get; set; }
        public string NonEducationPartner { get; set; }

        [Required]
        public int ProgramPartnerId { get; set; }
        [Required]
        public int ImplementationPartnerId { get; set; }

        public int UpazilaId { get; set; }
        public int UnionId { get; set; }

        public int? CampId { get; set; }
        public string ParaName { get; set; }
        public string ParaId { get; set; }
        public int? BlockId { get; set; }

        public FacilityType? FacilityType { get; set; }
        public long InstanceId { get; set; }

        public int? TeacherId { get; set; }
        public Coordinate Position => new Coordinate(Latitude, Longitude);
    }
}
