using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class TargetFrameworkCreateViewModel : IValidatableObject
    {
        public int? CampId { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int AgeGroupId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int PeopleInNeed { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Target { get; set; }
        [Required]
        [Range(1900, 3000)]
        public int StartYear { get; set; }
        [Required]
        public Month StartMonth { get; set; }
        [Required]
        [Range(1900, 3000)]
        public int EndYear { get; set; }
        [Required]
        public Month EndMonth { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int UpazilaId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int UnionId { get; set; }
        [Required]
        public TargetedPopulation TargetedPopulation { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            if (TargetedPopulation.NoneMatched())
            {
                errors.Add(new ValidationResult(Messages.TargetPopulationInvalid));
            }

            if (TargetedPopulation != TargetedPopulation.Host_Communities && !CampId.HasValue)
            {
                errors.Add(new ValidationResult(Messages.CampMandatory));
            }
            return errors;
        }
    }

}