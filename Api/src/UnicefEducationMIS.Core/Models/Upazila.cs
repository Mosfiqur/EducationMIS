using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class Upazila : BaseModel<int>
    {
        public Upazila()
        {
            Unions = new HashSet<Union>();
        }
        public string Name { get; set; }
        public int DistrictId { get; set; }

        public District District { get; set; }
        public ICollection<Union> Unions { get; set; }
    }
}
