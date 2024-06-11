using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.Repositories
{
    public class FacilityDynamicCellRepository : BaseRepository<FacilityDynamicCell, long>, IFacilityDynamicCellRepository
    {
        private const string facilityDataCollRecordNotFound = "Record not found in FacilityDataCollectionStatuses";
        private const string scheduleInstanceRecordNotFound = "Record not found in ScheduleInstance";

        private readonly IMapper _mapper;
        private readonly ICurrentLoginUserService _currentLoginUserService;
        public FacilityDynamicCellRepository(UnicefEduDbContext context, IMapper mapper, ICurrentLoginUserService currentLoginUserService)
            :base(context)
        {            
            _mapper = mapper;
            _currentLoginUserService = currentLoginUserService;
        }

        public async Task Delete(FacilityDynamicCellDeleteViewModel dynamicCell)
        {
            var facilityDynamicCells = await _context.FacilityDynamicCells
                .Where(a => a.InstanceId == dynamicCell.InstanceId && a.FacilityId == dynamicCell.FacilityId
                    && a.EntityDynamicColumnId == dynamicCell.EntityDynamicColumnId).ToListAsync();

            if (facilityDynamicCells.Count == 0)
                throw new RecordNotFound($"Facility Dynamic Cell Not Found");

            _context.RemoveRange(facilityDynamicCells);
            await _context.SaveChangesAsync();
        }

        public async Task Save(FacilityDynamicCellAddViewModel dynamicCell)
        {
            await facilityDynamicCellSave(dynamicCell);
            await ChangeCollectionStatus(dynamicCell.InstanceId, dynamicCell.FacilityId, CollectionStatus.Collected);
            await _context.SaveChangesAsync();
        }
        public async Task ImportVersionData(FacilityDynamicCellAddViewModel dynamicCell, CollectionStatus collectionStatus)
        {
            await facilityDynamicCellSave(dynamicCell);
            await ChangeCollectionStatusForImport(dynamicCell.InstanceId, dynamicCell.FacilityId, collectionStatus);
            await _context.SaveChangesAsync();
        }
        private async Task facilityDynamicCellSave(FacilityDynamicCellAddViewModel dynamicCell)
        {
            List<FacilityDynamicCell> lstFacilityDynamicCell = (from item in dynamicCell.DynamicCells
                                                                from cell in item.Value
                                                                select new FacilityDynamicCell
                                                                {

                                                                    EntityDynamicColumnId = item.EntityDynamicColumnId,
                                                                    Value = cell,
                                                                    FacilityId = dynamicCell.FacilityId,
                                                                    InstanceId = dynamicCell.InstanceId,
                                                                    LastUpdated = DateTime.Now,
                                                                    UpdatedBy = _currentLoginUserService.UserId,
                                                                    Status = CollectionStatus.Collected
                                                                }).ToList();
            foreach (var item in lstFacilityDynamicCell)
            {
                var dCells = await _context.FacilityDynamicCells
                    .Where(a => a.InstanceId == item.InstanceId && a.FacilityId == item.FacilityId
                    && a.EntityDynamicColumnId == item.EntityDynamicColumnId).ToListAsync();
                _context.FacilityDynamicCells.RemoveRange(dCells);
                _context.FacilityDynamicCells.Add(item);
                
            }
        }
        private async Task ChangeCollectionStatus(long instanceId, long facilityId, CollectionStatus collectionStatus)
        {
            var changeCollectionStatusToCollected = _context.FacilityDataCollectionStatuses
                .FirstOrDefault(a => a.FacilityId == facilityId && a.InstanceId == instanceId);
            if (changeCollectionStatusToCollected == null)
            {
                throw new RecordNotFound(facilityDataCollRecordNotFound);
            }

            var isAllCollected = await IsAllIndicatorDataCollected(instanceId, facilityId);
            if (!isAllCollected)
            {
                changeCollectionStatusToCollected.Status = CollectionStatus.NotCollected;
            }
            else
            {
                changeCollectionStatusToCollected.Status = collectionStatus;
            }

            _context.FacilityDataCollectionStatuses.Update(changeCollectionStatusToCollected);

            var scheduleInstance = await _context.ScheduleInstances.FirstOrDefaultAsync(a => a.Id == instanceId);
            if (scheduleInstance == null)
            {
                throw new RecordNotFound(scheduleInstanceRecordNotFound);
            }
            scheduleInstance.Status = InstanceStatus.Running;
            _context.ScheduleInstances.Update(scheduleInstance);
        }

        private async Task ChangeCollectionStatusForImport(long instanceId, long facilityId, CollectionStatus collectionStatus)
        {
            var changeCollectionStatusToCollected = _context.FacilityDataCollectionStatuses
                .FirstOrDefault(a => a.FacilityId == facilityId && a.InstanceId == instanceId);
            if (changeCollectionStatusToCollected == null)
            {
                throw new RecordNotFound(facilityDataCollRecordNotFound);
            }
            changeCollectionStatusToCollected.Status = collectionStatus;
            _context.FacilityDataCollectionStatuses.Update(changeCollectionStatusToCollected);

            var scheduleInstance = await _context.ScheduleInstances.FirstOrDefaultAsync(a => a.Id == instanceId);
            if (scheduleInstance == null)
            {
                throw new RecordNotFound(scheduleInstanceRecordNotFound);
            }
            scheduleInstance.Status = InstanceStatus.Running;
            _context.ScheduleInstances.Update(scheduleInstance);
        }

        // TODO: Safe delete this method 
        private async Task<bool> IsAllIndicatorDataCollected(long instanceId, long facilityId)
        {
            var allOptionalIndicators = FacilityFixedColumns.DamageRelatedIndicators.Union(FacilityFixedIndicators.Optional());

            var instanceIndicators = await _context.InstanceIndicators
                .Where(a => a.InstanceId == instanceId 
                && !allOptionalIndicators.Contains(a.EntityDynamicColumnId)
                )
                .Select(a => a.EntityDynamicColumnId)
                .ToListAsync();

            var addedDynamicCell = await _context.FacilityDynamicCells
                .Where(a =>a.FacilityId==facilityId && instanceIndicators.Contains(a.EntityDynamicColumnId) 
                && a.InstanceId == instanceId
                && a.Status== CollectionStatus.Collected)
                .Select(a => a.EntityDynamicColumnId)
                .Distinct()
                .ToListAsync();

            var lstDynamicCellIdicator = addedDynamicCell
               .Union(_context.FacilityDynamicCells.Local.Where(a =>a.InstanceId== instanceId 
               && a.FacilityId== facilityId && instanceIndicators.Contains(a.EntityDynamicColumnId)
               && a.Status== CollectionStatus.Collected)
               .Select(a => a.EntityDynamicColumnId))
               .Distinct().ToList().Except(allOptionalIndicators).ToList();
            return instanceIndicators.Count == lstDynamicCellIdicator.Count;
        }

        

        public async Task ImportVersionData(List<FacilityDynamicCellAddViewModel> cellvmList, CollectionStatus collectionStatus, long instanceId)
        {
            var facIds = cellvmList.Select(x => x.FacilityId).Distinct().ToList();
            await Delete(facIds, instanceId);

            var cells = cellvmList.SelectMany(
                x => x.DynamicCells.SelectMany(
                    y => y.Value.Select(
                        val => new FacilityDynamicCell
                        {
                            EntityDynamicColumnId = y.EntityDynamicColumnId,
                            Value = val,
                            FacilityId = x.FacilityId,
                            InstanceId = instanceId,
                            Status = CollectionStatus.Collected
                        }).ToList()
                    ).ToList()
                ).ToList();

            await _context.FacilityDynamicCells.AddRangeAsync(cells);
            await SetDataCollectionStatus(facIds, instanceId, collectionStatus);
            await _context.SaveChangesAsync();
        }
        private async Task Delete(List<long> ids, long instanceId)
        {
            var deleted = string.Join(",", ids.ToArray());
            var trans = await _context.Database.BeginTransactionAsync();
            var query = $"DELETE FROM `{TableNames.FacilityDynamicCell}` WHERE InstanceId = {instanceId} AND FacilityId IN ({deleted})";
            _context.Database.ExecuteSqlRaw(query);
            await trans.CommitAsync();
        }

        private async Task SetDataCollectionStatus(ICollection<long> facIds, long instanceId, CollectionStatus status)
        {
            var dcs = await _context
                .FacilityDataCollectionStatuses
                .Where(a => facIds.Contains(a.FacilityId) && a.InstanceId == instanceId)
                .ToListAsync();

            dcs.ForEach(x => x.Status = status);
            _context.FacilityDataCollectionStatuses.UpdateRange(dcs);
        }

    }
}
