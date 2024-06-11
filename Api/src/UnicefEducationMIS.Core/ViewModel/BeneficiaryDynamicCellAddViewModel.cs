using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
   public class BeneficiaryDynamicCellAddViewModel
    {
        public BeneficiaryDynamicCellAddViewModel()
        {
            DynamicCells = new List<DynamicCellViewModel>();
        }
     
        public long BeneficiaryId { get; set; }
        public long InstanceId { get; set; }
        public List<DynamicCellViewModel> DynamicCells { get; set; }

    }
}
