using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface INotificationRepository : IBaseRepository<Notification, long>
    {
        Task<List<int>> GetAllUser();
    }
}
