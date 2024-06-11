using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class GridViewDetails: BaseModel<long>
    {
        public long EntityDynamicColumnId { get; set; }
        public int ColumnOrder { get; set; }

        public long GridViewId { get; set; }

        public virtual GridView BeneficiaryView { get; set; }
        public virtual EntityDynamicColumn EntityDynamicColumn { get; set; }
    }
}
