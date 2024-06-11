using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class District:BaseModel<int>
    {
        public District()
        {
            Upazilas = new HashSet<Upazila>();
        }
        public string Name { get; set; }

        public ICollection<Upazila> Upazilas { get; set; }
    }
}
