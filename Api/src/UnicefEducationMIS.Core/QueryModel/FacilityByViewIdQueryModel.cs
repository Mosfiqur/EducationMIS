using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class FacilityByViewIdQueryModel : BaseQueryModel
    {
        public long ViewId { get; set; }
        public long InstanceId { get; set; }
        public CollectionStatus CollectionStatus { get; set; }

        public FacilityFilterViewModel Filter { get; set; }
    }
}
