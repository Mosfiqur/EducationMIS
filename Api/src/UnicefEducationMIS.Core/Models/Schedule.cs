using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public class Schedule : BaseModel<long>
    {
        public Schedule()
        {
            Instances = new HashSet<Instance>();
            Status = ScheduleStatus.Pending;
        }
        public string ScheduleName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public bool IsDeleted { get; set; }
        public ScheduleStatus Status { get; set; }
        public EntityType ScheduleFor { get; set; }
        public Frequency Frequency { get; set; }
        public ICollection<Instance> Instances { get; set; }
    }
}
