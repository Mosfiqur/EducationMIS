using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Models.Views;

namespace UnicefEducationMIS.Core.Models
{
    public class Camp : BaseModel<int>
    {
        public Camp()
        {
            Blocks = new HashSet<Block>();
        //    BeneficiarieCamps = new HashSet<Beneficiary>();
        }
        public string SSID { get; set; }
        public string Name { get; set; }
        public string NameAlias { get; set; }
        public int UnionId { get; set; }
        public Union Union { get; set; }
        public ICollection<Block> Blocks { get; set; }
        public ICollection<TargetFramework> TargetFrameworks { get; set; }
        public ICollection<CampCoordinate> CampCoordinates { get; set; }
        [NotMapped]
        public ICollection<FacilityView> Facilities { get; set; }
    }
}
