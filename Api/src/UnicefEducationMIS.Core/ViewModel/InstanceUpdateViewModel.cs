using System;
using System.ComponentModel.DataAnnotations;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class InstanceUpdateViewModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long Id { get; set; }
        [Required]
        [Range(1, long.MaxValue)]
        public long ScheduleId { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
