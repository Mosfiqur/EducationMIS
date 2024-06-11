using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class FacilityAggExportQueryModel
    {
        public FacilityAggExportQueryModel()
        {
            InstanceIds = new List<long>();
        }
        public List<long> InstanceIds { get; set; }
    }
}
