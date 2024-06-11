using System.ComponentModel.DataAnnotations;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class ScheduleUpdateViewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        [Required]
        public string ScheduleName { get; set; }
        public string Description { get; set; }
    }
}