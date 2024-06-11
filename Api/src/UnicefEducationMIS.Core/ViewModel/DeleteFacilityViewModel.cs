using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class DeleteFacilityViewModel
    {
        public DeleteFacilityViewModel()
        {
            FacilityIds = new List<long>();
        }
        public long InstanceId { get; set; }
        public List<long> FacilityIds { get; set; }
    }
}
