using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class InstanceViewModel
    {
        public long Id { get; set; }
        public long ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public string Title { get; set; }
        public DateTime DataCollectionDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public InstanceStatus Status { get; set; }
    }
}
