using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class FacilityGetAllQueryModel : BaseQueryModel
    {
        public FacilityFilterViewModel Filter { get; set; }
    }
}
