using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class BeneficiaryByViewIdQueryModel: BaseQueryModel
    {
        public long ViewId { get; set; }
        public long InstanceId { get; set; }
        public CollectionStatus CollectionStatus { get; set; }
        public BeneficiaryFilterViewModel Filter { get; set; }
    }
}
