using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class NotificationType : BaseModel<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
