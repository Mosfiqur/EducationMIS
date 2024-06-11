using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class VersionDataViewModel
    {
        public long InstanceId { get; set; }
        public IFormFile file { get; set; }
    }
}
