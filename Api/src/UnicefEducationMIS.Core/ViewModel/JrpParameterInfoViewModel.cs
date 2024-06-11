using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
   public class JrpParameterInfoViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public long TargetId { get; set; }
        public string TargetName { get; set; }
        public long IndicatorId { get; set; }
        public string IndicatorName { get; set; }
    }
}
