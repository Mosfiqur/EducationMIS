
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Scheduling;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Factories
{
    public class SchedulerFactory : ISchedulerFactory
    {
        public AbstractScheduler GetScheduler(ScheduleViewModel model)
        {
            switch (model.ScheduleType)
            {                               
                //case ScheduleType.Daily:
                //    return new DailyScheduler(model.StartDate, model.EndDate, model.Frequency.Interval);             
                case ScheduleType.Weekly:
                    return new WeeklyScheduler(model.StartDate, model.EndDate, model.Frequency.Interval, model.Frequency.DaysOfWeek);
                case ScheduleType.BiWeekly:
                    return new WeeklyScheduler(model.StartDate, model.EndDate, model.Frequency.Interval, model.Frequency.DaysOfWeek);
                case ScheduleType.Monthly:
                    return new MonthlyScheduler(model.StartDate, model.EndDate, (int)model.Frequency.Day, model.Frequency.Interval);                    
                //case ScheduleType.Yearly:
                //    return new YearlyScheduler(model.StartDate, model.EndDate, (int)model.Frequency.Day, (int)model.Frequency.Month);
                default:
                    throw new DomainException("Invalid Schedule Type");
            }
        }
    }
}
