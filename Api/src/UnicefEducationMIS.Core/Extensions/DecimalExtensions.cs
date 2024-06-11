using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Extensions
{
    public static class DecimalExtensions
    {
        public static bool Between(this decimal val, decimal lowerLimit, decimal upperLimit)
        {
            return val >= lowerLimit && val <= upperLimit;
        }
    }
}
