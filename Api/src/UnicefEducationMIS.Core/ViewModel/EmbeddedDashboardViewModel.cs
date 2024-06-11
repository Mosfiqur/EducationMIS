using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class EmbeddedDashboardViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public bool IsEmbedded { get; set; }
    }
}
