using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public class Facility : BaseModel<long>
    {
        public Facility()
        {        
            FacilityDynamicCells = new HashSet<FacilityDynamicCell>();
            FacilityDataCollectionStatus = new HashSet<FacilityDataCollectionStatus>();
        }
       
        public ICollection<FacilityDynamicCell> FacilityDynamicCells { get; set; }
        public ICollection<FacilityDataCollectionStatus> FacilityDataCollectionStatus { get; set; }
        
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string FacilityCode { get; set; }
        [NotMapped]
        public int CampId { get; set; }
        [NotMapped]
        public TargetedPopulation TargetedPopulation { get; set; }
        [NotMapped]
        public string Latitude { get; set; }
        [NotMapped]
        public string Longitude { get; set; }
    }
}
