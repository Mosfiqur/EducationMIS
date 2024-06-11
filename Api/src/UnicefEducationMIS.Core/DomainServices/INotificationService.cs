using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface INotificationService
    {
        Task Save(List<int> userIds, List<Notification> notification);
        Task<NotificationResponse> Get(BaseQueryModel model);
        Task<List<int>> GetAllUser();

        Task ClearNotification();
        Task ReadNotification(long notificationId);
    }
}
