using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class NotificationRepository : BaseRepository<Notification, long>, INotificationRepository
    {
        public NotificationRepository(UnicefEduDbContext context) : base(context)
        {
        }
        public async Task<List<int>> GetAllUser()
        {
            var userIds =await _context.Users.Where(a => !a.IsDeleted).Select(a => a.Id).ToListAsync();
            return userIds;
        }
        
    }
}
