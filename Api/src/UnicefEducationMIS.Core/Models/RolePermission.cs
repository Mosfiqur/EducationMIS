using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models.Identity;

namespace UnicefEducationMIS.Core.Models
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
