using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class PermissionPreset : BaseModel<int>
    {
        public PermissionPreset()
        {
            PermissionPresetPermissions = new HashSet<PermissionPresetPermission>();
        }

        public string PresetName { get; set; }        
        public virtual ICollection<PermissionPresetPermission> PermissionPresetPermissions { get; set; }
    }
}
