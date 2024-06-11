using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class TargetDynamicCellInsertViewModel
    {
        public long TargetFrameworkId { get; set; }
        public List<DynamicCellViewModel> DynamicCells { get; set; }
    }
}
