using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class MonitoringFrameworkViewModel
    {
        public long Id { get; set; }
        public string Objective { get; set; }
        public List<ObjectiveIndicatorViewModel> ObjectiveIndicators { get; set; }

        
    }
}
