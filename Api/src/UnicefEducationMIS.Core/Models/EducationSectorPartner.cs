using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public class EducationSectorPartner : BaseModel<int>
    {
        public EducationSectorPartner()
        {
            Partners = new HashSet<UserEduSectorPartner>();
        }
        public string PartnerName { get; set; }
        public PartnerType PartnerType { get; set; }
        public ICollection<UserEduSectorPartner> Partners { get; set; }

        public ICollection<ObjectiveIndicator> ObjectiveIndicators { get; set; }
    }
}
