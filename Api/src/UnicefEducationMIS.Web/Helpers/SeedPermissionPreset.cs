using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnicefEducationMIS.Web.Helpers
{
    public class SeedPermissionPreset
    {
        public int Id { get; set; }
        public string PresetName { get; set; }
        public List<int> Permissions { get; set; }
    }
}
