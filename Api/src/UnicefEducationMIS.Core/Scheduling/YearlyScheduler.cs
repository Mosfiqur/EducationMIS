using System;
using System.Collections.Generic;
using UnicefEducationMIS.Core.Scheduling;

namespace UnicefEducationMIS.Core.Scheduling
{
    public class YearlyScheduler : AbstractScheduler
    {
        List<DateTime> _instances = new List<DateTime>();
        private int _year = 1;
        private int _onDay;
        private int _inMonth;

        public YearlyScheduler(DateTime startDate, DateTime endDate, int onDay, int inMonth) : base(startDate, endDate)
        {            
            _inMonth = inMonth;
            _onDay = onDay;
        }

        public override  void GenerateDates()
        {
           
            DateTime tempDate;
            var givenDate = new DateTime(StartDate.Year, _inMonth, _onDay);

            if (givenDate < StartDate)
            {
                tempDate = new DateTime(StartDate.Year + 1, _inMonth, _onDay);
            }
            else
            {
                tempDate = new DateTime(StartDate.Year, _inMonth, _onDay);
            }

            while (tempDate <= EndDate)
            {
                _instances.Add(tempDate);
                tempDate = tempDate.AddYears(_year);
            }
            
        }

        public override IEnumerable<DateTime> GetDates() => _instances;
    }
}
