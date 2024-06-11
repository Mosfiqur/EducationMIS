using System.Collections.Generic;

namespace UnicefEducationMIS.Core.ViewModel.User
{
    public class UserDynamicCellInsertViewModel
    {
        public int UserId { get; set; }
        public List<DynamicCellViewModel> DynamicCells { get; set; }
    }
}