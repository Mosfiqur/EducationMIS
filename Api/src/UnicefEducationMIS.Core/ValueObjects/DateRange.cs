using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ValueObjects
{
    public class DateRange
    {
        public DateRange(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public DateRange(int ageGroupId)
        {

            var from = (AgeGroup)ageGroupId;

            if(from == AgeGroup.Three)
            {
                From = DateTime.Now.AddYears(-3);
                To = DateTime.Now;
            }

            if (from == AgeGroup.Four_Five)
            {
                From = DateTime.Now.AddYears(-5);
                To = DateTime.Now.AddYears(-3);
            }

            if (from == AgeGroup.Six_Fourteen)
            {
                From = DateTime.Now.AddYears(-14);
                To = DateTime.Now.AddYears(-5);
            }

            if (from == AgeGroup.Fifteen_Eighteen)
            {
                From = DateTime.Now.AddYears(-18);
                To = DateTime.Now.AddYears(-14);
            }

            if (from == AgeGroup.Nineteen_TwentyFour)
            {
                From = DateTime.Now.AddYears(-24);
                To = DateTime.Now.AddYears(-18);
            }
        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
