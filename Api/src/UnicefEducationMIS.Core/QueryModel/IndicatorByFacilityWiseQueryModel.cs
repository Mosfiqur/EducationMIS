using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class IndicatorByFacilityWiseQueryModel : BaseQueryModel
    {
        public long InstanceId { get; set; }
        public long FacilityId { get; set; }
    }
}
