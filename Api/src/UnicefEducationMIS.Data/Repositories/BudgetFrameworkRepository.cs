using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.Specifications;

namespace UnicefEducationMIS.Data.Repositories
{
    public class BudgetFrameworkRepository : BaseRepository<BudgetFramework, long> , IBudgetFrameworkRepository
    {
        public BudgetFrameworkRepository(UnicefEduDbContext context):base(context)
        {

        }

        public async Task<int> Count(Specification<BudgetFramework> filter)
        {
            return await _context.Budgets.Where(filter.ToExpression()).CountAsync();
        }
    }
}
