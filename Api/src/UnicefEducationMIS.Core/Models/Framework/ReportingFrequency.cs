using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models.Framework
{
    public class ReportingFrequency : BaseModel<int>
    {
        public string Name { get; set; }

        public ICollection<ObjectiveIndicator> ObjectiveIndicator { get; set; }
    }
}
