using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IBaseRepository<TModel, TId> : IDisposable where TModel : BaseModel<TId>
    {
        IQueryable<TModel> GetAll();
        Task<TModel> GetById(TId id);
        Task Insert(TModel entity);
        Task Add(TModel entity);

        Task SaveChangesAsync();
        Task InsertRange(IEnumerable<TModel> entity);
        Task Update(TModel entity);
        Task UpdateRange(IEnumerable<TModel> entity);
        Task Delete(TId id);
        Task Delete(TModel id);
        Task DeleteRange(IEnumerable<TModel> entities);

        Task<TModel> ThrowIfNotFound(TId id);
        Task<int> Count(Expression<Func<TModel, bool>> filter);
        Task<IReadOnlyList<TModel>> All(Expression<Func<TModel, bool>> filter);
    }
}
