using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class BeneficiaryStatusChangeViewModel
    {
        public List<long> BeneficiaryIds { get; set; }
        public long InstanceId { get; set; }
        public CollectionStatus Status { get; set; }
    }
}
