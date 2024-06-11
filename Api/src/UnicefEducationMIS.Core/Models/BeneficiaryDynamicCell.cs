using System;
using System.Collections.Generic;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public partial class BeneficiaryDynamicCell: BaseModel<long>
    {

        public string Value { get; set; }

        public long EntityDynamicColumnId { get; set; }
        public long BeneficiaryId { get; set; }

        public long InstanceId { get; set; }
        public CollectionStatus? Status { get; set; } = CollectionStatus.NotCollected;

        public virtual Beneficiary Beneficiary { get; set; }
        public virtual EntityDynamicColumn EntityDynamicColumn { get; set; }
        public Instance Instance { get; set; }


    }
}
