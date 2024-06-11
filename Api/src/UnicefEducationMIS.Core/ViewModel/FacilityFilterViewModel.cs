using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class FacilityFilterViewModel
    {
        public int? UpazilaId { get; set; }
        public int? UnionId { get; set; }
        public TargetedPopulation? TargetedPopulation { get; set; }
        public FacilityType? FacilityType { get; set; }
        public FacilityStatus? FacilityStatus { get; set; }
        public List<EducationSectorPartner> ProgramPartner { get; set; }
        public List<EducationSectorPartner> ImplementationPartner { get; set; }
        public List<TeacherViewModel> Teachers { get; set; }
        public string SearchText { get; set; }


    }
}
