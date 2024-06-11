using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ViewModel
{
public    class JrpChartViewModel
    {
        public JrpChartViewModel()
        {
            Categories = new List<string>();
            Target = new List<decimal>();
            Achievement = new List<decimal>();
        }
        public List<string> Categories { get; set; }
        public List<decimal> Target { get; set; }
        public List<decimal> Achievement { get; set; }
    }
}
