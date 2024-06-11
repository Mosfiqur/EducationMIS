using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class EduSectorPartnerViewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        public string PartnerName { get; set; }
        [Required]
        public PartnerType PartnerType { get; set; }
    }
}
