using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class ScheduleInstanceQueryModel : BaseQueryModel
    {
        public EntityType ScheduleFor { get; set; }        
    }
}
