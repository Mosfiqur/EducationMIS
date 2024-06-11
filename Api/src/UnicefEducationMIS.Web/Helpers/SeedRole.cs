using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models.Identity;

namespace UnicefEducationMIS.Web.Helpers
{

    public class SeedRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte LevelId { get; set; }
        public int PermissionPresetId { get; set; }
        public List<int> Permissions { get; set; }
    }
}
