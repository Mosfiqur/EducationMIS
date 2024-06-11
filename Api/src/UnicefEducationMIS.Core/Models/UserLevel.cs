using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public class UserLevel : BaseModel<byte>
    {
        public UserLevel()
        {
            Roles = new HashSet<Role>();            
        }

        public string LevelName { get; set; }
        public LevelRank Rank { get; set; }

        public virtual ICollection<Role> Roles { get; set; }        
    }
}
