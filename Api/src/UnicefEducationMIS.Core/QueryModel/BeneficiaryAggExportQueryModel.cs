using System.Collections.Generic;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class BeneficiaryAggExportQueryModel
    {
        public BeneficiaryAggExportQueryModel()
        {
            InstanceIds = new List<long>();
        }
        public List<long> InstanceIds { get; set; }
    }
}