using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class FacilityViewModel : FacilityObjectViewModel
    {

        public FacilityViewModel()
        {
            Properties = new List<PropertiesInfo>();
        }
        public List<PropertiesInfo> Properties { get; set; }
        public string InstanceName { get; set; }
        public long InstanceId { get; set; }
        public string TeacherEmail { get; set; }
    }
}
