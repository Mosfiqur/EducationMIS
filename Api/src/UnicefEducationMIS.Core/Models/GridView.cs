using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public class GridView : BaseModel<long>
    {
        public GridView()
        {
            GridViewDetails = new HashSet<GridViewDetails>();
        }
        public string Name { get; set; }
        public EntityType EntityTypeId { get; set; }
        public virtual ICollection<GridViewDetails> GridViewDetails { get; set; }

    }
}
