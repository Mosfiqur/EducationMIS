using System;
using System.Collections.Generic;
using UnicefEducationMIS.Core.Scheduling;

namespace UnicefEducationMIS.Core.Scheduling
{
    public class DailyScheduler : AbstractScheduler
    {
        private int _interval;
        List<DateTime> _instances = new List<DateTime>();

        public DailyScheduler(DateTime startDate, DateTime endDate, int interval) : base(startDate, endDate)
        {
            _interval = interval;
        }

        public override void GenerateDates()
        {
            var tempDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartDate.Hour, StartDate.Minute, StartDate.Second, StartDate.Millisecond);

            while (tempDate <= EndDate)
            {
                _instances.Add(tempDate);
                tempDate = tempDate.AddDays(_interval);
            }
        }

        public override IEnumerable<DateTime> GetDates() => _instances;
    }
}
