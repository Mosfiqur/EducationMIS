using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.Repositories
{

    public class IndicatorRepository : BaseRepository<InstanceIndicator, long>, IIndicatorRepository
    {
        // private readonly UnicefEduDbContext _context;
        private readonly IMapper _mapper;
        public IndicatorRepository(UnicefEduDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }


        public async Task Insert(List<InstanceIndicator> instanceIndicators)
        {
            var instanceId = instanceIndicators.Select(a => a.InstanceId).First();
            var instacneIndicatorColumnOrderList = await _context.InstanceIndicators.Where(a => a.InstanceId == instanceId)
                                                   .Select(a => a.ColumnOrder).ToListAsync();
            var maxInstancIndicatorColumnOrder = instacneIndicatorColumnOrderList.Count > 0 ? instacneIndicatorColumnOrderList.Max() + 1 : 1;

            foreach (var item in instanceIndicators)
            {
                item.ColumnOrder = maxInstancIndicatorColumnOrder;
                maxInstancIndicatorColumnOrder++;
            }

            _context.InstanceIndicators.AddRange(instanceIndicators);

            await _context.SaveChangesAsync();
        }
        public async Task Update(List<InstanceIndicator> instanceIndicators)
        {
            //var indicator = _context.Indicators.Where(x => x.EntityDynamicColumnId == entity.EntityDynamicColumnId &&
            //                                    x.EntityTypeId == entity.EntityTypeId).FirstOrDefault();
            //if (indicator != null)
            //{
            //    indicator.ColumnOrder = entity.ColumnOrder;
            //    indicator.LastUpdated = entity.LastUpdated;
            //    indicator.UpdatedBy = entity.UpdatedBy;
            //}
            _context.InstanceIndicators.UpdateRange(instanceIndicators);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(List<DeleteIndicatorViewModel> indicator)
        {
            List<InstanceIndicator> lstInstanceIndicators = new List<InstanceIndicator>();

            foreach (var item in indicator)
            {
                var ind = await _context.InstanceIndicators
                                .Where(x => x.InstanceId == item.InstanceId && x.EntityDynamicColumnId == item.EntityDynamicColumnId).FirstOrDefaultAsync();
                if (ind != null)
                    lstInstanceIndicators.Add(ind);

            }

            _context.InstanceIndicators.RemoveRange(lstInstanceIndicators);

            await _context.SaveChangesAsync();

        }


        public async Task<PagedResponse<IndicatorSelectViewModel>> GetIndicatorsByInstance(IndicatorsByInstanceQueryModel model)
        {
            var indicators = _context.InstanceIndicators
                .Where(a => a.InstanceId == model.InstanceId)
                .Select(a => new IndicatorSelectViewModel
                {
                    Id = a.Id,
                    ColumnOrder = a.ColumnOrder,
                    EntityDynamicColumnId = a.EntityDynamicColumnId,
                    IndicatorName = a.EntityDynamicColumn.Name,
                    IndicatorNameInBangla = a.EntityDynamicColumn.NameInBangla,
                    ColumnDataType = a.EntityDynamicColumn.ColumnType,
                    IsMultivalued = a.EntityDynamicColumn.IsMultiValued,
                    ListObject = a.EntityDynamicColumn.ColumnList,
                    ListItems = a.EntityDynamicColumn.ColumnList.ListItems,
                    ColumnListId= a.EntityDynamicColumn.ColumnList.Id

                });

            var total = await indicators.CountAsync();
            if (!(string.IsNullOrWhiteSpace(model.SearchText)))
                indicators = indicators.Where(x => x.IndicatorName == model.SearchText).AsQueryable();

            var paginatedData = await indicators.OrderBy(a => a.ColumnOrder)
                .Skip(model.Skip())
                .Take(model.PageSize).ToListAsync();

            return new PagedResponse<IndicatorSelectViewModel>(paginatedData, total, model.PageNo, model.PageSize);
        }

        public async Task<List<IndicatorSelectViewModel>> GetIndicatorsByInstances(List<long> instanceIds)
        {
            var indicators = await _context.InstanceIndicators
                .Where(a => instanceIds.Contains(a.InstanceId))
                .Select(a => new IndicatorSelectViewModel
                {
                    Id = a.Id,
                    ColumnOrder = a.ColumnOrder,
                    EntityDynamicColumnId = a.EntityDynamicColumnId,
                    IndicatorName = a.EntityDynamicColumn.Name,
                    IndicatorNameInBangla = a.EntityDynamicColumn.NameInBangla,
                    ColumnDataType = a.EntityDynamicColumn.ColumnType,
                    IsMultivalued = a.EntityDynamicColumn.IsMultiValued,
                    ListObject = a.EntityDynamicColumn.ColumnList,
                    ListItems = a.EntityDynamicColumn.ColumnList.ListItems,
                    ColumnListId = a.EntityDynamicColumn.ColumnList.Id

                }).ToListAsync();
                ;

            return indicators;
        }

        private async Task<List<IndicatorRawView>> GetBeneficiaryIndicatorRawViews(long indicatorInstanceId, long dynamicCellInstanceId, long id)
        {
            var indicatorRaw = await (from bdcs in _context.BeneciaryDataCollectionStatuses
                                      join b in _context.Beneficiary on bdcs.BeneficiaryId equals b.Id
                                      join ii in _context.InstanceIndicators on bdcs.InstanceId equals ii.InstanceId
                                      join si in _context.ScheduleInstances on ii.InstanceId equals si.Id
                                      join edc in _context.EntityDynamicColumn on ii.EntityDynamicColumnId equals edc.Id
                                      join lo in _context.ListDataType on edc.ColumnListId equals lo.Id into def
                                      from listObject in def.DefaultIfEmpty()
                                      join li in _context.ListItems on listObject.Id equals li.ColumnListId into def1
                                      from listItem in def1.DefaultIfEmpty()
                                      join bdc in _context.BeneficiaryDynamicCells on new { BeneficiaryId = b.Id, ii.EntityDynamicColumnId, InstanceId = dynamicCellInstanceId } equals new { bdc.BeneficiaryId, bdc.EntityDynamicColumnId, bdc.InstanceId } into def2
                                      from dynamicCell in def2.DefaultIfEmpty()
                                      where (ii.InstanceId == indicatorInstanceId && b.Id == id)
                                      select new IndicatorRawView
                                      {
                                          BeneficiaryId = b.Id,
                                          //TODO: beneficiary new schema
                                          //   BeneficairyName = b.Name,
                                          InstanceId = si.Id,

                                          DataCollectionDate = si.DataCollectionDate,
                                          DataCollectionStatusId = bdcs.Id,
                                          CollectionStatus = bdcs.Status,
                                          PropertiesId = dynamicCell.Id,
                                          PropertiesValue = dynamicCell.Value,
                                          EntityColumnId = edc.Id,
                                          EntityColumnName = edc.Name,
                                          ColumnOrder = ii.ColumnOrder,
                                          EntityColumnNameInBangla = edc.NameInBangla,
                                          IsMultiValued = edc.IsMultiValued,
                                          DataType = edc.ColumnType,
                                          ColumnListId = edc.ColumnListId,
                                          ColumnListName = listObject.Name,
                                          ListItemId = listItem.Id,
                                          ListItemTitle = listItem.Title,
                                          ListItemValue = listItem.Value

                                      }).ToListAsync();
            return indicatorRaw;
        }
        public async Task<PagedResponse<BeneficairyWiseIndicatorViewModel>> GetBeneficiaryIndicators(IndicatorByBeneficairyWiseQueryModel model)
        {

            var isDataExistInCurrentInstance = await _context.BeneficiaryDynamicCells
                .AnyAsync(a => a.BeneficiaryId == model.BeneficiaryId && a.InstanceId == model.InstanceId && a.Status != CollectionStatus.NotCollected);
            List<IndicatorRawView> indicatorRaws = new List<IndicatorRawView>();
            List<BeneficairyWiseIndicatorViewModel> indicators = new List<BeneficairyWiseIndicatorViewModel>();

            if (isDataExistInCurrentInstance)
            {
                indicatorRaws = await GetBeneficiaryIndicatorRawViews(model.InstanceId, model.InstanceId, model.BeneficiaryId);

            }
            else
            {
                var presentInstanceCollectinDate = _context.ScheduleInstances.Where(a => a.Id == model.InstanceId).Select(a => a.DataCollectionDate).FirstOrDefault();
                var previousInstanceId = _context.ScheduleInstances.Where(a => a.DataCollectionDate < presentInstanceCollectinDate && a.Schedule.ScheduleFor == EntityType.Beneficiary).OrderByDescending(a => a.DataCollectionDate)
                                        .Select(a => a.Id).FirstOrDefault();

                indicatorRaws = await GetBeneficiaryIndicatorRawViews(model.InstanceId, previousInstanceId, model.BeneficiaryId);
            }
            indicators = GetBeneficairyWiseIndicators(indicatorRaws);

            var total = indicators.Count > 0 ? indicators[0].Indicators.Count : 0;
            return new PagedResponse<BeneficairyWiseIndicatorViewModel>(indicators, total, model.PageNo, model.PageSize);
        }

        private List<BeneficairyWiseIndicatorViewModel> GetBeneficairyWiseIndicators(List<IndicatorRawView> indicatorRawViews)
        {
            var beneficairyWiseIndicator = indicatorRawViews.GroupBy(a => new { a.BeneficiaryId, a.BeneficairyName })
                                            .Select(a => new BeneficairyWiseIndicatorViewModel
                                            {
                                                InstanceId = a.Select(b => b.InstanceId).FirstOrDefault(),
                                                BeneficiaryId = a.Key.BeneficiaryId,
                                                BeneficairyName = a.Key.BeneficairyName,
                                                Indicators = a.GroupBy(b => new { b.EntityColumnId, b.EntityColumnName })
                                                            .Select(c => new IndicatorGetViewModel
                                                            {

                                                                EntityDynamicColumnId = c.Key.EntityColumnId,
                                                                ColumnName = c.Key.EntityColumnName,
                                                                ColumnNameInBangla = c.Select(d => d.EntityColumnNameInBangla).FirstOrDefault(),
                                                                IsMultiValued = c.Select(d => d.IsMultiValued).FirstOrDefault() ?? false,
                                                                ColumnOrder = c.Select(d => d.ColumnOrder).FirstOrDefault(),
                                                                ColumnDataType = c.Select(d => d.DataType).FirstOrDefault(),
                                                                DataCollectionDate = c.Select(d => d.DataCollectionDate).OrderByDescending(d => d).FirstOrDefault(),
                                                                ListObjectId = c.Select(n => n.ColumnListId).FirstOrDefault(),
                                                                ListObjectName = c.Select(n => n.ColumnListName).FirstOrDefault(),
                                                                ListItems = c.GroupBy(d => d.ListItemId)
                                                                .Select(o => new ListItemViewModel
                                                                {
                                                                    Id = o.Key,
                                                                    Title = o.Select(o => o.ListItemTitle).FirstOrDefault(),
                                                                    Value = o.Select(o => o.ListItemValue).FirstOrDefault()
                                                                }).ToList(),
                                                                Values = //c.Where(n=>n.CollectionStatus==CollectionStatus.Approved)
                                                                c
                                                                //.GroupBy(n => n.PropertiesId)
                                                                .Select(o => o.PropertiesValue).ToList()
                                                                .Where(o => !string.IsNullOrEmpty(o)).ToList()

                                                            }).OrderBy(a=>a.ColumnOrder).ToList()

                                            }).ToList();
            return beneficairyWiseIndicator;
        }
        private List<FacilityWiseIndicatorViewModel> GetFacilityWiseIndicators(List<IndicatorRawView> indicatorRawViews)
        {
            var facilityWiseIndicator = indicatorRawViews.GroupBy(a => new { a.FacilityId, a.FacilityName })
                                            .Select(a => new FacilityWiseIndicatorViewModel
                                            {
                                                InstanceId = a.Select(b => b.InstanceId).FirstOrDefault(),
                                                FacilityId = a.Key.FacilityId,
                                                FacilityName = a.Key.FacilityName,
                                                Indicators = a.GroupBy(b => new { b.EntityColumnId, b.EntityColumnName })
                                                            .Select(c => new IndicatorGetViewModel
                                                            {

                                                                EntityDynamicColumnId = c.Key.EntityColumnId,
                                                                ColumnName = c.Key.EntityColumnName,
                                                                ColumnNameInBangla = c.Select(d => d.EntityColumnNameInBangla).FirstOrDefault(),
                                                                IsMultiValued = c.Select(d => d.IsMultiValued).FirstOrDefault() ?? false,
                                                                ColumnOrder = c.Select(d => d.ColumnOrder).FirstOrDefault(),
                                                                ColumnDataType = c.Select(d => d.DataType).FirstOrDefault(),
                                                                DataCollectionDate = c.Select(d => d.DataCollectionDate).OrderByDescending(d => d).FirstOrDefault(),
                                                                ListObjectId = c.Select(n => n.ColumnListId).FirstOrDefault(),
                                                                ListObjectName = c.Select(n => n.ColumnListName).FirstOrDefault(),
                                                                ListItems = c.GroupBy(d => d.ListItemId)
                                                                .Select(o => new ListItemViewModel
                                                                {
                                                                    Id = o.Key,
                                                                    Title = o.Select(o => o.ListItemTitle).FirstOrDefault(),
                                                                    Value = o.Select(o => o.ListItemValue).FirstOrDefault()
                                                                }).ToList(),
                                                                Values = //c.Where(n=>n.CollectionStatus==CollectionStatus.Approved)
                                                                c.Select(o => o.PropertiesValue).ToList()
                                                                .Where(o => !string.IsNullOrEmpty(o)).ToList()

                                                            }).OrderBy(a=>a.ColumnOrder).ToList()

                                            }).ToList();
            return facilityWiseIndicator;
        }

        private async Task<List<IndicatorRawView>> GetFacilityIndicatorRawViews(long indicatorInstanceId, long dynamicCellInstanceId, long id)
        {
            //var indicatorRaws = await (from fdcs in _context.FacilityDataCollectionStatuses
            //                           join f in _context.FacilityView on fdcs.FacilityId equals f.Id
            //                           join ii in _context.InstanceIndicators on fdcs.InstanceId equals ii.InstanceId
            //                           join si in _context.ScheduleInstances on ii.InstanceId equals si.Id
            //                           join edc in _context.EntityDynamicColumn on ii.EntityDynamicColumnId equals edc.Id
            //                           join lo in _context.ListDataType on edc.ColumnListId equals lo.Id into def
            //                           from listObject in def.DefaultIfEmpty()
            //                           join li in _context.ListItems on listObject.Id equals li.ColumnListId into def1
            //                           from listItem in def1.DefaultIfEmpty()
            //                           join bdc in _context.FacilityDynamicCells 
            //                               on new { FacilityId = f.Id, ii.EntityDynamicColumnId, InstanceId = dynamicCellInstanceId } 
            //                               equals new { bdc.FacilityId, bdc.EntityDynamicColumnId, bdc.InstanceId } into def2
            //                           from dynamicCell in def2.DefaultIfEmpty()
            //                           where (ii.InstanceId == indicatorInstanceId && f.Id == id)
            //                           select new IndicatorRawView
            //                           {
            //                               FacilityId = f.Id,
            //                               FacilityName = f.Name,
            //                               InstanceId = si.Id,

            //                               DataCollectionDate = si.DataCollectionDate,
            //                               DataCollectionStatusId = fdcs.Id,
            //                               CollectionStatus = fdcs.Status,
            //                               PropertiesId = dynamicCell.Id,
            //                               PropertiesValue = dynamicCell.Value,
            //                               EntityColumnId = edc.Id,
            //                               EntityColumnName = edc.Name,
            //                               ColumnOrder = ii.ColumnOrder,
            //                               EntityColumnNameInBangla = edc.NameInBangla,
            //                               IsMultiValued = edc.IsMultiValued,
            //                               DataType = edc.ColumnType,
            //                               ColumnListId = edc.ColumnListId,
            //                               ColumnListName = listObject.Name,
            //                               ListItemId = listItem.Id,
            //                               ListItemTitle = listItem.Title,
            //                               ListItemValue = listItem.Value

            //                           }).ToListAsync();
            //return indicatorRaws;
            throw new NotImplementedException();
        }


        public async Task<PagedResponse<FacilityWiseIndicatorViewModel>> GetFacilityIndicators(IndicatorByFacilityWiseQueryModel model)
        {
            var isDataExistInCurrentInstance = await _context.FacilityDynamicCells
               .AnyAsync(a => a.FacilityId == model.FacilityId && a.InstanceId == model.InstanceId && a.Status != CollectionStatus.NotCollected);
            List<IndicatorRawView> indicatorRaws = new List<IndicatorRawView>();
            List<FacilityWiseIndicatorViewModel> indicators = new List<FacilityWiseIndicatorViewModel>();

            if (isDataExistInCurrentInstance)
            {
                indicatorRaws = await GetFacilityIndicatorRawViews(model.InstanceId, model.InstanceId, model.FacilityId);
            }
            else
            {
                var presentInstanceCollectinDate = _context.ScheduleInstances.Where(a => a.Id == model.InstanceId).Select(a => a.DataCollectionDate).FirstOrDefault();
                var previousInstanceId = _context.ScheduleInstances.Where(a => a.DataCollectionDate < presentInstanceCollectinDate && a.Schedule.ScheduleFor == EntityType.Facility).OrderByDescending(a => a.DataCollectionDate)
                                        .Select(a => a.Id).FirstOrDefault();

                indicatorRaws = await GetFacilityIndicatorRawViews(model.InstanceId, previousInstanceId, model.FacilityId);
            }
            indicators = GetFacilityWiseIndicators(indicatorRaws);
            
            var total = indicators.Count > 0 ? indicators[0].Indicators.Count : 0;
            return new PagedResponse<FacilityWiseIndicatorViewModel>(indicators, total, model.PageNo, model.PageSize);
        }


        public async Task AddInstanceIndicator(Instance instance)
        {
            //var entityType = await _context.ScheduleInstances.Where(a => a.Id == instanceId).Select(a => a.Schedule.ScheduleFor).FirstOrDefaultAsync();
            var previousInstanceId = await _context.InstanceIndicators
                .Where(a => a.Instance.DataCollectionDate < instance.DataCollectionDate && a.Instance.Schedule.ScheduleFor == instance.Schedule.ScheduleFor)
                .OrderByDescending(a => a.Instance.DataCollectionDate).Select(a => a.InstanceId).FirstOrDefaultAsync();

            var instanceIndicator = await _context.InstanceIndicators.Where(a => a.InstanceId == previousInstanceId)
                .Select(a => new InstanceIndicator
                {
                    InstanceId = instance.Id,
                    EntityDynamicColumnId = a.EntityDynamicColumnId,
                    ColumnOrder = a.ColumnOrder
                }).ToListAsync();


            _context.AddRange(instanceIndicator);
            await _context.SaveChangesAsync();
        }

        public async Task<List<IndicatorSelectViewModel>> GetAutoCalculatedIndicator(EntityType entityType)
        {
            var data = await _context.EntityDynamicColumn
                .Where(a => a.IsAutoCalculated ?? false && a.EntityTypeId == entityType)
                .Select(a => new IndicatorSelectViewModel
                {
                    Id = a.Id,
                    EntityDynamicColumnId = a.Id,
                    IndicatorName = a.Name,
                    IndicatorNameInBangla = a.NameInBangla,
                    ColumnDataType = a.ColumnType,
                    IsMultivalued = a.IsMultiValued,
                    ListObject = a.ColumnList,
                    ListItems = a.ColumnList.ListItems,
                    ColumnOrder = int.MaxValue
                }).ToListAsync();
            return data;
        }
    }
}
