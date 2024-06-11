using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class SubmittedFacilityQueryModel : BaseQueryModel
    {
        public CollectionStatus? CollectionStatus { get; set; }
        public long InstanceId { get; set; }

        public FacilityFilterViewModel Filter { get; set; }
    }
}
