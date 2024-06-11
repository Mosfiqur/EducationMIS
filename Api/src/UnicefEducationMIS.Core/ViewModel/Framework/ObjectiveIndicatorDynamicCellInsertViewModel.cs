using System.Collections.Generic;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class ObjectiveIndicatorDynamicCellInsertViewModel
    {
        public long ObjectiveIndicatorId { get; set; }
        public List<DynamicCellViewModel> DynamicCells { get; set; }
    }
}