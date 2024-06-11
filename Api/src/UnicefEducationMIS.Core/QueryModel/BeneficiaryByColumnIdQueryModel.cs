using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class BeneficiaryByColumnIdQueryModel : BaseQueryModel
    {

        public List<long> ColumnIds { get; set; }
        public long InstanceId { get; set; }

    }
}
