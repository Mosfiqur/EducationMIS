using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class FacilityApprovalViewModel
    {
        public FacilityApprovalViewModel()
        {
            FacilityIds = new List<long>();
        }
        [Required]
        public List<long> FacilityIds { get; set; }
        [Required]
        [Range(1, long.MaxValue)]
        public long InstanceId { get; set; }
    }
}
