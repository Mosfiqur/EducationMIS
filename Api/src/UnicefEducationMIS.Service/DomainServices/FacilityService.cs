using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.Import;
using UnicefEducationMIS.Service.Import;
using UnicefEducationMIS.Service.Report;
using UnicefEducationMIS.Core.ApplicationServices;
using System.Linq.Expressions;
using DocumentFormat.OpenXml.Spreadsheet;
using Org.BouncyCastle.Crypto.Digests;
using Renci.SshNet.Messages.Connection;
using UnicefEducationMIS.Core.Models.Common;
using UnicefEducationMIS.Core.Specifications;

namespace UnicefEducationMIS.Service.DomainServices
{
    // TODO: Refactor: 1. Split this service into several 
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        private readonly IScheduleService _scheduleService;
        private readonly ILogger<FacilityImportViewModel> _logger;
        private readonly ILogger<FacilityDynamicCellAddViewModel> _logger2;
        private readonly IIndicatorRepository _indicatorRepository;
        private readonly IFacilityDynamicCellService _facilityDynamicCellService;
        private readonly ICampRepository _campRepository;
        private readonly ICurrentLoginUserService _currentUserService;
        private readonly IFacilityDataCollectionRepository _facilityDataCollectionRepository;
        private readonly IScheduleInstanceRepository _scheduleInstanceRepository;
        private readonly INotificationService _notificationService;
        private readonly IFacilityExporter _facilityExporter;

        private readonly IModelToIndicatorConverter _modelToIndicatorConverter;
        private readonly IFacilityDataCollectionStatusRepository _facilityDataCollectionStatusRepository;
        private readonly IFacilityVersionDataImportFileBuilder _versionDataImportFileBuilder;


        private readonly IFacilityVersionImporter _facilityVersionImporter;
        public FacilityService(
            IFacilityRepository facilityRepository,
            IScheduleService scheduleService,
            IMapper mapper,
            ICampRepository campRepository,
            IIndicatorRepository indicatorRepository,
            IFacilityDynamicCellService facilityDynamicCellService,
            ILogger<FacilityImportViewModel> logger,
            ILogger<FacilityDynamicCellAddViewModel> logger2,
            IFacilityExporter facilityExporter, ICurrentLoginUserService currentUserService
            , IFacilityDataCollectionRepository facilityDataCollectionRepository
            , IScheduleInstanceRepository scheduleInstanceRepository
            , INotificationService notificationService,
            IModelToIndicatorConverter modelToIndicatorConverter,
            IFacilityDataCollectionStatusRepository facilityDataCollectionStatusRepository,
            IFacilityVersionDataImportFileBuilder versionDataImportFileBuilder,
            IBlockRepository blockRepository,
            IEducationSectorPartnerRepository espRepository,
            IUpazilaRepository upazilaRepository,
            IUnionRepository unionRepository,
            IUserService userService,
            IModelToIndicatorConverter modelToIndicator,
            IFacilityVersionImporter facilityVersionImporter)


        {
            _facilityRepository = facilityRepository;
            _mapper = mapper;
            _scheduleService = scheduleService;
            _campRepository = campRepository;
            _indicatorRepository = indicatorRepository;
            _facilityDynamicCellService = facilityDynamicCellService;
            _logger = logger;
            _logger2 = logger2;
            _currentUserService = currentUserService;
            _facilityDataCollectionRepository = facilityDataCollectionRepository;
            _scheduleInstanceRepository = scheduleInstanceRepository;
            _notificationService = notificationService;
            _modelToIndicatorConverter = modelToIndicatorConverter;
            _facilityDataCollectionStatusRepository = facilityDataCollectionStatusRepository;
            _versionDataImportFileBuilder = versionDataImportFileBuilder;
            _facilityVersionImporter = facilityVersionImporter;
            _facilityExporter = facilityExporter;
        }


