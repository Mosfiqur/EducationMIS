using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class BeneficiaryDynamicCellService : IBeneficiaryDynamicCellService
    {
        private readonly IBeneficiaryDynamicCellRepository _beneficiaryDynamicCellRepository;
        private readonly IDynamicColumnRepositories _dynamicColumnRepositories;
        private readonly IScheduleInstanceRepository _scheduleInstanceRepository;
        private readonly IBeneficiaryDataCollectionStatusRepository _collectionStatusRepository;

        public BeneficiaryDynamicCellService(IBeneficiaryDynamicCellRepository beneficiaryDynamicCellRepository,
            IDynamicColumnRepositories dynamicColumnRepositories
            ,IScheduleInstanceRepository scheduleInstanceRepository
            ,IBeneficiaryDataCollectionStatusRepository collectionStatusRepository)
        {
            _beneficiaryDynamicCellRepository = beneficiaryDynamicCellRepository;
            _dynamicColumnRepositories = dynamicColumnRepositories;
            _scheduleInstanceRepository = scheduleInstanceRepository;
            _collectionStatusRepository = collectionStatusRepository;
        }

        public async Task Delete(BeneficiaryDynamicCellDeleteViewModel dynamicCell)
        {
            await _beneficiaryDynamicCellRepository.Delete(dynamicCell);
        }


        private async Task<bool> IsInvalidCell(List<DynamicCellViewModel> dynamicCells)
        {
            bool returnVal = true;
            var listTypeDynamicColumn = await _dynamicColumnRepositories.GetAll()
                                       .Where(a => a.EntityTypeId == EntityType.Beneficiary && a.ColumnType == ColumnDataType.List).ToListAsync();
            var allListValue = listTypeDynamicColumn
                .Join(dynamicCells, entityColumn => entityColumn.Id, cells => cells.EntityDynamicColumnId
                  , (entityColumn, cells) => new { entityColumn, cells }).SelectMany(a => a.cells.Value).ToList();

            returnVal = allListValue.Any(a=> !a.Any(char.IsDigit));

            return returnVal;
        }

        public async Task Save(BeneficiaryDynamicCellAddViewModel dynamicCell)
        {
            if (await IsInvalidCell(dynamicCell.DynamicCells))
            {
                throw new InvalidListTypeDataException();
            }
            var isRunningInstance = await _scheduleInstanceRepository.IsRunningInstance(dynamicCell.InstanceId);
            if (!isRunningInstance)
            {
                throw new DomainException(Messages.InstanceNotRunning);
            }
            var status = await _collectionStatusRepository.GetAll()
                .SingleOrDefaultAsync(x => x.InstanceId == dynamicCell.InstanceId && x.BeneficiaryId == dynamicCell.BeneficiaryId);
            if (status.Status == CollectionStatus.Deleted)
            {
                throw new DomainException(Messages.ItemIsNonEditable);
            }
            await _beneficiaryDynamicCellRepository.Save(dynamicCell);
        }
    }
}
