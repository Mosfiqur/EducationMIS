using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Service.Report;
using System;
using UnicefEducationMIS.Data.Factory;
using Indicator = UnicefEducationMIS.Core.Models.InstanceIndicator;
using UnicefEducationMIS.Core.Exceptions;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private const string noInactiveRecordFound = "No inactivated record found.";
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IScheduleService _scheduleService;

        private readonly IMapper _mapper;
        private readonly IBeneficiaryImporter _importer;
        private readonly IBeneficiaryVersionImporter _beneficiaryVersionImporter;
        private readonly IBeneficiaryDynamicCellRepository _beneficiaryDynamicCellRepository;
        private readonly IEnvironment _env;
        private readonly ICurrentLoginUserService _userService;
        private readonly IIndicatorRepository _indicatorRepository;
        private readonly IBeneficiaryVersionDataImportFileBuilder _versionDataImportFileBuilder;
        private readonly IBeneficiaryExporter _beneficiaryExporter;

        private readonly IBeneficiaryDataCollectionRepository _beneficiaryDataCollectionRepository;
        private readonly IScheduleInstanceRepository _scheduleInstanceRepository;
        private readonly IBeneficiaryDataCollectionStatusRepository _beneficiaryDataCollectionStatusRepository;
        private readonly IModelToIndicatorConverter _modelToIndicatorConverter;
        public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository,
            IScheduleService scheduleService, IMapper mapper, IBeneficiaryImporter importer,
            IBeneficiaryVersionImporter beneficiaryVersionImporter,
            IBeneficiaryDynamicCellRepository beneficiaryDynamicCellRepository,
            IEnvironment env,
            ICurrentLoginUserService userService,
            IIndicatorRepository indicatorRepository,
            IBeneficiaryVersionDataImportFileBuilder versionDataImportFileBuilder,
            IBeneficiaryExporter beneficiaryExporter,
            ICampRepository campRepository,
            IBlockRepository blockRepository, ISubBlockRepository subBlockRepository
            , IFacilityRepository facilityRepository
            , IEducationSectorPartnerRepository educationSectorPartnerRepository
            , IBeneficiaryDataCollectionRepository beneficiaryDataCollectionRepository
            , IScheduleInstanceRepository scheduleInstanceRepository
            , IBeneficiaryDataCollectionStatusRepository beneficiaryDataCollectionStatusRepository, IModelToIndicatorConverter modelToIndicatorConverter)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _scheduleService = scheduleService;
            _mapper = mapper;
            _importer = importer;
            _beneficiaryVersionImporter = beneficiaryVersionImporter;
            _beneficiaryDynamicCellRepository = beneficiaryDynamicCellRepository;
            _env = env;
            _userService = userService;
            _indicatorRepository = indicatorRepository;
            _versionDataImportFileBuilder = versionDataImportFileBuilder;
            _beneficiaryExporter = beneficiaryExporter;
            _beneficiaryDataCollectionRepository = beneficiaryDataCollectionRepository;
            _scheduleInstanceRepository = scheduleInstanceRepository;
            _beneficiaryDataCollectionStatusRepository = beneficiaryDataCollectionStatusRepository;
            _modelToIndicatorConverter = modelToIndicatorConverter;
        }
        private BeneficiaryDynamicCellAddViewModel GetBeneficiaryDynamicCell(BeneficiaryAddViewModel beneficiary)
        {
            var beneficiaryDynamicCell = new BeneficiaryDynamicCellAddViewModel();
            beneficiaryDynamicCell.BeneficiaryId = beneficiary.Id;
            beneficiaryDynamicCell.InstanceId = beneficiary.InstanceId;
            if (!string.IsNullOrEmpty(beneficiary.UnhcrId))
                beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
                {
                    EntityDynamicColumnId = BeneficairyFixedColumns.UnhcrId,
                    Value = new List<string>() { beneficiary.UnhcrId ?? "" }
                });
            if (!string.IsNullOrEmpty(beneficiary.Name))
                beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
                {
                    EntityDynamicColumnId = BeneficairyFixedColumns.Name,
                    Value = new List<string>() { beneficiary.Name ?? "" }
                });
            if (!string.IsNullOrEmpty(beneficiary.FatherName))
                beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
                {
                    EntityDynamicColumnId = BeneficairyFixedColumns.FatherName,
                    Value = new List<string>() { beneficiary.FatherName ?? "" }
                });
            if (!string.IsNullOrEmpty(beneficiary.MotherName))
                beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
                {
                    EntityDynamicColumnId = BeneficairyFixedColumns.MotherName,
                    Value = new List<string>() { beneficiary.MotherName ?? "" }
                });
            if (!string.IsNullOrEmpty(beneficiary.FCNId))
                beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
                {
                    EntityDynamicColumnId = BeneficairyFixedColumns.FCNId,
                    Value = new List<string>() { beneficiary.FCNId ?? "" }
                });
            beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
            {
                EntityDynamicColumnId = BeneficairyFixedColumns.DateOfBirth,
                Value = new List<string>() { beneficiary.DateOfBirth.ToString("dd-MMM-yyyy") }
            });
            beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
            {
                EntityDynamicColumnId = BeneficairyFixedColumns.Sex,
                Value = new List<string>() { ((int)beneficiary.Sex).ToString() }
            });
            beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
            {
                EntityDynamicColumnId = BeneficairyFixedColumns.Disabled,
                Value = new List<string>() { beneficiary.Disabled ? "Yes" : "No" }
            });
            beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
            {
                EntityDynamicColumnId = BeneficairyFixedColumns.LevelOfStudy,
                Value = new List<string>() { beneficiary.LevelOfStudy }
            });
            beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
            {
                EntityDynamicColumnId = BeneficairyFixedColumns.EnrollmentDate,
                Value = new List<string>() { beneficiary.EnrollmentDate.ToString("dd-MMM-yyyy") }
            });
            beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
            {
                EntityDynamicColumnId = BeneficairyFixedColumns.FacilityId,
                Value = new List<string>() { beneficiary.FacilityId.ToString() }
            });
            beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
            {
                EntityDynamicColumnId = BeneficairyFixedColumns.CampId,
                Value = new List<string>() { beneficiary.BeneficiaryCampId.ToString() }
            });
            beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
            {
                EntityDynamicColumnId = BeneficairyFixedColumns.BlockId,
                Value = new List<string>() { beneficiary.BlockId.ToString() }
            });
            beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
            {
                EntityDynamicColumnId = BeneficairyFixedColumns.SubBlockId,
                Value = new List<string>() { beneficiary.SubBlockId.ToString() }
            });
            beneficiaryDynamicCell.DynamicCells.Add(new DynamicCellViewModel()
            {
                EntityDynamicColumnId = BeneficairyFixedColumns.Remarks,
                Value = new List<string>() { beneficiary.Remarks }
            });

            return beneficiaryDynamicCell;

        }
        public async Task<BeneficiaryAddViewModel> Add(BeneficiaryAddViewModel beneficiary)
        {
            //TODO: beneficiary new schema
            var isRunningInstance = await _scheduleInstanceRepository.IsRunningInstance(beneficiary.InstanceId);
            if (!isRunningInstance)
            {
                throw new DomainException(Messages.InstanceNotRunning);
            }

            var ben = _mapper.Map<Beneficiary>(beneficiary);
            var beneficiaryDataCollectionStatus = _mapper.Map<BeneficiaryDataCollectionStatus>(beneficiary);
            beneficiaryDataCollectionStatus.Status = CollectionStatus.NotCollected;
            beneficiaryDataCollectionStatus.InstanceId = beneficiary.InstanceId;
            ben.BeneciaryDataCollectionStatuses.Add(beneficiaryDataCollectionStatus);

            try
            {
                var newBeneficiary = await _beneficiaryRepository.Add(ben);
                beneficiary.Id = newBeneficiary.Id;

                await _beneficiaryDynamicCellRepository.Save(GetBeneficiaryDynamicCell(beneficiary));
            }
            catch (Exception ex)
            {
                await _beneficiaryDynamicCellRepository.Revert(GetBeneficiaryDynamicCell(beneficiary));
                await _beneficiaryRepository.Revert(beneficiary.Id);

                beneficiary = null;
            }

            return beneficiary;


        }
        public async Task Update(BeneficiaryAddViewModel beneficiary)
        {
            
            var isRunningInstance = await _scheduleInstanceRepository.IsRunningInstance(beneficiary.InstanceId);
            if (!isRunningInstance)
            {
                throw new DomainException(Messages.InstanceNotRunning);
            }
            await _beneficiaryDynamicCellRepository.Save(GetBeneficiaryDynamicCell(beneficiary));
        }
        
        public async Task ActivateBeneficiary(BeneficiaryStatusChangeViewModel model)
        {
            var isRunningInstance =await _scheduleInstanceRepository.IsRunningInstance(model.InstanceId);
            if (!isRunningInstance)
            {
                throw new DomainException(Messages.InstanceNotRunning);
            }
            
            var data = await _beneficiaryDataCollectionStatusRepository.GetAll()
                             .Where(a => model.BeneficiaryIds.Contains(a.BeneficiaryId)
                             && a.Status==CollectionStatus.Inactivated
                             ).ToListAsync();
            if (data.Count == 0)
            {
                throw new RecordNotFound(noInactiveRecordFound);
            }
            data.ForEach(a =>
            {
                a.Status = CollectionStatus.NotCollected;
            });

            await _beneficiaryDataCollectionStatusRepository.UpdateRange(data);
        }
        public async Task InactivateBeneficiary(BeneficiaryStatusChangeViewModel model)
        {
            var isRunningInstance = await _scheduleInstanceRepository.IsRunningInstance(model.InstanceId);
            if (!isRunningInstance)
            {
                throw new DomainException(Messages.InstanceNotRunning);
            }

            var data = await _beneficiaryDataCollectionStatusRepository.GetAll()
                             .Where(a => model.BeneficiaryIds.Contains(a.BeneficiaryId)
                             && a.Status != CollectionStatus.Inactivated
                             ).ToListAsync();
            if (data.Count == 0)
            {
                throw new RecordNotFound("No record found to inactivate.");
            }
            data.ForEach(a =>
            {
                a.Status = CollectionStatus.Requested_Inactive;
            });

            await _beneficiaryDataCollectionStatusRepository.UpdateRange(data);
        }

        public async Task Delete(BeneficiaryStatusChangeViewModel model)
        {
            var isRunningInstance = await _scheduleInstanceRepository.IsRunningInstance(model.InstanceId);
            if (!isRunningInstance)
            {
                throw new DomainException(Messages.InstanceNotRunning);
            }

            var data = await _beneficiaryDataCollectionStatusRepository.GetAll()
                             .Where(a => model.BeneficiaryIds.Contains(a.BeneficiaryId)).ToListAsync();
            data.ForEach(a =>
            {
                a.Status = CollectionStatus.Deleted;
            });

            await _beneficiaryDataCollectionStatusRepository.UpdateRange(data);
        }
        
        public async Task<PagedResponse<BeneficiaryViewModel>> GetByFacilityId(BeneficiaryByFacilityIdQueryModel queryModel)
        {
            var facilityInstanceId = await _scheduleService.GetMappingFacilityInstanceId(queryModel.InstanceId);
            return await _beneficiaryRepository.GetByFacilityId(queryModel, facilityInstanceId);
        }

        public async Task<PagedResponse<BeneficiaryViewModel>> GetAllByViewId(BeneficiaryByViewIdQueryModel beneficiaryByViewId)
        {
            var facilityInstanceId = await _scheduleService.GetMappingFacilityInstanceId(beneficiaryByViewId.InstanceId);
            return await _beneficiaryRepository.GetAllByViewId(beneficiaryByViewId, facilityInstanceId);
        }
        public async Task<PagedResponse<BeneficiaryViewModel>> GetAllByInstanceId(BeneficiaryByViewIdQueryModel beneficiaryByViewId)
        {

            var facilityInstanceId = await _scheduleService.GetMappingFacilityInstanceId(beneficiaryByViewId.InstanceId);
            return await _beneficiaryRepository.GetAllByInstanceId(beneficiaryByViewId, facilityInstanceId);
        }

        public async Task<BeneficiaryEditViewModel> GetById(long id, long instanceId)
        {
            return await _beneficiaryRepository.GetById(id, instanceId);
        }

       

       
        public async Task<ImportResult<BeneficiaryVersionDataViewModel>> ImportBeneficiariesVersionData(Stream ms, long instanceId)
        {

            var result = await _beneficiaryVersionImporter.Import(ms, instanceId);
            if (result.ImportedObjects.Count > 0)
            {
                var newInserts = _beneficiaryVersionImporter.NewInserts();
                if (newInserts.Count > 0)
                {
                    await _beneficiaryRepository.InsertRange(newInserts.Select(id => new Beneficiary() { Id = id }));
                    await _beneficiaryRepository.StartCollectionForSelectedBeneficiaries(instanceId, newInserts);
                }

                var canApprove = _userService.GetClaims().Any(a => a.Value == AppPermissions.ApproveFacilityData);
                var collectionStatus = canApprove ? CollectionStatus.Approved : CollectionStatus.Collected;

                var toBeSaved = result.ImportedObjects.Select(item => new BeneficiaryDynamicCellAddViewModel
                {
                    BeneficiaryId = item.BeneficiaryId,
                    DynamicCells = item.DynamicCells,
                    InstanceId = instanceId
                }).ToList();





                await _beneficiaryDynamicCellRepository.ImportVersionData(toBeSaved, collectionStatus, instanceId);
            }

            result.TotalImported = result.ImportedObjects.Count;
            result.ImportedObjects = null;
            return result;
        }

        public string GetImportTemplatePath()
        {
            return Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName,
                FileNames.BeneficiaryImportTemplateName);
        }

        public async Task<byte[]> GetVersionDataImportTemplate(long instanceId)
        {
            var model = new IndicatorsByInstanceQueryModel { InstanceId = instanceId };
            var pagedResponse = await _indicatorRepository.GetIndicatorsByInstance(model);
            var indicators = pagedResponse.Data.ToList();

            var maxFacilityInstanceId = await _scheduleService.GetMappingFacilityInstanceId(instanceId);
            var allBeneficiaries = await _beneficiaryRepository
                .GetAllBeneficiariesForVersionDataTemplate(instanceId, maxFacilityInstanceId);
            var buildFile = await _versionDataImportFileBuilder.BuildFile(indicators, allBeneficiaries);
            return buildFile;
        }

        public async Task<byte[]> ExportBeneficiaries(long instanceId)
        {

            var pagedResponse = await _indicatorRepository.GetIndicatorsByInstance(IndicatorsByInstanceQueryModel.GetAllQuery(instanceId));
            var indicators = pagedResponse.Data.ToList();

            var maxFacilityInstanceId = await _scheduleService.GetMappingFacilityInstanceId(instanceId);

            var beneficiaries = _beneficiaryRepository.GetBeneficiaryByInstance(instanceId, maxFacilityInstanceId);
            var result = _beneficiaryRepository.SetBeneficiaryViewModel(beneficiaries.ToList());

            _modelToIndicatorConverter.ReplaceBeneficiaryFixedIndicatorIdsWithValues(result);
            return await _beneficiaryExporter.ExportAll(result, indicators);
        }

        public async Task<byte[]> ExportAggBeneficiaries(BeneficiaryAggExportQueryModel model)
        {
            var allIndicators =
                await _indicatorRepository.GetIndicatorsByInstances(model.InstanceIds);

            var distinctIndicators = allIndicators.GroupBy(x => x.EntityDynamicColumnId)
                .Select(g => g.First())
                .ToList();
            //var maxFacilityInstanceId = await _scheduleService.GetMaxFacilityInstanceId();
            //var beneficiaries = _beneficiaryRepository.GetBeneficiaryByInstances(model.InstanceIds, maxFacilityInstanceId);
            var beneficiaries =await _beneficiaryRepository.GetAggregateBeneficiary(model.InstanceIds); 
            var result = _beneficiaryRepository.SetBeneficiaryViewModel(beneficiaries.ToList());

            _modelToIndicatorConverter.ReplaceBeneficiaryFixedIndicatorIdsWithValues(result);
            return await _beneficiaryExporter.ExportAggBeneficiaries(result, distinctIndicators);
        }
    }
}
