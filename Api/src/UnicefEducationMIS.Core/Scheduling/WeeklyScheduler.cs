using System;
using System.Collections.Generic;
using UnicefEducationMIS.Core.Scheduling;

namespace UnicefEducationMIS.Core.Scheduling
{
    public class WeeklyScheduler : AbstractScheduler
    {
        private int _interval;
        private List<DayOfWeek> _onDays;
        private List<DateTime> _instances;

        public WeeklyScheduler(DateTime startDate, DateTime endDate, int interval, List<DayOfWeek> onDays) : base(startDate, endDate)
        {
            _interval = interval;
            _onDays = onDays;
            _instances = new List<DateTime>();
        }

        public override void GenerateDates()
        {
            var tempDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartDate.Hour, StartDate.Minute, StartDate.Second, StartDate.Millisecond);

            while (tempDate <= EndDate)
            {
                if (ValidInstance(tempDate))
                    _instances.Add(tempDate);
                tempDate = tempDate.AddDays(_interval);
            }
        }

        public override IEnumerable<DateTime> GetDates() => _instances;
        private bool ValidInstance(DateTime date) => _onDays.Count == 0 || _onDays.Contains(date.DayOfWeek);
    }
}
