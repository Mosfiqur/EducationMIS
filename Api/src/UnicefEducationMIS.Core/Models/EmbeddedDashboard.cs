using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class EmbeddedDashboard : BaseModel<int>
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public bool IsEmbedded { get; set; }

    }
}
