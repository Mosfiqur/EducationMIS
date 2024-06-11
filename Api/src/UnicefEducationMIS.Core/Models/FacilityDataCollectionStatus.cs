using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public class FacilityDataCollectionStatus : BaseModel<long>
    {
        public long FacilityId { get; set; }
        public long InstanceId { get; set; }
        public CollectionStatus Status { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }

        public Instance ScheduleInstance { get; set; }
        public Facility Facility { get; set; }
    }
}
