using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class PasswordResetViewModel
    {
        
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }        
        [Required]
        public string NewPassword { get; set; }
    }
}
