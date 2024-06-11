using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories.Framework;

namespace UnicefEducationMIS.Data.Repositories.Framework
{
    public class MonitoringDynamicCellRepository : BaseRepository<MonitoringFrameworkDynamicCell, long>, IMonitoringDynamicCellRepository
    {
        public MonitoringDynamicCellRepository(UnicefEduDbContext context) : base(context)
        {
        }
    }
}
