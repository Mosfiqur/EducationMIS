using System;
using System.Collections.Generic;

namespace UnicefEducationMIS.Core.Models.Framework
{
    public class BudgetFramework : BaseModel<long>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public int ProjectId { get; set; }
        public int DonorId { get; set; }
        public Project Project { get; set; }
        public Donor Donor { get; set; }

        public BudgetFramework()
        {
            DynamicCells = new HashSet<BudgetFrameworkDynamicCell>();            
        }

        
        public ICollection<BudgetFrameworkDynamicCell> DynamicCells { get; set; }
    }
}
