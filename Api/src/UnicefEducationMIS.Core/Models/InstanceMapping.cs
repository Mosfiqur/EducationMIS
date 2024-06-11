using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class InstanceMapping :BaseModel<int>
    {
        public long BeneficiaryInstanceId { get; set; }
        public long FacilityInstanceId { get; set; }

    }
}
