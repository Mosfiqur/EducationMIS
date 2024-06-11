using System;
using System.ComponentModel.DataAnnotations;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class ObjectiveIndicatorCreateViewModel
    {
        [Required]
        public string Indicator { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long MonitoringFrameworkId { get; set; }

        [Required]
        public string Unit { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int BaseLine { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Target { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int OrganizationId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int ReportingFrequencyId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

    }
}