using System.Collections.Generic;

namespace UnicefEducationMIS.Core.Models
{
    public class ListDataType : BaseModel<long>
    {
        public ListDataType()
        {
            ListItems = new HashSet<ListItem>();
            EntityDynamicColumns = new HashSet<EntityDynamicColumn>();
        }
        public string Name { get; set; }
        public ICollection<ListItem> ListItems { get; set; }
        public ICollection<EntityDynamicColumn> EntityDynamicColumns { get; set; }
    }
}
