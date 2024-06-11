using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Models
{
    public class BeneficiaryDataCollectionStatus : BaseModel<long>
    {
        public long BeneficiaryId { get; set; }
        public long InstanceId { get; set; }
        public CollectionStatus Status { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }

        public Instance ScheduleInstance { get; set; }
        public Beneficiary Beneficiary { get; set; }

    }
}
