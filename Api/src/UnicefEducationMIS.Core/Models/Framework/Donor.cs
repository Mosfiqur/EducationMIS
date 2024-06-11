using System.Collections.Generic;

namespace UnicefEducationMIS.Core.Models.Framework
{
    public class Donor : BaseModel<int>
    {
        public string Name { get; set; }


        public Donor()
        {
            BudgetFrameworks = new HashSet<BudgetFramework>();
        }
        public ICollection<BudgetFramework> BudgetFrameworks { get; set; }
    }
}