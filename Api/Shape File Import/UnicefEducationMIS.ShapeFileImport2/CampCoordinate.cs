using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicefEducationMIS.ShapeFileImport2
{
    public class CampCoordinate
    {
        public long Id { get; set; }
        public int CampId { get; set; }
        public int SequenceNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
