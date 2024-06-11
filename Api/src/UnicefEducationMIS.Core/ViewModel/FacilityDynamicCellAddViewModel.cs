using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class FacilityDynamicCellAddViewModel
    {
        public long FacilityId { get; set; }
        public long InstanceId { get; set; }
        public List<DynamicCellViewModel> DynamicCells { get; set; }
        public FacilityDynamicCellAddViewModel()
        {
            DynamicCells = new List<DynamicCellViewModel>();
        }

        public bool IsNew { get; set; }
        public bool HasCode { get; set; }
    }
}
