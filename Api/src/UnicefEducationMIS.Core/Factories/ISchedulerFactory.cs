using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Scheduling;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Factories
{
    public interface ISchedulerFactory
    {
        AbstractScheduler GetScheduler(ScheduleViewModel model);
    }
}
