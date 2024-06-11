using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UnicefEducationMIS.Web.Helpers
{
    public class DownloadFacilityVersionTemplateViewModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long InstanceId { get; set; }
    }
}
