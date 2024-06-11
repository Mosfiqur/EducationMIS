using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class BeneficiaryDataForApproval
    {
        public List<BeneficiaryViewModel> CollectedData { get; set; }
        public List<BeneficiaryViewModel> DeactivateData { get; set; }

    }
}
