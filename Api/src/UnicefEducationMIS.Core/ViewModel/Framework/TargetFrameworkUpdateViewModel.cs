using System.ComponentModel.DataAnnotations;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class TargetFrameworkUpdateViewModel : TargetFrameworkCreateViewModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long Id { get; set; }
    }
}