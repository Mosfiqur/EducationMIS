using System.Collections.Generic;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class BeneficiaryViewModel : BeneficairyObjectViewModel
    {

        public BeneficiaryViewModel()
        {
            Properties = new List<PropertiesInfo>();
        }
        public List<PropertiesInfo> Properties { get; set; }
        public long InstanceId { get; set; }
        public string InstanceName { get; set; }

    }
}
