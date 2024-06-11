using System;
using System.Collections.Generic;
using UnicefEducationMIS.Core.Scheduling;

namespace UnicefEducationMIS.Core.Scheduling
{
    public class MonthlyScheduler : AbstractScheduler
    {
        private int _onDay;
        private int _interval;
        List<DateTime> _instances = new List<DateTime>();
        public MonthlyScheduler(DateTime startDate, DateTime endDate, int onDay, int interval) : base(startDate, endDate)
        {
            _onDay = onDay;
            _interval = interval;
        }
        public override void GenerateDates()
        {
            DateTime tempDate;
            if (StartDate.Day > _onDay)
            {
                tempDate = new DateTime(StartDate.Year, StartDate.Month + 1, _onDay, StartDate.Hour, StartDate.Minute, StartDate.Second, StartDate.Millisecond);
            }
            else
            {
                tempDate = new DateTime(StartDate.Year, StartDate.Month, _onDay, StartDate.Hour, StartDate.Minute, StartDate.Second, StartDate.Millisecond);
            }

            while (tempDate <= EndDate)
            {
                _instances.Add(tempDate);
                tempDate = tempDate.AddMonths(_interval);
                tempDate = new DateTime(tempDate.Year, tempDate.Month , 1);
            }
        }

        public override IEnumerable<DateTime> GetDates() => _instances;
    }
}
