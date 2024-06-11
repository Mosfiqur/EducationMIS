using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class BeneficiaryDynamicCellRepository : IBeneficiaryDynamicCellRepository
    {
        private const string beneficiaryDataCollRecordNotFound = "Record not found in BeneciaryDataCollectionStatuses";
        private const string scheduleInstanceRecordNotFound = "Record not found in ScheduleInstance";

        private readonly UnicefEduDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentLoginUserService _currentLoginUserService;
        private readonly IScheduleInstanceRepository _scheduleInstanceRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BeneficiaryDynamicCellRepository> _logger;
        public BeneficiaryDynamicCellRepository(
            UnicefEduDbContext context,
            IMapper mapper,
            ICurrentLoginUserService currentLoginUserService,
            ILogger<BeneficiaryDynamicCellRepository> logger,
            IScheduleInstanceRepository scheduleInstanceRepository,
            IServiceProvider serviceProvider)
        {
            _context = context;
            _mapper = mapper;
            _currentLoginUserService = currentLoginUserService;
            _logger = logger;
            _scheduleInstanceRepository = scheduleInstanceRepository;
            _serviceProvider = serviceProvider;
        }
        public async Task Save(BeneficiaryDynamicCellAddViewModel model)
        {
            await SaveOnly(model);
            await SetStatusTo(CollectionStatus.Collected, model.InstanceId, model.BeneficiaryId);
            await _context.SaveChangesAsync();
        }

        private async Task SaveOnly(BeneficiaryDynamicCellAddViewModel model)
        {
            var dynamicCells = (from item in model.DynamicCells
                                from cell in item.Value
                                select new BeneficiaryDynamicCell
                                {
                                    EntityDynamicColumnId = item.EntityDynamicColumnId,
                                    Value = cell,
                                    BeneficiaryId = model.BeneficiaryId,
                                    InstanceId = model.InstanceId,
                                    Status = CollectionStatus.Collected,
                                    LastUpdated = DateTime.Now,
                                    UpdatedBy = _currentLoginUserService.UserId,
                                }).ToList();

            Expression<Func<BeneficiaryDynamicCell, bool>> filter =
                x => dynamicCells.Select(x => x.EntityDynamicColumnId).Contains(x.EntityDynamicColumnId)
                     && x.InstanceId == model.InstanceId && x.BeneficiaryId == model.BeneficiaryId;

            var existingEntries = _context.BeneficiaryDynamicCells.Where(filter).ToList();
            _context.BeneficiaryDynamicCells.RemoveRange(existingEntries);
            await _context.BeneficiaryDynamicCells.AddRangeAsync(dynamicCells);
        }

        private async Task SetStatusTo(CollectionStatus collectionStatus, long instanceId, long beneficiaryId)
        {

            var entity = await _context.BeneciaryDataCollectionStatuses.Where(a => a.BeneficiaryId == beneficiaryId && a.InstanceId == instanceId).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new RecordNotFound(beneficiaryDataCollRecordNotFound);
            }

            var isAllCollected = await IsAllIndicatorDataCollected(instanceId, beneficiaryId);
            if (!isAllCollected)
            {
                entity.Status = CollectionStatus.NotCollected;
            }
            else
            {
                entity.Status = collectionStatus;
            }
            
            _context.BeneciaryDataCollectionStatuses.Update(entity);

        }

        private async Task<bool> IsAllIndicatorDataCollected(long instanceId, long beneficairyId)
        {
            var allOptionalIndicators = BeneficairyFixedColumns.Optional();

            var instanceIndicators = await _context.InstanceIndicators
                .Where(a => a.InstanceId == instanceId && !allOptionalIndicators.Contains(a.EntityDynamicColumnId))
                .Select(a => a.EntityDynamicColumnId)
                .ToListAsync();

            var addedDynamicCell = await _context.BeneficiaryDynamicCells
                .Where(a => a.BeneficiaryId == beneficairyId 
                && instanceIndicators.Contains(a.EntityDynamicColumnId) && a.InstanceId == instanceId
                && a.Status==CollectionStatus.Collected)
                .Select(a => a.EntityDynamicColumnId)
                .Distinct()
                .ToListAsync();

            var lstDynamicCellIdicator = addedDynamicCell
                .Union(_context.BeneficiaryDynamicCells.Local.Where(a =>a.InstanceId==instanceId 
                && a.BeneficiaryId == beneficairyId && instanceIndicators.Contains(a.EntityDynamicColumnId)
                && a.Status==CollectionStatus.Collected)
                    .Select(a => a.EntityDynamicColumnId))
                .Distinct().ToList().Except(allOptionalIndicators).ToList();
            return instanceIndicators.Count == lstDynamicCellIdicator.Count;

        }
        public async Task Delete(BeneficiaryDynamicCellDeleteViewModel dynamicCell)
        {
            var beneficiaryDynamicCells = await _context.BeneficiaryDynamicCells
                .Where(a => a.InstanceId == dynamicCell.InstanceId && a.BeneficiaryId == dynamicCell.BeneficiaryId
                    && a.EntityDynamicColumnId == dynamicCell.EntityDynamicColumnId).ToListAsync();

            if (beneficiaryDynamicCells.Count == 0)
                throw new RecordNotFound("No Beneficiary Dynamic Cells Found");

            _context.RemoveRange(beneficiaryDynamicCells);
            await _context.SaveChangesAsync();
        }

        public async Task ImportVersionData(List<BeneficiaryDynamicCellAddViewModel> items, CollectionStatus collectionStatus, long instanceId)
        {
            var beneficiaryIds = items.Select(x => x.BeneficiaryId).Distinct().ToList();
            await DeleteAll(beneficiaryIds, instanceId);
            // bulk insert 
            var dynamicCells =
            items.SelectMany(dc =>
                dc.DynamicCells.SelectMany(cell =>
                    cell.Value.Select(val =>
                    new BeneficiaryDynamicCell
                    {
                        EntityDynamicColumnId = cell.EntityDynamicColumnId,
                        Value = val,
                        BeneficiaryId = dc.BeneficiaryId,
                        InstanceId = dc.InstanceId,
                        Status = CollectionStatus.Collected,
                    }).ToList()
                    ).ToList()
                ).ToList();
            await _context.BeneficiaryDynamicCells.AddRangeAsync(dynamicCells);

            // Change data collection status
            var entry = await _context
                .BeneciaryDataCollectionStatuses
                .Where(a => beneficiaryIds.Contains(a.BeneficiaryId) && a.InstanceId == instanceId)
                .ToListAsync();
            entry.ForEach(x => x.Status = collectionStatus);
            _context.BeneciaryDataCollectionStatuses.UpdateRange(entry);
            // Change instance status
            var scheduleInstance = await _scheduleInstanceRepository.ThrowIfNotFound(instanceId);
            scheduleInstance.Status = InstanceStatus.Running;
            _context.ScheduleInstances.Update(scheduleInstance);
            await _context.SaveChangesAsync();
        }

        private async Task DeleteAll(List<long> ids, long instanceId)
        {
            var tobeDeleted = string.Join(",", ids.ToArray());
            var trans = await _context.Database.BeginTransactionAsync();
            var query = $"DELETE FROM `{TableNames.BeneficiaryDynamicCell}` WHERE InstanceId = {instanceId} AND BeneficiaryId IN ({tobeDeleted})";
            _context.Database.ExecuteSqlRaw(query);
            await trans.CommitAsync();
        }

        public async Task Revert(BeneficiaryDynamicCellAddViewModel model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _ucontext =
               scope.ServiceProvider
                   .GetRequiredService<UnicefEduDbContext>();

                Expression<Func<BeneficiaryDynamicCell, bool>> filter =
            x => model.DynamicCells.Select(x => x.EntityDynamicColumnId).Contains(x.EntityDynamicColumnId)
            && x.InstanceId == model.InstanceId && x.BeneficiaryId == model.BeneficiaryId;

                var existingEntries = await _ucontext.BeneficiaryDynamicCells.Where(filter).ToListAsync();
                if (existingEntries.Count > 0)
                    _ucontext.BeneficiaryDynamicCells.RemoveRange(existingEntries);

                var entry = await _ucontext.BeneciaryDataCollectionStatuses.Where(a => a.BeneficiaryId == model.BeneficiaryId && a.InstanceId == model.InstanceId).FirstOrDefaultAsync();
                if (entry != null)
                {
                    _ucontext.BeneciaryDataCollectionStatuses.Remove(entry);
                }
                await _ucontext.SaveChangesAsync();
            }
        }

    }
}
