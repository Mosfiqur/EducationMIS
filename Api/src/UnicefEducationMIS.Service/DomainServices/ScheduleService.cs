using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Factories;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.Scheduling;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class ScheduleService : IScheduleService
    {

        private readonly IScheduleRepository _scheduleRepository;
        private readonly IScheduleInstanceRepository _scheduleInstanceRepository;
        private readonly IMapper _mapper;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IIndicatorRepository _indicatorRepository;

        private readonly IBeneficiaryDataCollectionStatusRepository _beneficiaryDataCollectionStatusRepository;
        private readonly IBeneficiaryDataCollectionRepository _beneficiaryDataCollectionRepository;
        private readonly IFacilityDataCollectionRepository _facilityDataCollectionRepository;
        private readonly IFacilityDataCollectionStatusRepository _facilityDataCollectionStatusRepository;
        private readonly INotificationService _notificationService;
        private readonly IInstanceMappingRepository _instanceMappingRepository;

        public ScheduleService(IScheduleRepository scheduleRepository,
            IScheduleInstanceRepository scheduleInstanceRepository, IMapper mapper,
            ISchedulerFactory factory,
            IBeneficiaryRepository beneficiaryRepository,
            IFacilityRepository facilityRepository,
            IIndicatorRepository indicatorRepository,
            IBeneficiaryDataCollectionStatusRepository beneficiaryDataCollectionStatusRepository,
            IBeneficiaryDataCollectionRepository beneficiaryDataCollectionRepository,
            IFacilityDataCollectionRepository facilityDataCollectionRepository,
            IFacilityDataCollectionStatusRepository facilityDataCollectionStatusRepository,
            INotificationService notificationService,
            IInstanceMappingRepository instanceMappingRepository
            )
        {
            _scheduleRepository = scheduleRepository;
            _scheduleInstanceRepository = scheduleInstanceRepository;
            _mapper = mapper;
            _schedulerFactory = factory;
            _beneficiaryRepository = beneficiaryRepository;
            _facilityRepository = facilityRepository;
            _indicatorRepository = indicatorRepository;
            _beneficiaryDataCollectionStatusRepository = beneficiaryDataCollectionStatusRepository;
            _beneficiaryDataCollectionRepository = beneficiaryDataCollectionRepository;
            _facilityDataCollectionRepository = facilityDataCollectionRepository;
            _facilityDataCollectionStatusRepository = facilityDataCollectionStatusRepository;
            _notificationService = notificationService;
            _instanceMappingRepository = instanceMappingRepository;
        }

        public async Task CreateSchedule(ScheduleViewModel model)
        {
            //var existing = _scheduleRepository.GetAll()
            //    .SingleOrDefault(x => x.Status != ScheduleStatus.Completed && !x.IsDeleted && x.ScheduleFor == model.ScheduleFor);
            var existing = _scheduleRepository.GetAll()
                .SingleOrDefault(x => x.Status != ScheduleStatus.Completed && x.ScheduleFor == model.ScheduleFor);

            if (existing != null)
            {
                throw new DomainException($"Schedule {existing.ScheduleName} is not compeleted yet.");
            }

            var lastScheduleInstance = await _scheduleInstanceRepository.GetAll().
                Include(a => a.Schedule).
                Where(a => a.Status == InstanceStatus.Completed && a.Schedule.ScheduleFor == model.ScheduleFor)
                .OrderByDescending(a => a.DataCollectionDate)
                .FirstOrDefaultAsync();
            if (lastScheduleInstance != null)
            {
                var lastInstanceData = _mapper.Map<InstanceViewModel>(lastScheduleInstance);

                if (lastInstanceData.EndDate > model.StartDate)
                {
                    throw new DomainException("Schedule start date should be greater than last instance end date.");
                }
            }

            AbstractScheduler scheduler = _schedulerFactory.GetScheduler(model);
            scheduler.GenerateDates();
            model.Instances = scheduler.GetDates().Select(date => new InstanceViewModel
            {
                Title = $"{model.ScheduleName}-{date.ToString("MMM/yyyy")}",
                DataCollectionDate = date
            }).ToList();

            var schedule = _mapper.Map<Schedule>(model);
            await _scheduleRepository.Insert(schedule);
        }

        public async Task UpdateSchedule(ScheduleUpdateViewModel model)
        {
            var existing = await _scheduleRepository.ThrowIfNotFound(model.Id);
            existing.ScheduleName = model.ScheduleName;
            existing.Description = model.Description;
            await _scheduleRepository.Update(existing);
        }


        public async Task<ScheduleViewModel> GetCurrentSchedule(EntityType scheduleFor)
        {

            var existing = _scheduleRepository.GetAll()
                .Include(x => x.Frequency)
                .Include(x => x.Instances)
                .SingleOrDefault(x => x.ScheduleFor == scheduleFor && x.Status != ScheduleStatus.Completed);

            if (existing == null)
            {
                throw new RecordNotFound("Please create a schedule first.");
            }


            var schedule = _mapper.Map<ScheduleViewModel>(existing);

            return await Task.FromResult(schedule);
        }
        private async Task DeleteInstanceIndicator(List<long> instanceIds)
        {
            var deletableInstanceIndicator =
                   await _indicatorRepository.GetAll().Where(a => instanceIds.Contains(a.InstanceId)).ToListAsync();
            await _indicatorRepository.DeleteRange(deletableInstanceIndicator);
        }
        private async Task DeleteDataCollectionStatus(List<long> instanceIds, EntityType entityType)
        {
            if (entityType == EntityType.Beneficiary)
            {
                var deletableDataCollectionStatus =
                await _beneficiaryDataCollectionStatusRepository.GetAll().Where(a => instanceIds.Contains(a.InstanceId)).ToListAsync();
                await _beneficiaryDataCollectionStatusRepository.DeleteRange(deletableDataCollectionStatus);
            }
            else if (entityType == EntityType.Facility)
            {
                var deletableDataCollectionStatus =
                 await _facilityDataCollectionStatusRepository.GetAll().Where(a => instanceIds.Contains(a.InstanceId)).ToListAsync();
                await _facilityDataCollectionStatusRepository.DeleteRange(deletableDataCollectionStatus);
            }
        }
        private async Task DeleteDynamicCellData(List<long> instanceIds, EntityType entityType)
        {
            if (entityType == EntityType.Beneficiary)
            {
                var deletableDynamicCellData =
                await _beneficiaryDataCollectionRepository.GetAll().Where(a => instanceIds.Contains(a.InstanceId)).ToListAsync();
                await _beneficiaryDataCollectionRepository.DeleteRange(deletableDynamicCellData);
            }
            else if (entityType == EntityType.Facility)
            {
                var deletableDynamicCellData =
                await _facilityDataCollectionRepository.GetAll().Where(a => instanceIds.Contains(a.InstanceId)).ToListAsync();
                await _facilityDataCollectionRepository.DeleteRange(deletableDynamicCellData);
            }
        }
        public async Task DeleteSchedule(long scheduleId)
        {
            var schedule = await _scheduleRepository.GetAll()
                .SingleOrDefaultAsync(x => x.Id == scheduleId && !x.IsDeleted);

            if (schedule == null)
            {
                throw new RecordNotFound("Schedule does not exist.");
            }

            if (schedule.Status == ScheduleStatus.Pending)
            {
                await _scheduleRepository.Delete(schedule);
            }

            if (schedule.Status == ScheduleStatus.Running)
            {
                var deleteable = await _scheduleInstanceRepository.GetAll()
                    .Where(x => x.ScheduleId == schedule.Id && x.Status != InstanceStatus.Completed)
                    .ToListAsync();

                var deletableInstaceIds = deleteable.Select(a => a.Id).ToList();
                // delete instance Indicator
                await DeleteInstanceIndicator(deletableInstaceIds);
                // delete datacollection 
                await DeleteDataCollectionStatus(deletableInstaceIds, schedule.ScheduleFor);
                // delete dynamic cell
                await DeleteDynamicCellData(deletableInstaceIds, schedule.ScheduleFor);

                await _scheduleInstanceRepository.DeleteRange(deleteable);
                schedule.IsDeleted = true;
                schedule.Status = ScheduleStatus.Completed;
                await _scheduleRepository.Update(schedule);
            }
        }
        //private async Task DeleteRunningInstanceData(List<long> runningInstanceId)
        //{
        //    var 
        //}
        public async Task StartCollection(StartCollectionViewModel model)
        {
            // Check if any other instances running besides this one


            var instance = await _scheduleInstanceRepository.GetAll()
                .Include(x => x.Schedule)
                .SingleOrDefaultAsync(x => x.Id == model.InstanceId);

            if (instance == null || instance.Schedule.IsDeleted)
            {
                throw new RecordNotFound("Instance not found.");
            }

            var anyOtherRunningInstance = _scheduleInstanceRepository
                .GetAll()
                .Any(x => x.Status == InstanceStatus.Running &&
                          x.Id != model.InstanceId &&
                          x.Schedule.ScheduleFor == instance.Schedule.ScheduleFor);

            if (anyOtherRunningInstance)
            {
                throw new InstanceAlreadyRunningException();
            }
            var isPendingInstanceBeforeCurrentInstanceFound = await _scheduleInstanceRepository.GetAll()
                .AnyAsync(a => a.DataCollectionDate < instance.DataCollectionDate &&
                               a.Status == InstanceStatus.Pending &&
                               a.Schedule.ScheduleFor == instance.Schedule.ScheduleFor
                         );
            if (isPendingInstanceBeforeCurrentInstanceFound)
            {
                throw new DomainException("There is pending instance before current instance.");
            }

            instance.Status = InstanceStatus.Running;
            if (instance.Schedule.Status != ScheduleStatus.Running)
            {
                instance.Schedule.Status = ScheduleStatus.Running;
                await _scheduleRepository.Update(instance.Schedule);
            }


            await _scheduleInstanceRepository.Update(instance);
            await AddDataToDataCollectionStatus(instance.Schedule.ScheduleFor, model.InstanceId);
            await _indicatorRepository.AddInstanceIndicator(instance);

            var users = await _notificationService.GetAllUser();
            var notificationId = (int)(instance.Schedule.ScheduleFor == EntityType.Beneficiary ?
                NotificationTypeEnum.Beneficiary_Instance_Start :
                NotificationTypeEnum.Facility_Instance_Start);
            var notificationFor = instance.Schedule.ScheduleFor == EntityType.Beneficiary ? "Beneficiary" : "Facility";

            List<Notification> notifications = new List<Notification>() { new Notification() {
                Data=@"{""InstanceId"":"+instance.Id+"}",
                Details=$"{notificationFor} data collection process of instance [{instance.Title}] has been started on [{DateTime.Now.ToString("dd-MM-yyyy")}]. ",
                IsActed=false,
                IsDeleted=false,
                NotificationTypeId=notificationId,

            } };

            await _notificationService.Save(users, notifications);

        }

        private async Task CopyBeneficiaryData(long currentInstanceId)
        {
            var currentInstanceCollectionDate = _scheduleInstanceRepository.GetInstanceDataCollectionDate(currentInstanceId);
            var previousInstanceId = _scheduleInstanceRepository.GetPreviousInstanceId(currentInstanceCollectionDate, EntityType.Beneficiary);
            if (previousInstanceId.HasValue && previousInstanceId.Value > 0)
            {
                var facilityInstanceId = await GetMappingFacilityInstanceId((long)previousInstanceId);
                var deletedFacilityIds = await _facilityDataCollectionStatusRepository.GetAll()
                    .Where(a => a.InstanceId == facilityInstanceId && a.Status == CollectionStatus.Deleted)
                    .Select(a => a.FacilityId).ToListAsync();

                var beneficairyData = await _beneficiaryDataCollectionRepository.GetAll()
                    .Where(a => a.InstanceId == previousInstanceId)
                    .ToListAsync();

                var beneficiaryRaw = beneficairyData.GroupBy(a => a.BeneficiaryId).Select(f => new BeneficiaryRawViewModel
                {
                    EntityId = f.Key,
                    FacilityId = f.Where(g => g.EntityDynamicColumnId == BeneficairyFixedColumns.FacilityId).Select(h => Convert.ToInt32(h.Value)).FirstOrDefault(),

                }).ToList();

                var beneficiaryIdsFromDeletedFacility = beneficiaryRaw
                    .Where(a => deletedFacilityIds.Contains(a.FacilityId))
                    .Select(a => a.EntityId).ToList();


                var collectionStatuses = await _beneficiaryDataCollectionStatusRepository.GetAll()
                    .Where(x => x.InstanceId == previousInstanceId && x.Status != CollectionStatus.Deleted)
                    .Select(x => new BeneficiaryDataCollectionStatus()
                    {
                        InstanceId = currentInstanceId,
                        BeneficiaryId = x.BeneficiaryId,
                        Status = (x.Status == CollectionStatus.Inactivated) ? (CollectionStatus)x.Status : CollectionStatus.NotCollected
                    })
                    .ToListAsync();

                collectionStatuses = collectionStatuses.Where(a => !beneficiaryIdsFromDeletedFacility.Contains(a.BeneficiaryId)).ToList();

                var beneficiaryIds = collectionStatuses.Select(a => a.BeneficiaryId).ToList();

                var beneficiaryDynamicCellData = beneficairyData.Where(a => beneficiaryIds.Contains(a.BeneficiaryId))
                                                .Select(a => new BeneficiaryDynamicCell
                                                {
                                                    EntityDynamicColumnId = a.EntityDynamicColumnId,
                                                    InstanceId = currentInstanceId,
                                                    BeneficiaryId = a.BeneficiaryId,
                                                    Value = a.Value
                                                }).ToList();


                //var beneficiaryDynamicCellData =
                //    (from st in _beneficiaryDataCollectionStatusRepository.GetAll()
                //     join cell in _beneficiaryDataCollectionRepository.GetAll() on
                //         new { st.BeneficiaryId, st.InstanceId } equals
                //         new { cell.BeneficiaryId, cell.InstanceId }
                //     where st.Status != CollectionStatus.Deleted && st.InstanceId == previousInstanceId
                //     select new BeneficiaryDynamicCell
                //     {
                //         EntityDynamicColumnId = cell.EntityDynamicColumnId,
                //         InstanceId = currentInstanceId,
                //         BeneficiaryId = cell.BeneficiaryId,
                //         Value = cell.Value
                //     }).ToList();

                await _beneficiaryDataCollectionRepository.InsertRange(beneficiaryDynamicCellData);

                await _beneficiaryDataCollectionStatusRepository.InsertRange(collectionStatuses);

            }

        }
        private async Task CopyFacilityData(long currentInstanceId)
        {
            var previousInstanceCollectionDate = _scheduleInstanceRepository.GetInstanceDataCollectionDate(currentInstanceId);
            var previousInstanceId = _scheduleInstanceRepository.GetPreviousInstanceId(previousInstanceCollectionDate, EntityType.Facility);
            if (previousInstanceId.HasValue && previousInstanceId.Value > 0)
            {

                var facilityDynamicCellData =
                    (from st in _facilityDataCollectionStatusRepository.GetAll()
                     join cell in _facilityDataCollectionRepository.GetAll() on
                         new { st.FacilityId, st.InstanceId } equals
                         new { cell.FacilityId, cell.InstanceId }
                     where st.Status != CollectionStatus.Deleted && st.InstanceId == previousInstanceId
                     select new FacilityDynamicCell
                     {
                         EntityDynamicColumnId = cell.EntityDynamicColumnId,
                         InstanceId = currentInstanceId,
                         FacilityId = cell.FacilityId,
                         Value = cell.Value
                     }).ToList();

                await _facilityDataCollectionRepository.InsertRange(facilityDynamicCellData);

                var collectionStatuses = await _facilityDataCollectionStatusRepository.GetAll()
                    .Where(x => x.InstanceId == previousInstanceId && x.Status != CollectionStatus.Deleted)
                    .Select(x => new FacilityDataCollectionStatus()
                    {
                        InstanceId = currentInstanceId,
                        FacilityId = x.FacilityId,
                        Status = (x.Status == CollectionStatus.Inactivated) ? (CollectionStatus)x.Status : CollectionStatus.NotCollected
                    })
                    .ToListAsync();

                await _facilityDataCollectionStatusRepository.InsertRange(collectionStatuses);
            }

        }
        private async Task AddInstanceMapping(long instanceId)
        {
            var maxFacilityInstanceId = await GetMaxFacilityInstanceId();
            var instanceMapping = new InstanceMapping()
            {
                BeneficiaryInstanceId = instanceId,
                FacilityInstanceId = maxFacilityInstanceId
            };
            await _instanceMappingRepository.Insert(instanceMapping);
        }
        private async Task AddDataToDataCollectionStatus(EntityType entityType, long instanceId)
        {
            switch (entityType)
            {
                case EntityType.Beneficiary:
                    //await _beneficiaryRepository.StartCollectionForAllBeneficiary(instanceId);
                    await CopyBeneficiaryData(instanceId);
                    await AddInstanceMapping(instanceId);
                    break;
                case EntityType.Facility:
                    //await _facilityRepository.StartCollectionForAllFacility(instanceId);
                    await CopyFacilityData(instanceId);
                    break;
                default:
                    throw new DomainException("Invalid Entity Type");
            }
        }

        public async Task<PagedResponse<ScheduleViewModel>> GetSchedules(ScheduleQueryModel model)
        {
            Expression<Func<Schedule, bool>> filter = x =>
                     x.ScheduleFor == model.ScheduleFor;

            var total = _scheduleRepository.GetAll().Where(filter).Count();

            var data = await _scheduleRepository.GetAll()
                .Where(filter)
                .OrderByDescending(a => a.StartDate)
                .ToListAsync();

            var mappedData = _mapper.Map<IEnumerable<ScheduleViewModel>>(data);
            return new PagedResponse<ScheduleViewModel>(mappedData, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<InstanceViewModel>> GetCompletedInstances(BaseQueryModel model)
        {
            Expression<Func<Instance, bool>> filter = x => x.Status == InstanceStatus.Completed;

            var total = _scheduleInstanceRepository.GetAll()
                .Where(filter)
                .Count();

            var data = await _scheduleInstanceRepository.GetAll()
                .Include(x => x.Schedule)
                .ThenInclude(x => x.Frequency)
                .Where(filter)
                .OrderByDescending(x => x.DataCollectionDate)
                .Skip(model.Skip())
                .Take(model.PageSize)
                .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<InstanceViewModel>>(data);
            return new PagedResponse<InstanceViewModel>(mapped, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<InstanceViewModel>> GetRunningInstances(ScheduleInstanceQueryModel model)
        {
            Expression<Func<Instance, bool>> filter =
                    instance => instance.Status == InstanceStatus.Running &&
                    instance.Schedule.ScheduleFor == model.ScheduleFor && !instance.Schedule.IsDeleted;

            var total = _scheduleInstanceRepository.GetAll()
                .Where(filter)
                .Count();

            var data = await _scheduleInstanceRepository.GetAll()
                .Include(instance => instance.Schedule)
                .ThenInclude(x => x.Frequency)
                .Where(filter)
                .OrderByDescending(instance => instance.DataCollectionDate)
                .Skip(model.Skip())
                .Take(model.PageSize)
                .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<InstanceViewModel>>(data);


            return new PagedResponse<InstanceViewModel>(mapped, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<InstanceViewModel>> GetCurrentScheduleInstances(ScheduleInstanceQueryModel model)
        {

            //Expression<Func<Instance, bool>> filter =
            //        x => x.Schedule.ScheduleFor == model.ScheduleFor &&
            //        x.Schedule.Status != ScheduleStatus.Completed &&
            //        !x.Schedule.IsDeleted;

            Expression<Func<Instance, bool>> filter =
                    x => x.Schedule.ScheduleFor == model.ScheduleFor;

            var total = _scheduleInstanceRepository.GetAll()
                .Where(filter)
               .Count();

            var instances = await _scheduleInstanceRepository.GetAll()
               .Include(x => x.Schedule)
               .ThenInclude(x => x.Frequency)
               .Where(filter)
               .OrderByDescending(a=>a.Status==InstanceStatus.Running)
               .ThenBy(a => a.Status)
               .ThenBy(a => a.DataCollectionDate)
               .Skip(model.Skip())
               .Take(model.PageSize)
               .ToListAsync();
            var data = _mapper.Map<IEnumerable<InstanceViewModel>>(instances);

            return await Task.FromResult(new PagedResponse<InstanceViewModel>(data, total, model.PageNo, model.PageSize));
        }
        public async Task<PagedResponse<InstanceViewModel>> GetNotPendingInstances(ScheduleInstanceQueryModel model)
        {
            Expression<Func<Instance, bool>> filter = x => x.Status != InstanceStatus.Pending &&
                    x.Schedule.ScheduleFor == model.ScheduleFor;

            var total = _scheduleInstanceRepository.GetAll()
                .Where(filter)
                .Count();

            var data = await _scheduleInstanceRepository.GetAll()
                .Include(x => x.Schedule)
                .ThenInclude(x => x.Frequency)
                .Where(filter)
                 .OrderBy(a => a.Status)
               .ThenBy(a => a.DataCollectionDate)
                .Skip(model.Skip())
                .Take(model.PageSize)
                .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<InstanceViewModel>>(data);
            return new PagedResponse<InstanceViewModel>(mapped, total, model.PageNo, model.PageSize);
        }
        public async Task<PagedResponse<InstanceViewModel>> GetInstancesStatusWise(ScheduleInstanceStatusWiseQueryModel model)
        {
            // model.InstanceStatus = InstanceStatus.Completed | InstanceStatus.Pending;
            InstanceStatus instanceStatus = (InstanceStatus)model.InstanceStatus;
            Expression<Func<Instance, bool>> filter = x => ((instanceStatus & x.Status) == x.Status) &&
                    x.Schedule.ScheduleFor == model.ScheduleFor;

            var total = _scheduleInstanceRepository.GetAll()
                .Where(filter)
                .Count();

            var data = await _scheduleInstanceRepository.GetAll()
                .Include(x => x.Schedule)
                .ThenInclude(x => x.Frequency)
                .Where(filter)
                .OrderBy(a => a.Status)
               .ThenBy(a => a.DataCollectionDate)
                .Skip(model.Skip())
                .Take(model.PageSize)
                .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<InstanceViewModel>>(data);
            return new PagedResponse<InstanceViewModel>(mapped, total, model.PageNo, model.PageSize);
        }

        public async Task CompleteInstance(long instanceId)
        {


            var schedule = await _scheduleRepository.GetAll()
                .Include(x => x.Instances)
                .FirstOrDefaultAsync(x => x.Instances.Any(x => x.Id == instanceId) && !x.IsDeleted);

            if (schedule == null)
            {
                throw new RecordNotFound();
            }

            var instance = schedule.Instances.FirstOrDefault(x => x.Id == instanceId);
            instance.Status = InstanceStatus.Completed;
            await _scheduleInstanceRepository.Update(instance);

            // await DeleteUnapprovedData(schedule, instanceId);
            var cnt = schedule.Instances.Where(x => x.Status != InstanceStatus.Completed).Count();
            if (cnt == 0)
            {
                schedule.Status = ScheduleStatus.Completed;
                await _scheduleRepository.Update(schedule);
            }
        }

        public Task<long> GetMaxFacilityInstanceId()
        {
            return _scheduleInstanceRepository.GetMaxInstanceId(EntityType.Facility);
        }

        public Task<long> GetMaxBeneficiaryInstanceId()
        {
            return _scheduleInstanceRepository.GetMaxInstanceId(EntityType.Beneficiary);
        }

        public async Task<long> GetMappingFacilityInstanceId(long instanceId)
        {
            return await _instanceMappingRepository.GetAll().Where(a => a.BeneficiaryInstanceId == instanceId)
                .Select(a => a.FacilityInstanceId).FirstOrDefaultAsync();
        }
        public async Task<long> GetMappingBeneficiaryInstanceId(long instanceId)
        {
            return await _instanceMappingRepository.GetAll().Where(a => a.FacilityInstanceId == instanceId)
                .Select(a => a.BeneficiaryInstanceId).FirstOrDefaultAsync();
        }

        private async Task DeleteUnapprovedData(Schedule schedule, long instanceId)
        {
            if (schedule.ScheduleFor == EntityType.Beneficiary)
            {
                var unApproveBenefiriary = await _beneficiaryDataCollectionStatusRepository.GetAll()
                    .Where(a => a.InstanceId == instanceId && a.Status != CollectionStatus.Approved)
                    .ToListAsync();
                await _beneficiaryDataCollectionStatusRepository.DeleteRange(unApproveBenefiriary);
            }
            else if (schedule.ScheduleFor == EntityType.Facility)
            {
                var unApproveFacility = await _facilityDataCollectionStatusRepository.GetAll()
                    .Where(a => a.InstanceId == instanceId && a.Status != CollectionStatus.Approved)
                    .ToListAsync();
                await _facilityDataCollectionStatusRepository.DeleteRange(unApproveFacility);
            }
        }
        public async Task UpdateScheduleInstance(InstanceUpdateViewModel model)
        {
            var entity = await _scheduleInstanceRepository.ThrowIfNotFound(model.Id);
            entity.Title = model.Title;
            await _scheduleInstanceRepository.Update(entity);
        }
    }
}
