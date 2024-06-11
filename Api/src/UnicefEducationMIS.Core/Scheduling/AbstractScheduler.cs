using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Scheduling
{
    public abstract class AbstractScheduler
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public AbstractScheduler(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        public abstract void GenerateDates();
        public abstract IEnumerable<DateTime> GetDates();
    }
}
