using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentLoginUserService _currentLoginUserService;

        public NotificationService(INotificationRepository notificationRepository, ICurrentLoginUserService currentLoginUserService)
        {
            _notificationRepository = notificationRepository;
            _currentLoginUserService = currentLoginUserService;
        }
        public async Task<NotificationResponse> Get(BaseQueryModel model)
        {
            IQueryable<Notification> notificationQuery = _notificationRepository.GetAll()
                                               .Where(x => x.User == _currentLoginUserService.UserId && !x.IsDeleted);
            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                notificationQuery = notificationQuery.Where(x => x.Data.Contains(model.SearchText));
            var total = await notificationQuery.CountAsync();
            var notActedTotal = await notificationQuery.Where(a => !a.IsActed).CountAsync();

            var data = await notificationQuery
                .Skip(model.Skip())
                .Take(model.PageSize)
                .ToListAsync();

            return new NotificationResponse(data, total, notActedTotal, model.PageNo, model.PageSize);
        }
        public async Task<List<int>> GetAllUser()
        {
            return await _notificationRepository.GetAllUser();
        }
        public string GetUri(NotificationTypeEnum notificationType)
        {
            string uri = "";
            if (notificationType == NotificationTypeEnum.Beneficiary_Instance_Start)
            {
                uri = "unicef/beneficiary/indicators";
            }
            else if (notificationType == NotificationTypeEnum.Facility_Instance_Start)
            {
                uri = "unicef/facility/indicators";
            }
            else if (notificationType == NotificationTypeEnum.Recollect_Facility)
            {
                uri = "";
            }
            else if (notificationType == NotificationTypeEnum.Recollect_Facility)
            {
                uri = "";
            }
            else if (notificationType == NotificationTypeEnum.Assign_Teacher)
            {
                uri = "unicef/facility/all";
            }

            return uri;
        }

        public async Task ReadNotification(long notificationId)
        {
            var notification = await _notificationRepository.GetAll().FirstOrDefaultAsync(a => a.Id == notificationId);
            notification.IsActed = true;
            await _notificationRepository.Update(notification);
        }
        public async Task ClearNotification()
        {
            var notifications = await _notificationRepository.GetAll().Where(a => a.User == _currentLoginUserService.UserId).ToListAsync();
            foreach (var item in notifications)
            {
                item.IsDeleted = true;
            }

            await _notificationRepository.UpdateRange(notifications);
        }
        public async Task Save(List<int> userIds, List<Notification> notifications)
        {
            List<Notification> lstNotifications = new List<Notification>();

            foreach (var id in userIds)
            {
                foreach (var notification in notifications)
                {
                    notification.Actor = _currentLoginUserService.UserId;
                    notification.Uri = GetUri((NotificationTypeEnum)notification.NotificationTypeId);
                    notification.User = id;
                    lstNotifications.Add(new Notification()
                    {
                        Actor = _currentLoginUserService.UserId,
                        Uri= GetUri((NotificationTypeEnum)notification.NotificationTypeId),
                        User=id,
                        Data=notification.Data,
                        Details= notification.Details,
                        NotificationTypeId= notification.NotificationTypeId
                    });
                }
            }
            await _notificationRepository.InsertRange(lstNotifications);
        }
    }
}
