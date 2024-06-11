using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class DbLoggingRepository:IDbLoggingRepository
    {
        private readonly UnicefEduDbContext _context;
        private DbSet<LogEntry> _dbSet;

        public DbLoggingRepository(UnicefEduDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<LogEntry>();
        }   

        public async Task Insert(LogEntry log)
        {
            await _dbSet.AddAsync(log);
            await _context.SaveChangesAsync(); 
        }
    }
}
