using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Core.ResponseModel
{
   public class NotificationResponse:PagedResponse<Notification>
    {
        public int NotActedTotal { get; set; }

        public NotificationResponse(IEnumerable<Notification> data, int total,int notActedTotal, int pageNumber, int pageSize):base(data,total,pageNumber,pageSize)
        {
            NotActedTotal = notActedTotal;
        }

    }
}
