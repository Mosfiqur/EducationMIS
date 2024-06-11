using System;
using System.ComponentModel.DataAnnotations;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class BudgetFrameworkCreateViewModel
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int ProjectId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int DonorId { get; set; }
        public string ProjectName { get; set; }
        public string DonorName { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}