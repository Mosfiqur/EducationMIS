using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
  public  class JrpDashboardFilterViewModel
    {

        //public int? Year { get; set; }
        //public int? Month { get; set; }

        public long? BeneficiaryInstanceId { get; set; }
        public long? FacilityInstanceId { get; set; }

    
        //facility 
        public long? ProgramPartnerId { get; set; }
        public long? ImplementationPartnerId { get; set; }

        //both
        public long? CampId { get; set; }
        
        //beneficiary
        public Gender? Gender { get; set; }
        public bool? Disability { get; set; }
        public TargetedPopulation? TargetedPopulationId { get; set; }
        public int? Age { get; set; }
        public LevelOfStudy? Level { get; set; }


    }
}
