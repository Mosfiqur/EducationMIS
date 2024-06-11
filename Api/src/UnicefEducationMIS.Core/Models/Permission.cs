using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using UnicefEducationMIS.Core.Models.Identity;

namespace UnicefEducationMIS.Core.Models
{
    public class Permission : BaseModel<int>
    {
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
            PermissionPresetPermissions = new HashSet<PermissionPresetPermission>();
            
        }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<PermissionPresetPermission> PermissionPresetPermissions { get; set; }
    }
}