        private async Task ValidateFacilityCoordinate(CreateFacilityViewModel facility)
        {
            var campCoordinates = await _campRepository.GetCoordinatesByCampId(facility.CampId ?? 0);
            var inside = campCoordinates.IsPointInside(facility.Position);
            if (!inside)
            {
                throw new InvalidCoordinateException();
            }
        }
        public async Task Add(CreateFacilityViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Latitude) && !string.IsNullOrEmpty(model.Longitude))
            {
                await ValidateFacilityCoordinate(model);
            }

            var facility = _mapper.Map<Facility>(model);

            var facilityDataCollectionStatus = _mapper.Map<FacilityDataCollectionStatus>(model);
            facilityDataCollectionStatus.Status = CollectionStatus.NotCollected;
            facilityDataCollectionStatus.InstanceId = model.InstanceId;
            facility.FacilityDataCollectionStatus.Add(facilityDataCollectionStatus);

            await _facilityRepository.Insert(facility);

            var dynamicCellAdd = new FacilityDynamicCellAddViewModel
            {
                FacilityId = facility.Id,
                InstanceId = model.InstanceId
            };

            model.FacilityCode = (long.Parse(await _facilityRepository.GetMaxFacilityCode()) + 1).ToString();
            dynamicCellAdd.DynamicCells.AddRange(_modelToIndicatorConverter.FacilityToDynamicCell(model));

            try
            {
                await _facilityDynamicCellService.Save(dynamicCellAdd);
            }
            catch (Exception)
            {
                await _facilityRepository.Delete(facility);
                throw;
            }
        }

        public async Task Import(CreateFacilityViewModel createFacilityViewModel)
        {

            var facility = _mapper.Map<Facility>(createFacilityViewModel);

            var runningInstances = await _scheduleService.GetRunningInstances(new ScheduleInstanceQueryModel() { ScheduleFor = EntityType.Facility });
            foreach (var item in runningInstances.Data)
            {
                var facilityDataCollectionStatus = _mapper.Map<FacilityDataCollectionStatus>(createFacilityViewModel);
                facilityDataCollectionStatus.Status = CollectionStatus.NotCollected;
                facilityDataCollectionStatus.InstanceId = item.Id;
                facility.FacilityDataCollectionStatus.Add(facilityDataCollectionStatus);
            }

            await _facilityRepository.Import(facility);
        }

        public async Task Delete(DeleteFacilityViewModel model)
        {
            var items = await
                _facilityDataCollectionStatusRepository.GetAll()
                    .Where(x => x.InstanceId == model.InstanceId && model.FacilityIds.Contains(x.FacilityId))
                    .ToListAsync();
            items.ForEach(x => x.Status = CollectionStatus.Deleted);
            await _facilityDataCollectionStatusRepository.UpdateRange(items);
        }

        public async Task Update(CreateFacilityViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Latitude) && !string.IsNullOrEmpty(model.Longitude))
            {
                await ValidateFacilityCoordinate(model);
            }

            var instance = await
                _scheduleInstanceRepository.ThrowIfNotFound(model.InstanceId);
            if (instance.Status == InstanceStatus.Completed)
            {
                throw new CompletedInstanceDataIsNotEditableException();
            }

            var facility = _mapper.Map<Facility>(model);

            await _facilityRepository.Update(facility);

            var dynamicCellAdd = new FacilityDynamicCellAddViewModel();
            dynamicCellAdd.FacilityId = facility.Id;
            dynamicCellAdd.InstanceId = model.InstanceId;
            dynamicCellAdd.DynamicCells.AddRange(_modelToIndicatorConverter.FacilityToDynamicCell(model));

            await _facilityDynamicCellService.Save(dynamicCellAdd);
        }
        public async Task<bool> IsFacilityCodeExist(string facilityCode)
        {
            var isExist = await _facilityRepository.IsFacilityCodeExist(facilityCode);
            return isExist;
        }
        //public Task StartCollectionForAllFacility(long instanceId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<PagedResponse<FacilityViewModel>> GetAllWithValue(FacilityQueryModel facilityQueryModel)
        {
            return await _facilityRepository.GetAllWithValue(facilityQueryModel);
        }

        public async Task<PagedResponse<FacilityViewModel>> GetAllByViewId(FacilityByViewIdQueryModel viewId)
        {
            return await _facilityRepository.GetAllByViewId(viewId);
        }
        public async Task<PagedResponse<FacilityViewModel>> GetAllByInstanceId(FacilityByViewIdQueryModel viewId)
        {
            return await _facilityRepository.GetAllByInstanceId(viewId);
        }
        private async Task<List<EntityWithPropertiesInfo>> GetFacilityProperties(long instanceId
            , List<long> ids, List<FacilityObjectViewModel> facilities
            , List<IndicatorSelectViewModel> instanceIndicator, bool isPreviousInstance)
        {
            var data = await _facilityDataCollectionRepository.GetAll()
                     .Where(a => a.InstanceId == instanceId
                     && ids.Contains(a.FacilityId)).ToListAsync();

            var joinData = (
                from f in facilities
                from i in instanceIndicator
                join dc in data on new { f.Id, i.EntityDynamicColumnId } equals new { Id = dc.FacilityId, dc.EntityDynamicColumnId } into def1
                from cData in def1.DefaultIfEmpty()
                where ids.Contains(f.Id)
                select new EntityWithPropertiesInfo
                {
                    EntityColumnId = i.EntityDynamicColumnId,
                    Properties = i.IndicatorName,
                    ColumnNameInBangla = i.IndicatorNameInBangla,
                    DataType = i.ColumnDataType,
                    ColumnOrder = i.ColumnOrder,
                    IsMultiValued = i.IsMultivalued ?? false,
                    ColumnListId = i.ListObject != null ? i.ListObject.Id : (long?)null,
                    ColumnListName = i.ListObject != null ? i.ListObject.Name : string.Empty,
                    Status = isPreviousInstance ? CollectionStatus.NotCollected :
                                cData != null && cData.Status.HasValue ? cData.Status : CollectionStatus.NotCollected,
                    ListItem = i.ListItems.Select(a => new ListItemViewModel
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Value = a.Value
                    }).ToList(),
                    EntityId = f.Id,
                    Value = cData != null ? cData.Value ?? "" : "",
                    InstanceId = cData != null ? cData.InstanceId : instanceId
                }).ToList();
            return joinData;
        }
        private List<FacilityViewModel> SetFacilityViewModels(List<EntityWithPropertiesInfo> propertiesInfos)
        {
            return propertiesInfos.GroupBy(a => a.EntityId)
            .Select(a => new FacilityViewModel
            {
                Id = a.Key,
                Properties = a.GroupBy(a => a.EntityColumnId)
                .Select(b => new PropertiesInfo
                {
                    EntityColumnId = b.Key,
                    Properties = b.Select(c => c.Properties).FirstOrDefault(),
                    ColumnNameInBangla = b.Select(c => c.ColumnNameInBangla).FirstOrDefault(),
                    DataType = b.Select(c => c.DataType).FirstOrDefault(),
                    ColumnOrder = b.Select(c => c.ColumnOrder).FirstOrDefault(),
                    IsMultiValued = (bool)b.Select(c => c.IsMultiValued).FirstOrDefault(),
                    ColumnListId = b.Select(c => c.ColumnListId).FirstOrDefault(),
                    ColumnListName = b.Select(c => c.ColumnListName).FirstOrDefault(),
                    Status = b.Select(c => c.Status).FirstOrDefault(),
                    ListItem = b.Select(c => c.ListItem).FirstOrDefault(),
                    Values = b.Where(c => !string.IsNullOrEmpty(c.Value)).Select(c => c.Value).ToList()
                }).ToList()
            }).ToList();

        }
        private List<FacilityViewModel> SetFacilityViewModels(List<FacilityObjectViewModel> facilities, List<FacilityViewModel> propertiesInfos)
        {
            var data = (from f in facilities
                        join p in propertiesInfos on f.Id equals p.Id into def1
                        from prop in def1.DefaultIfEmpty()

                        select new FacilityViewModel
                        {
                            Id = f.Id,
                            FacilityName = f.FacilityName,
                            FacilityCode = f.FacilityCode,
                            UpazilaId = f.UpazilaId,
                            UnionId = f.UnionId,
                            BlockId = f.BlockId,
                            CampId = f.CampId,
                            CampName = f.CampName,
                            Donors = f.Donors,
                            Latitude = f.Latitude,
                            longitude = f.longitude,
                            NonEducationPartner = f.NonEducationPartner,
                            ParaId = f.ParaId,
                            ParaName = f.ParaName,
                            FacilityStatus = f.FacilityStatus,
                            FacilityType = f.FacilityType,
                            TargetedPopulation = f.TargetedPopulation,
                            ProgrammingPartnerId = f.ProgrammingPartnerId,
                            ProgrammingPartnerName = f.ProgrammingPartnerName,
                            ImplemantationPartnerId = f.ImplemantationPartnerId,
                            ImplemantationPartnerName = f.ImplemantationPartnerName,
                            TeacherId = f.TeacherId,
                            CollectionStatus = f.CollectionStatus,
                            Properties = prop != null ? prop.Properties : new List<PropertiesInfo>()
                        }).ToList();

            return data;
        }

        public async Task<PagedResponse<FacilityViewModel>> GetAllForDevice(FacilityQueryModel queryModel)
        {
            return await _facilityRepository.GetAllForDevice(queryModel);
        }

        public async Task<PagedResponse<FacilityObjectViewModel>> GetAll(FacilityQueryModel facilityQuery)
        {
            return await _facilityRepository.GetAll(facilityQuery);
        }
        public async Task<PagedResponse<FacilityObjectViewModel>> GetAllByBeneficiaryInstance(FacilityQueryModel facilityQuery)
        {
            var facilityInstanceId = await _scheduleService.GetMappingFacilityInstanceId(facilityQuery.InstanceId);
            facilityQuery.InstanceId = facilityInstanceId;
            return await _facilityRepository.GetAll(facilityQuery);
        }

        public async Task<PagedResponse<FacilityObjectViewModel>> GetAllFilteredData(FacilityGetAllQueryModel facilityQuery)
        {
            return await _facilityRepository.GetAllFilteredData(facilityQuery);
        }

        public async Task<FacilityEditViewModel> GetById(long id, long instanceId)
        {
            return await _facilityRepository.GetById(id, instanceId);
        }
        public async Task AssignTeacher(AssignTeacherViewModel model)
        {

            model.FacilityIds.ForEach(async id =>
           {
               var dynamicCellAdd = new FacilityDynamicCellAddViewModel
               {
                   FacilityId = id,
                   InstanceId = model.InstanceId
               };
               dynamicCellAdd.DynamicCells.AddRange(_modelToIndicatorConverter.AssignTeacherToDynamicCell(model));
               await _facilityDynamicCellService.Save(dynamicCellAdd);
           });

            var users = new List<int>() { model.TeacherId };
            var notificationId = (int)NotificationTypeEnum.Assign_Teacher;
            List<Notification> notifications = new List<Notification>() { new Notification() {
                Data=$"[{string.Join(',', model.FacilityIds)}]",
                Details=$"{model.FacilityIds.Count} facilities has been assigned to you.",
                IsActed=false,
                IsDeleted=false,
                NotificationTypeId=notificationId,

            } };

            await _notificationService.Save(users, notifications);
        }

        public async Task<ImportResult<FacilityImportViewModel>> ImportFacilities(MemoryStream stream)
        {

            throw new NotImplementedException();
            //var blocks = _blockRepository.GetAll().ToList();
            //var camps = _campRepository.GetAll().ToList();
            //var espList = _espRepository.GetAll().ToList();

            //var upazilaList = await _upazilaRepository.All(x => true);
            //var unionList = await _unionRepository.All(x => true);

            //var dict = new Dictionary<string, long>();

            //var facilities = (await _facilityRepository.GetAll()
            //    .Select(x => new { x.FacilityCode, x.Id })
            //    .ToListAsync());

            //facilities.ForEach(x => dict.TryAdd(x.FacilityCode, x.Id));


            //AbstractWorkBookImporter<FacilityImportViewModel> importer =
            //    new FacilityImporter(
            //        _logger, 
            //        FacilityExcelColumns.All(),
            //        espList, 
            //        unionList.ToList(), 
            //        upazilaList.ToList(),
            //        camps,
            //        blocks,
            //        dict
            //        );
            //var result = await importer.Process(stream);

            //foreach (var item in result.ImportedObjects.Where(x => x.Id == 0))
            //{
            //    var mapped = _mapper.Map<CreateFacilityViewModel>(item);
            //    await Import(mapped);
            //}

            //foreach (var item in result.ImportedObjects.Where(x => x.Id > 0))
            //{
            //    var mapped = _mapper.Map<CreateFacilityViewModel>(item);
            //    await Update(mapped);
            //}

            //result.TotalImported = result.ImportedObjects.Count;
            //result.ImportedObjects = null;

            //return result;
        }

        public async Task<ImportResult<FacilityDynamicCellAddViewModel>> ImportVersionedFacilities(MemoryStream stream, long instanceId)
        {

            var importResult = await _facilityVersionImporter.Import(stream, instanceId);


            if (importResult.InvalidFile)
            {
                return importResult;
            }

            var newInserts = importResult.ImportedObjects.Where(x => x.IsNew).ToList();
            if (newInserts.Count > 0)
            {
                // Id generation is required because of StartCollectionForAllFacility
                var maxFacilityId = 0L;

                var maxFacilityCode = long.Parse(await _facilityRepository.GetMaxFacilityCode());
                if (await _facilityRepository.Count(x => true) > 0)
                {
                    maxFacilityId = _facilityRepository.GetAll().Max(x => x.Id);
                }

                newInserts.ForEach(x =>
                {
                    x.FacilityId = ++maxFacilityId;
                    if (!x.HasCode)
                    {
                        x.DynamicCells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Code, ++maxFacilityCode));
                    }
                });

                await _facilityRepository.InsertRange(newInserts.Select(x => new Facility { Id = x.FacilityId }));
                await _facilityRepository.StartCollectionForAllFacility(instanceId, newInserts.Select(x => x.FacilityId).ToList());
            }

            if (importResult.ImportedObjects.Count > 0)
            {
                bool canApprove = _currentUserService.GetClaims().Any(a => a.Value == AppPermissions.ApproveFacilityData);
                var status = canApprove ? CollectionStatus.Approved : CollectionStatus.Collected;
                await _facilityDynamicCellService.ImportVersionData(importResult.ImportedObjects, status, instanceId);
            }

            if (importResult.RowErrors.IsEmpty())
            {
                importResult.Succeed();
            }
            else
            {
                importResult.Fail();
            }
            return importResult;
        }

        public async Task<byte[]> GetVersionedDataImportTemplate(long instanceId)
        {
            var model = new IndicatorsByInstanceQueryModel { InstanceId = instanceId };
            var pagedResponse = await _indicatorRepository.GetIndicatorsByInstance(model);

            var autoCalculatedColumn = await _indicatorRepository.GetAutoCalculatedIndicator(EntityType.Facility);
            var indicators = pagedResponse.Data
                .ToList();
            indicators.AddRange(autoCalculatedColumn);

            var allFacilitiesRaw = _facilityRepository.GetFacilityByInstanceForImportTemplate(instanceId).ToList();
            var allFacilities = _facilityRepository.SetFacilityViewModel(allFacilitiesRaw);
            allFacilities.ForEach(item => _modelToIndicatorConverter.ReplaceFacilityFixedIndicatorIdsWithValues(item));
            var buildFile = await _versionDataImportFileBuilder.BuildFile(indicators, allFacilities);
            return buildFile;
        }

        public async Task<byte[]> ExportFacilities()
        {
            var response = await _facilityRepository.ExportAll(BaseQueryModel.All);
            var result = _mapper.Map<IEnumerable<FacilityViewModel>>(response.Data).ToList();
            return await _facilityExporter.ExportFacilities(result);
        }

        public async Task<byte[]> ExportVersionedFacilities(long instanceId)
        {
            var pagedResponse = await _indicatorRepository.GetIndicatorsByInstance(IndicatorsByInstanceQueryModel.GetAllQuery(instanceId));
            var indicators = pagedResponse.Data.ToList()
                .ToList();

            var list2 = _facilityRepository.GetFacilityByInstance(instanceId).ToList();

            var result = _facilityRepository.SetFacilityViewModel(list2);

            byte[] fileAsBytes = await _facilityExporter.ExportVersionedFacilities(result, indicators);
            return fileAsBytes;
        }

        public async Task<byte[]> ExportAggFacilities(FacilityAggExportQueryModel model)
        {
            var allIndicators =
            await _indicatorRepository.GetIndicatorsByInstances(model.InstanceIds);

            var distinctIndicators = allIndicators.GroupBy(x => x.EntityDynamicColumnId)
                .Select(g => g.First())
                .ToList();

            var list2 = _facilityRepository.GetFacilityByInstances(model.InstanceIds).ToList();
            var result = _facilityRepository.SetFacilityViewModel(list2);

            byte[] fileAsBytes = await _facilityExporter.ExportAggregatedFacilities(result.OrderBy(x => x.InstanceId).ThenBy(x => x.Id).ToList(), distinctIndicators.OrderBy(x => x.IndicatorName).ToList());
            return fileAsBytes;
        }

        public async Task<PagedResponse<FacilityObjectViewModel>> GetAllLatest(BaseQueryModel baseQueryModel)
        {
            var facilityInstanceId = await _scheduleService.GetMaxFacilityInstanceId();
            var facilityQueryModel = new FacilityQueryModel()
            {
                InstanceId = facilityInstanceId,
                PageNo = baseQueryModel.PageNo,
                PageSize = baseQueryModel.PageSize,
                SearchText = baseQueryModel.SearchText
            };
            return await _facilityRepository.GetAll(facilityQueryModel);
        }
    }
}
