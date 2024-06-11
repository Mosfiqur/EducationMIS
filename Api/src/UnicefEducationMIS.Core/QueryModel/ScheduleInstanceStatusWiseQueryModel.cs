using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class ScheduleInstanceStatusWiseQueryModel : BaseQueryModel
    {
        public EntityType ScheduleFor { get; set; }
        public int InstanceStatus { get; set; }

    }
}
