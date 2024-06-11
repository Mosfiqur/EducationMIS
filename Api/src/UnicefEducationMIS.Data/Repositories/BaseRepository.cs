using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{

    public class BaseRepository<TModel, TId> : IBaseRepository<TModel, TId> where TModel : BaseModel<TId>
    {
        protected readonly UnicefEduDbContext _context;
        protected DbSet<TModel> _dbSet;
        protected BaseRepository(UnicefEduDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TModel>();
        }

        public IQueryable<TModel> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public Task<TModel> GetById(TId id)
        {
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(t => t.Id.Equals(id));
        }

        public async Task Insert(TModel entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Add(TModel entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task InsertRange(IEnumerable<TModel> entity)
        {
            await _dbSet.AddRangeAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TModel entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRange(IEnumerable<TModel> entity)
        {
            _dbSet.UpdateRange(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(TId id)
        {
            var entity = await GetById(id);
            await Delete(entity);
        }

        public async Task Delete(TModel entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRange(IEnumerable<TModel> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<TModel> ThrowIfNotFound(TId id)
        {
            var existing = await GetById(id);
            if (existing == null)
                throw new RecordNotFound(Messages.RecordNotFound);
            return existing;
        }

        public async Task<int> Count(Expression<Func<TModel, bool>> filter)
        {
            return await _dbSet.Where(filter).CountAsync();
        }

        public async Task<IReadOnlyList<TModel>> All(Expression<Func<TModel, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        public void Dispose()
        {
            _dbSet = null;
            _context.Dispose();
        }
    }
}
