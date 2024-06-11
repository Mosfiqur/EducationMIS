using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class IndicatorService : IIndicatorService
    {
        private readonly IIndicatorRepository _indicatorRepository;
        private readonly IMapper _mapper;
        private readonly IScheduleService _scheduleService;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IBeneficiaryDataCollectionRepository _beneficiaryDataCollectionRepository;
        private readonly IScheduleInstanceRepository _scheduleInstanceRepository;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IFacilityDataCollectionRepository _facilityDataCollectionRepository;

        public IndicatorService(IIndicatorRepository indicatorRepository, IMapper mapper
            , IScheduleService scheduleService, IBeneficiaryRepository beneficiaryRepository
            , IBeneficiaryDataCollectionRepository beneficiaryDataCollectionRepository
            , IScheduleInstanceRepository scheduleInstanceRepository
            , IFacilityRepository facilityRepository
            , IFacilityDataCollectionRepository facilityDataCollectionRepository
            )
        {
            _indicatorRepository = indicatorRepository;
            _mapper = mapper;
            _scheduleService = scheduleService;
            _beneficiaryRepository = beneficiaryRepository;
            _beneficiaryDataCollectionRepository = beneficiaryDataCollectionRepository;
            _scheduleInstanceRepository = scheduleInstanceRepository;
            _facilityRepository = facilityRepository;
            _facilityDataCollectionRepository = facilityDataCollectionRepository;
        }

        public async Task Insert(List<AddInstanceIndicatorViewModel> instanceIndicators)
        {
            var ind = _mapper.Map<List<InstanceIndicator>>(instanceIndicators);
            await _indicatorRepository.Insert(ind);
        }

        public async Task Update(List<AddInstanceIndicatorViewModel> instanceIndicators)
        {
            var ind = _mapper.Map<List<InstanceIndicator>>(instanceIndicators);
            await _indicatorRepository.Update(ind);
        }
        public async Task Delete(List<DeleteIndicatorViewModel> indicator)
        {
            await _indicatorRepository.Delete(indicator);
        }
        public async Task<PagedResponse<BeneficairyWiseIndicatorViewModel>> GetBeneficiaryIndicators(IndicatorByBeneficairyWiseQueryModel model)
        {
            //TODO: beneficiary new schema
            var beneficiariesQuery = await _beneficiaryDataCollectionRepository.GetAll()
               .Where(a => a.BeneficiaryId == model.BeneficiaryId && a.InstanceId == model.InstanceId
               && a.EntityDynamicColumnId == BeneficairyFixedColumns.Name)
               .Select(a => new { BeneficiaryId = model.BeneficiaryId, BeneficiaryName = a.Value })
               .FirstOrDefaultAsync();

            var instanceIndicator = await _indicatorRepository.GetAll()
                .Where(a => a.InstanceId == model.InstanceId)
                .Select(a => new EntityDynamicColumnViewModel()
                {
                    EntityDynamicColumnId = a.EntityDynamicColumnId,
                    ColumnName = a.EntityDynamicColumn.Name,
                    ColumnNameInBangla = a.EntityDynamicColumn.NameInBangla,
                    ColumnOrder = a.ColumnOrder,
                    ColumnDataType = a.EntityDynamicColumn.ColumnType,
                    IsMultiValued = a.EntityDynamicColumn.IsMultiValued,
                    ListObjectId = a.EntityDynamicColumn.ColumnListId,
                    ListObjectName = a.EntityDynamicColumn.ColumnList.Name,
                    ListItems = a.EntityDynamicColumn.ColumnList.ListItems
                }).ToListAsync();

            var presentInstanceCollectinDate = _scheduleInstanceRepository.GetAll()
                .Where(a => a.Id == model.InstanceId).Select(a => a.DataCollectionDate).FirstOrDefault();

            var isDataExistInCurrentInstance = await _beneficiaryDataCollectionRepository.GetAll()
                .AnyAsync(a => a.BeneficiaryId == model.BeneficiaryId && a.InstanceId == model.InstanceId && a.Status != CollectionStatus.NotCollected);

            List<BeneficiaryDynamicCell> cellData = new List<BeneficiaryDynamicCell>();

            cellData = await _beneficiaryDataCollectionRepository.GetAll()
                .Where(a => a.InstanceId == model.InstanceId && a.BeneficiaryId == model.BeneficiaryId).ToListAsync();

            var instanceWithCellData =
                (from i in instanceIndicator
                 join c in cellData on i.EntityDynamicColumnId equals c.EntityDynamicColumnId into def1
                 from dynamicCell in def1.DefaultIfEmpty()
                 select new { indicatorData = i, cellData = dynamicCell }
                 ).ToList();


            var indicatorList = instanceWithCellData.GroupBy(a => a.indicatorData.EntityDynamicColumnId)
                .Select(a => new IndicatorGetViewModel
                {
                    EntityDynamicColumnId = a.Key,
                    ColumnName = a.Select(b => b.indicatorData.ColumnName).FirstOrDefault(),
                    ColumnNameInBangla = a.Select(b => b.indicatorData.ColumnNameInBangla).FirstOrDefault(),
                    ColumnDataType = a.Select(b => b.indicatorData.ColumnDataType).FirstOrDefault(),
                    ColumnOrder = a.Select(b => b.indicatorData.ColumnOrder).FirstOrDefault(),
                    IsMultiValued = a.Select(b => b.indicatorData.IsMultiValued).FirstOrDefault(),
                    ListObjectId = a.Select(b => b.indicatorData.ListObjectId).FirstOrDefault(),
                    ListObjectName = a.Select(b => b.indicatorData.ListObjectName).FirstOrDefault(),
                    Status = isDataExistInCurrentInstance ?
                            a.Select(b => b.cellData != null ? b.cellData.Status : CollectionStatus.NotCollected).FirstOrDefault()
                            : CollectionStatus.NotCollected,
                    DataCollectionDate = presentInstanceCollectinDate,
                    ListItems = a.Select(b => b.indicatorData.ListItems.Select(c => new ListItemViewModel()
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Value = c.Value
                    }).ToList()).FirstOrDefault(),
                    Values = a.Where(b => b.cellData != null).Select(b => b.cellData.Value).ToList()
                }).OrderBy(a => a.ColumnOrder).ToList();

            var data = new List<BeneficairyWiseIndicatorViewModel>
            {
                new BeneficairyWiseIndicatorViewModel()
                {
                    BeneficiaryId = beneficiariesQuery.BeneficiaryId,
                    BeneficairyName = beneficiariesQuery.BeneficiaryName,
                    Indicators = indicatorList,
                    InstanceId = model.InstanceId
                }
            };
            var total = indicatorList.Count;
            return new PagedResponse<BeneficairyWiseIndicatorViewModel>(data, total, model.PageNo, model.PageSize);
            //return await _indicatorRepository.GetBeneficiaryIndicators(model);
        }

        public async Task<PagedResponse<FacilityWiseIndicatorViewModel>> GetFacilityIndicators(IndicatorByFacilityWiseQueryModel model)
        {
            var facilityQuery = await _facilityDataCollectionRepository.GetAll()
               .Where(a => a.FacilityId == model.FacilityId && a.InstanceId == model.InstanceId
               && a.EntityDynamicColumnId == FacilityFixedColumns.Name)
               .Select(a => new { FacilityId = model.FacilityId, FacilityName = a.Value })
               .FirstOrDefaultAsync();

            var instanceIndicator = await _indicatorRepository.GetAll()
                .Where(a => a.InstanceId == model.InstanceId)
                .Select(a => new EntityDynamicColumnViewModel()
                {
                    EntityDynamicColumnId = a.EntityDynamicColumnId,
                    ColumnName = a.EntityDynamicColumn.Name,
                    ColumnNameInBangla = a.EntityDynamicColumn.NameInBangla,
                    ColumnOrder = a.ColumnOrder,
                    ColumnDataType = a.EntityDynamicColumn.ColumnType,
                    IsMultiValued = a.EntityDynamicColumn.IsMultiValued,
                    ListObjectId = a.EntityDynamicColumn.ColumnListId,
                    ListObjectName = a.EntityDynamicColumn.ColumnList.Name,
                    ListItems = a.EntityDynamicColumn.ColumnList.ListItems
                }).ToListAsync();

            var presentInstanceCollectinDate = await _scheduleInstanceRepository.GetAll()
                .Where(a => a.Id == model.InstanceId).Select(a => a.DataCollectionDate).FirstOrDefaultAsync();

            var isDataExistInCurrentInstance = await _facilityDataCollectionRepository.GetAll()
                .AnyAsync(a => a.FacilityId == model.FacilityId && a.InstanceId == model.InstanceId && a.Status != CollectionStatus.NotCollected);

            List<FacilityDynamicCell> cellData = new List<FacilityDynamicCell>();

            cellData = await _facilityDataCollectionRepository.GetAll()
                     .Where(a => a.InstanceId == model.InstanceId && a.FacilityId == model.FacilityId).ToListAsync();

            var instanceWithCellData =
                (from i in instanceIndicator
                 join c in cellData on i.EntityDynamicColumnId equals c.EntityDynamicColumnId into def1
                 from dynamicCell in def1.DefaultIfEmpty()
                 select new { indicatorData = i, cellData = dynamicCell }
                 ).ToList();


            var indicatorList = instanceWithCellData.GroupBy(a => a.indicatorData.EntityDynamicColumnId)
                .Select(a => new IndicatorGetViewModel
                {
                    EntityDynamicColumnId = a.Key,
                    ColumnName = a.Select(b => b.indicatorData.ColumnName).FirstOrDefault(),
                    ColumnNameInBangla = a.Select(b => b.indicatorData.ColumnNameInBangla).FirstOrDefault(),
                    ColumnDataType = a.Select(b => b.indicatorData.ColumnDataType).FirstOrDefault(),
                    ColumnOrder = a.Select(b => b.indicatorData.ColumnOrder).FirstOrDefault(),
                    IsMultiValued = a.Select(b => b.indicatorData.IsMultiValued).FirstOrDefault(),
                    ListObjectId = a.Select(b => b.indicatorData.ListObjectId).FirstOrDefault(),
                    ListObjectName = a.Select(b => b.indicatorData.ListObjectName).FirstOrDefault(),
                    DataCollectionDate = presentInstanceCollectinDate,
                    Status = isDataExistInCurrentInstance ?
                            a.Select(b => b.cellData != null ? b.cellData.Status : CollectionStatus.NotCollected).FirstOrDefault()
                            : CollectionStatus.NotCollected,
                    ListItems = a.Select(b => b.indicatorData.ListItems.Select(c => new ListItemViewModel()
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Value = c.Value
                    }).ToList()).FirstOrDefault(),
                    Values = a.Where(b => b.cellData != null).Select(b => b.cellData.Value).ToList()
                }).OrderBy(a => a.ColumnOrder).ToList();

            var data = new List<FacilityWiseIndicatorViewModel>
            {
                new FacilityWiseIndicatorViewModel()
                {
                    FacilityId = facilityQuery.FacilityId,
                    FacilityName = facilityQuery.FacilityName,
                    Indicators = indicatorList,
                    InstanceId = model.InstanceId
                }
            };
            var total = indicatorList.Count;
            return new PagedResponse<FacilityWiseIndicatorViewModel>(data, total, model.PageNo, model.PageSize);


        }

        public async Task<PagedResponse<IndicatorSelectViewModel>> GetIndicatorsByInstance(IndicatorsByInstanceQueryModel model)
        {
            return await _indicatorRepository.GetIndicatorsByInstance(model);
        }
    }
}
