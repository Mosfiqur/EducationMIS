using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class IndicatorsByInstanceQueryModel : BaseQueryModel
    {
        public long InstanceId { get; set; }

        public static IndicatorsByInstanceQueryModel GetAllQuery(long instanceId)
        {
            return new IndicatorsByInstanceQueryModel
            {
                InstanceId = instanceId,
                PageNo = 1,
                PageSize = SystemDefaults.MaxPageSize
            };
        }
    }
}
