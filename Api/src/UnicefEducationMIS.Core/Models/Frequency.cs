using System;
using System.Collections.Generic;

namespace UnicefEducationMIS.Core.Models
{
    public class Frequency
    {        
        public Frequency()
        {
            DaysOfWeek = new List<DayOfWeek>();
        }
        public long ScheduleId { get; set; }
        public int Interval { get; set; }
        public int? Day { get; set; }
        public int? Month { get; set; }
        public List<DayOfWeek> DaysOfWeek { get; set; }

        public Schedule Schedule { get; set; }
    }
}
