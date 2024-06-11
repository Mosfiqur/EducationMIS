using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEntityTypes = UnicefEducationMIS.Core.ValueObjects.EntityType;

namespace UnicefEducationMIS.Data.Repositories
{
    public class ScheduleInstanceRepository : BaseRepository<Instance, long>, IScheduleInstanceRepository
    {
        public ScheduleInstanceRepository(UnicefEduDbContext context) : base(context)
        {
        }

        public DateTime GetInstanceDataCollectionDate(long instanceId)
        {
            return GetAll().Where(a => a.Id == instanceId).Select(a => a.DataCollectionDate).FirstOrDefault();
        }

        public long? GetPreviousInstanceId(DateTime presentInstanceCollectinDate, UnicefEntityTypes entityType)
        {
            return GetAll().Where(a => a.DataCollectionDate < presentInstanceCollectinDate && a.Schedule.ScheduleFor == entityType)
                                .OrderByDescending(a => a.DataCollectionDate)
                                .Select(a => a.Id).FirstOrDefault();
        }

        public async Task<bool> IsRunningInstance(long instanceId)
        {
            var isRunningInstance = await GetAll()
                .AnyAsync(a => a.Id == instanceId && a.Status == InstanceStatus.Running);
            return isRunningInstance;
        }

        public async Task<long> GetMaxInstanceId(EntityType entityType)
        {
            return await _context.ScheduleInstances
                                  .Where(a => a.Schedule.ScheduleFor == entityType &&
                                  (a.Status == InstanceStatus.Completed || a.Status == InstanceStatus.Running))
                                  .OrderByDescending(a => a.DataCollectionDate)
                                      .OrderByDescending(a => a.Id)
                                  .Select(a => a.Id)
                                  .FirstOrDefaultAsync();
        }
    }
}
