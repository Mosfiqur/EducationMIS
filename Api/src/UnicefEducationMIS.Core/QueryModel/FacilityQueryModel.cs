using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class FacilityQueryModel:BaseQueryModel
    {
        public FacilityQueryModel() { }
        public FacilityQueryModel(long instanceId)
        {
            InstanceId = instanceId;
        }
        public long InstanceId { get; set; }

    }
}
