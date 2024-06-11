using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class ListDataTypeViewModel
    {
        public ListDataTypeViewModel()
        {
            ListItems = new List<ListItemViewModel>();
        }
        public long? Id { get; set; }
        public string Name { get; set; }

        public ICollection<ListItemViewModel> ListItems { get; set; }

    }
}
