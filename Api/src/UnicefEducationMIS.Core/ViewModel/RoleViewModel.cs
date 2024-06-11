using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class RoleViewModel
    {
        public int? Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        [Required]
        [Range(1, byte.MaxValue)]
        public byte LevelId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int PermissionPresetId { get; set; }
        public string LevelName { get; set; }
        public LevelRank LevelRank { get; set; }
        public List<PermissionViewModel> Permissions { get; set; }
        public PermissionPresetViewModel PermissionPreset { get; set; }
    }
}
