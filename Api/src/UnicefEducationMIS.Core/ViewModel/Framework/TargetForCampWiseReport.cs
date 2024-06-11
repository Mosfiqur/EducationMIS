using UnicefEducationMIS.Core.ValueObjects;
using AgeGroup = UnicefEducationMIS.Core.Models.Framework.AgeGroup;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class TargetForCampWiseReport
    {
        public int? CampId { get; set; }
        public Gender Gender { get; set; }
        public AgeGroup AgeGroup { get; set; }
        public int PeopleInNeed { get; set; }
        public int Target { get; set; }

        public int StartYear { get; set; }

    }
}
