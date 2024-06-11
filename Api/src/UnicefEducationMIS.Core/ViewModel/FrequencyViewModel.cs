using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class FrequencyViewModel
    {
        public long ScheduleId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Interval { get; set; }
        public int? Day { get; set; }
        public int? Month { get; set; }
        public List<DayOfWeek> DaysOfWeek { get; set; }
    }
}
