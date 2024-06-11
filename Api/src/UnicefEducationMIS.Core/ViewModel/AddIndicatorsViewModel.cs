
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class AddIndicatorsViewModel
    {
        public List<IndicatorViewModel> Indicators { get; set; }
        public EntityType EntityType { get; set; }
    }
}
