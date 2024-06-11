using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Extensions;

namespace UnicefEducationMIS.Core.Models.Common
{
    public class Coordinate
    {
        public Coordinate()
        {

        }
        public Coordinate(decimal lat, decimal lng)
        {
            Latitude = lat;
            Longitude = lng;
        }
        public Coordinate(string lat, string lng)
            :this(decimal.TryParse(lat, out var latitude) ? latitude: SystemDefaults.InvalidLatitude, 
                  decimal.TryParse(lng, out var longitude) ? longitude : SystemDefaults.InvalidLongitude)
        {
        }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public bool IsValid()
        {
            return Latitude.Between(-90, 90) && Longitude.Between(-180, 180);
        }

        public bool WithinTheBound()
        {
            // The bound validation will be more extensive
            // Later we will make sure that each coordinate is within a certain boundary
            // 
            return Latitude != 0 && Longitude != 0;
        }
    }
}
