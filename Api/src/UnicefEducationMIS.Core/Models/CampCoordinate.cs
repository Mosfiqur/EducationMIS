using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class CampCoordinate : BaseModel<long>
    {
        public int CampId { get; set; }
        public int SequenceNumber { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public Camp Camp { get; set; }
    }
}
