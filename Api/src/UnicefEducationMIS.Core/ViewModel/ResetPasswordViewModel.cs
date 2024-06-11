using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class ResetPasswordViewModel : IValidatableObject
    {
        [Required]
        [MinLength(6)]
        [MaxLength(16)]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Token { get; set; }    

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (!NewPassword.Equals(ConfirmPassword))
            {
                errors.Add(new ValidationResult("Password and Confirm Password doesn't match"));
            }
            return errors;
        }
    }
}
