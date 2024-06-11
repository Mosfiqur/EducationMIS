using System.ComponentModel.DataAnnotations;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class MonitoringFrameworkUpdateViewModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long Id { get; set; }
        [Required]
        public string Objective { get; set; }
    }
}