using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class Notification : BaseModel<long>
    {
        public int NotificationTypeId { get; set; }
        public int Actor { get; set; }
        public int User { get; set; }
        public string Uri { get; set; }
        public string Data { get; set; }
        public string Details { get; set; }
        public bool IsActed { get; set; }
        public bool IsDeleted { get; set; }

        public NotificationType NotificationType { get; set; }

    }
}
