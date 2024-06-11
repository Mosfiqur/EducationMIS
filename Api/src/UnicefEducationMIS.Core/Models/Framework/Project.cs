using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models.Framework
{
    public class Project : BaseModel<int> 
    {
        public string Name { get; set; }

        public Project()
        {
            BudgetFrameworks= new HashSet<BudgetFramework>();
        }
        public ICollection<BudgetFramework> BudgetFrameworks { get; set; }
    }
}
