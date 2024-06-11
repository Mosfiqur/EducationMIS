using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories.Framework;

namespace UnicefEducationMIS.Data.Repositories.Framework
{
    public class BudgetDynamicCellRepository : BaseRepository<BudgetFrameworkDynamicCell, long>, IBudgetDynamicCellRepository
    {
        public BudgetDynamicCellRepository(UnicefEduDbContext context) : base(context)
        {
        }
    }
}
