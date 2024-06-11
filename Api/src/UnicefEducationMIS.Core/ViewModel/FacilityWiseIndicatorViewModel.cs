using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class FacilityWiseIndicatorViewModel
    {
        public long FacilityId { get; set; }
        public string FacilityName { get; set; }
        public long InstanceId { get; set; }
        public List<IndicatorGetViewModel> Indicators { get; set; }
    }
}
