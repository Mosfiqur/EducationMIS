using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class ScheduleRepository : BaseRepository<Schedule, long>, IScheduleRepository
    {
        public ScheduleRepository(UnicefEduDbContext context) : base(context)
        {
        }
    }
}
