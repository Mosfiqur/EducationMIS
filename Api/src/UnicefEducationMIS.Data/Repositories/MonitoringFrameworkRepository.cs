using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class MonitoringFrameworkRepository : BaseRepository<MonitoringFramework,long> , IMonitoringFrameworkRepository
    {
        public MonitoringFrameworkRepository(UnicefEduDbContext context):base(context)
        {

        }
    }
}
