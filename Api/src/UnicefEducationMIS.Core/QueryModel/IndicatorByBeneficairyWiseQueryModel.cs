using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class IndicatorByBeneficairyWiseQueryModel:BaseQueryModel
    {
        public long  InstanceId { get; set; }
        public long BeneficiaryId { get; set; }

    }
}
