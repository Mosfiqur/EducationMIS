using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class FacilityDynamicCellViewModel
    {
        //public long Id { get; set; }
        public string Value { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public long FacilityId { get; set; }
        public long InstanceId { get; set; }
    }
}
