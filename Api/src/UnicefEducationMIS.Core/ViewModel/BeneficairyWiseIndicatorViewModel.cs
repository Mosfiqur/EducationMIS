using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class BeneficairyWiseIndicatorViewModel
    {
        public long BeneficiaryId { get; set; }
        public string BeneficairyName { get; set; }
        public long InstanceId { get; set; }
        public List<IndicatorGetViewModel> Indicators { get; set; }

    }
}
