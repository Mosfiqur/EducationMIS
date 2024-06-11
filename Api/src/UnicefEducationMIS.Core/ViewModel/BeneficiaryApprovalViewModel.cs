using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class BeneficiaryApprovalViewModel
    {
        public BeneficiaryApprovalViewModel()
        {
            BeneficiaryIds = new List<long>();
        }
        [Required]
        public List<long> BeneficiaryIds { get; set; }
        [Required]
        [Range(1, long.MaxValue)]
        public long InstanceId { get; set; }
    }
}
