using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models.Framework;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IBudgetFrameworkRepository : IBaseRepository<BudgetFramework, long>, ICountable<BudgetFramework>
    {
    }
}
