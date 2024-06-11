using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicefEducationMIS.ShapeFileImport2
{
    public class CampArea
    {
        public string SSID { get; set; }
        public string CampName { get; set; }
        public List<GpsCoordinate> Points { get; set; }
    }

    public class GpsCoordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
