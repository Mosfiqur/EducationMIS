using System.ComponentModel.DataAnnotations;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class BudgetFrameworkUpdateViewModel : BudgetFrameworkCreateViewModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long Id { get; set; }
    }
}