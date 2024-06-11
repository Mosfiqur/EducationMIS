using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class BeneficiaryGetAllQueryModel : BaseQueryModel
    {
        public BeneficiaryFilterViewModel Filter { get; set; }
    }
}
