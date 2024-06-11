using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class FacilityDynamicCellService : IFacilityDynamicCellService
    {
        private readonly IFacilityDynamicCellRepository _facilityDynamicCellRepository;
        private readonly IDynamicColumnRepositories _dynamicColumnRepositories;
        private readonly IScheduleInstanceRepository _scheduleInstanceRepository;
        private readonly IFacilityDataCollectionStatusRepository _collectionStatusRepository;

        public FacilityDynamicCellService(
            IFacilityDynamicCellRepository facilityDynamicCellRepository,
            IDynamicColumnRepositories dynamicColumnRepositories, 
            IScheduleInstanceRepository scheduleInstanceRepository, IFacilityDataCollectionStatusRepository collectionStatusRepository)
        {
            _facilityDynamicCellRepository = facilityDynamicCellRepository;
            _dynamicColumnRepositories = dynamicColumnRepositories;
            _scheduleInstanceRepository = scheduleInstanceRepository;
            _collectionStatusRepository = collectionStatusRepository;
        }

        public async Task Delete(FacilityDynamicCellDeleteViewModel model)
        {
            var status = await _collectionStatusRepository.GetAll()
                .SingleOrDefaultAsync(x => x.InstanceId == model.InstanceId && x.FacilityId == model.FacilityId);
            if (status.Status == CollectionStatus.Deleted)
            {
                throw new DomainException(Messages.ItemIsNonEditable);
            }
            await _facilityDynamicCellRepository.Delete(model);
        }
        private async Task<bool> IsInvalidCell(List<DynamicCellViewModel> dynamicCells)
        {
            bool returnVal = true;
            var listTypeDynamicColumn = await _dynamicColumnRepositories.GetAll()
                                       .Where(a => a.EntityTypeId == EntityType.Facility && a.ColumnType == ColumnDataType.List).ToListAsync();

            var allListValue = listTypeDynamicColumn
                .Join(dynamicCells, entityColumn => entityColumn.Id, cells => cells.EntityDynamicColumnId
                  , (entityColumn, cells) => new { entityColumn, cells }).SelectMany(a => a.cells.Value).ToList();

            returnVal = allListValue.Any(a => !a.Any(char.IsDigit));

            return returnVal;
        }
        public async Task Save(FacilityDynamicCellAddViewModel model)
        {
            if (await IsInvalidCell(model.DynamicCells))
            {
                throw new InvalidListTypeDataException();
            }
            var isRunningInstance = await _scheduleInstanceRepository.IsRunningInstance(model.InstanceId);
            if (!isRunningInstance)
            {
                throw new DomainException(Messages.InstanceNotRunning);
            }
            var status = await _collectionStatusRepository.GetAll()
                .SingleOrDefaultAsync(x => x.InstanceId == model.InstanceId && x.FacilityId == model.FacilityId);
            if (status.Status == CollectionStatus.Deleted)
            {
                throw new DomainException(Messages.ItemIsNonEditable);
            }
            await _facilityDynamicCellRepository.Save(model);
        }
      
        public async Task ImportVersionData(List<FacilityDynamicCellAddViewModel> cellvmList, CollectionStatus collectionStatus, long instanceId)
        {
            await _facilityDynamicCellRepository.ImportVersionData(cellvmList, collectionStatus, instanceId);
        }

    }
}
