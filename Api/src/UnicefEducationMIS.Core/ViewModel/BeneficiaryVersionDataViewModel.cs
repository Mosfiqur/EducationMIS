using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class BeneficiaryVersionDataViewModel
    {
        public BeneficiaryVersionDataViewModel()
        {
            DynamicCells = new List<DynamicCellViewModel>();
        }
        public string UnhcrId { get; set; }
        public string Name { get; set; }
        public long FacilityId { get; set; }

        public long BeneficiaryId { get; set; }
        public long InstanceId { get; set; }
        public List<DynamicCellViewModel> DynamicCells { get; set; }
    }
}
