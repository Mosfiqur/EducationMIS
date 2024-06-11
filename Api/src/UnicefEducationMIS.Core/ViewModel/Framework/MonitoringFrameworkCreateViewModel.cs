using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class MonitoringFrameworkCreateViewModel
    {
        [Required]
        public string Objective { get; set; }       
    }
}
