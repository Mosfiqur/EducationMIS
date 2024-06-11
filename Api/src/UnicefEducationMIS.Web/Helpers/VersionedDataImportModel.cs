using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnicefEducationMIS.Web.Helpers
{
    public class VersionedDataImportModel
    {
        public long InstanceId { get; set; }
        public IFormFile File { get; set; }
    }
}
