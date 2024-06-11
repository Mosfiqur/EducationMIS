using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class Union : BaseModel<int>
    {
        public Union()
        {
            Camps = new HashSet<Camp>();
        }
        public string Name { get; set; }
        public int UpazilaId { get; set; }
        public Upazila Upazila { get; set; }
        public ICollection<Camp> Camps { get; set; }
    }
}
