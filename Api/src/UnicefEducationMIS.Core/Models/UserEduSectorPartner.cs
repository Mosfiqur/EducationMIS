using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public class UserEduSectorPartner
    {
        public int UserId { get; set; }
        public int PartnerId { get; set; }
        public PartnerType PartnerType { get; set; }
        public User User { get; set; }
        public EducationSectorPartner Partner { get; set; }
    }
}
