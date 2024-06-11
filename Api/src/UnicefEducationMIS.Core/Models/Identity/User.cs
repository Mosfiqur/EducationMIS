using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace UnicefEducationMIS.Core.Models.Identity
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
            UserEduSectorPartners = new HashSet<UserEduSectorPartner>();
            DynamicCells = new HashSet<UserDynamicCell>();
        }
                
        public string FullName { get; set; }
        public string DesignationName { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }          
        public ICollection<UserEduSectorPartner> UserEduSectorPartners { get; set; }
        public ICollection<UserDynamicCell> DynamicCells { get; set; }
    }
}
