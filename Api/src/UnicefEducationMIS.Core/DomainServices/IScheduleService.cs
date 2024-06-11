
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IScheduleService
    {        
        Task CreateSchedule(ScheduleViewModel model);
        Task UpdateSchedule(ScheduleUpdateViewModel model);
        Task UpdateScheduleInstance(InstanceUpdateViewModel model);
        Task<ScheduleViewModel> GetCurrentSchedule(EntityType scheduleFor);
        Task<PagedResponse<ScheduleViewModel>> GetSchedules(ScheduleQueryModel model);
        Task<PagedResponse<InstanceViewModel>> GetCompletedInstances(BaseQueryModel model);
        Task<PagedResponse<InstanceViewModel>> GetRunningInstances(ScheduleInstanceQueryModel model);
        Task<PagedResponse<InstanceViewModel>> GetCurrentScheduleInstances(ScheduleInstanceQueryModel model);
        Task<PagedResponse<InstanceViewModel>> GetNotPendingInstances(ScheduleInstanceQueryModel model);
        Task DeleteSchedule(long scheduleId);
        Task StartCollection(StartCollectionViewModel model);
        Task<PagedResponse<InstanceViewModel>> GetInstancesStatusWise(ScheduleInstanceStatusWiseQueryModel model);
        Task CompleteInstance(long instanceId);
        Task<long> GetMaxFacilityInstanceId();
        Task<long> GetMaxBeneficiaryInstanceId();
        Task<long> GetMappingFacilityInstanceId(long instanceId);
        Task<long> GetMappingBeneficiaryInstanceId(long instanceId);
    }
}
