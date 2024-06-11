using System;
using System.Collections.Generic;
using System.Linq;
using UnicefEducationMIS.Core.Models.Common;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel.Dashboard
{

    public class LearningCenterViewModel
    {
        public int CampId { get; set; }
        public string CampName { get; set; }
        public decimal FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }
        public Coordinate Position { get; set; }
        public int NumberOfBeneficiaries { get; set; }
        public int Radius { get; set; }
        public TargetedPopulation TargetedPopulation { get; set; }
        public string PpName { get; set; }
        public string IpName { get; set; }
        public string UpazilaName { get; set; }
        public string UnionName { get; set; }


        public LearningCenterViewModel()
        {
  
        }
              
        //public void CalculateCenter()
        //{
        //    if(this.CampCoordinates.Count == 0)
        //    {
        //        return;
        //    }
        //    var minLat = this.CampCoordinates.Select(x => x.Latitude).Min();
        //    var minLng = this.CampCoordinates.Select(x => x.Longitude).Min();

        //    var maxLat = this.CampCoordinates.Select(x => x.Latitude).Max();
        //    var maxLng = this.CampCoordinates.Select(x => x.Longitude).Max();

        //    Position = new Coordinate()
        //    {
        //        Latitude = minLat + (maxLat - minLat) / 2,
        //        Longitude = minLng + (maxLng - minLng) / 2
        //    };            
        //}
    }
}
