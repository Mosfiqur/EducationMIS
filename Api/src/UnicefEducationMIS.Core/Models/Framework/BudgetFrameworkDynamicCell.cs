using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models.Framework
{
    public class BudgetFrameworkDynamicCell : BaseModel<long>
    {
        public long BudgetFrameworkId { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public string Value { get; set; }

        public virtual BudgetFramework BudgetFramework { get; set; }
        public virtual EntityDynamicColumn EntityDynamicColumn { get; set; }
    }
}
