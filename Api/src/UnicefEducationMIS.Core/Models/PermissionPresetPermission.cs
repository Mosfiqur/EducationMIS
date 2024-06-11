using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class PermissionPresetPermission
    {
        public int PermissionId { get; set; }
        public int PermissionPresetId { get; set; }

        public Permission Permission { get; set; }
        public PermissionPreset PermissionPreset { get; set; }
    }
}
