using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class PasswordChangeViewModel : IValidatableObject
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(16)]
        [RegularExpression(@"(?=.*[^A-Za-z\d])(?=.*[A-Z])(?=.*\d).{8,}",ErrorMessage= "At least 8 character with min one digit, one upper case and one special character.")]
        public string NewPassword { get; set; }
        [Required]        
        public string ConfirmPassword { get; set; }

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
