using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class BeneficiaryFilterViewModel
    {
        public DateRangeViewModel DateOfBirth { get; set; }
        public DateRangeViewModel EnrolmentDate { get; set; }
        public List<FacilityViewModel> Facilities { get; set; }
        public List<Camp> Camps { get; set; }
        public Gender? Sex { get; set; }

        public LevelOfStudy? LevelOfStudy { get; set; }
        public bool? Disable { get; set; }
        public string SearchText { get; set; }

    }
}
