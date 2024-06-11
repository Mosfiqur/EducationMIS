using System;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IScheduleInstanceRepository : IBaseRepository<Instance, long>
    {
        DateTime GetInstanceDataCollectionDate(long instanceId);
        long? GetPreviousInstanceId( DateTime presentInstanceCollectinDate,EntityType entityType);
        Task<bool> IsRunningInstance(long instanceId);
        Task<long> GetMaxInstanceId(EntityType entityType);
    }
}
