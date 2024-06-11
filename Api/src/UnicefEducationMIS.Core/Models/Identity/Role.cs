using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models.Identity
{
    public class Role : IdentityRole<int>
    {
        public Role()
        {
            RoleClaims = new HashSet<RoleClaim>();
            UserRoles = new HashSet<UserRole>();

        }
        public byte LevelId { get; set; }
        public int PermissionPresetId { get; set; }
        public PermissionPreset PermissionPreset { get; set; }
        public UserLevel Level { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
