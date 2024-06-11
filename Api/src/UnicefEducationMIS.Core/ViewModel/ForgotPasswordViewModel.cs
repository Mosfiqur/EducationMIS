using System.ComponentModel.DataAnnotations;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}
