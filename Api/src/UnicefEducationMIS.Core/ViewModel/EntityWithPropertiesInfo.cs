using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class EntityWithPropertiesInfo : PropertiesInfo
    {
        public long EntityId { get; set; }
        public string Value { get; set; }
        public long InstanceId { get; set; }

    }
}
