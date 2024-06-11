using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public class FacilityDynamicCell:BaseModel<long>
    {
        public string Value { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public long FacilityId { get; set; }
        public long InstanceId { get; set; }
        public CollectionStatus? Status { get; set; } = CollectionStatus.NotCollected;

        public virtual Facility Facility { get; set; }
        public virtual EntityDynamicColumn EntityDynamicColumn { get; set; }
        public Instance Instance { get; set; }
    }
}
