using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models.Identity;

namespace UnicefEducationMIS.Core.Models
{
    public class UserDynamicCell : BaseModel<long>
    {
        public int UserId { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public string Value { get; set; }
        public User User { get; set; }
        public EntityDynamicColumn EntityDynamicColumn { get; set; }
    }
}
