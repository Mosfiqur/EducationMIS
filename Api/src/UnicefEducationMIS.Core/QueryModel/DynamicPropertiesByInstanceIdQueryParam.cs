using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class DynamicPropertiesByInstanceIdQueryParam : BaseQueryModel
    {
        public EntityType EntityTypeId { get; set; }
        public long InstanceId { get; set; }

    }
}
