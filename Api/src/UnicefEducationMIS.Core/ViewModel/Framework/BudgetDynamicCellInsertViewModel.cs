using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class BudgetDynamicCellInsertViewModel
    {
        public long BudgetFrameworkId { get; set; }
        public List<DynamicCellViewModel> DynamicCells { get; set; }
    }
}
