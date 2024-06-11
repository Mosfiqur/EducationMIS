using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using Renci.SshNet.Messages;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.Import;

namespace UnicefEducationMIS.Service.Import
{
    public class FacilityVersionImporter : AbstractWorkBookImporter<FacilityDynamicCellAddViewModel>, IFacilityVersionImporter
    {

        private readonly IBlockRepository _blockRepository;
        private readonly IEducationSectorPartnerRepository _espRepository;
        private readonly IUpazilaRepository _upazilaRepository;
        private readonly IUnionRepository _unionRepository;
        private readonly IUserService _userService;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IModelToIndicatorConverter _converter;
        private readonly ICampRepository _campRepository;
        private readonly IIndicatorRepository _indicatorRepository;


        
        private  List<IndicatorSelectViewModel> _indicators;
        private  List<Facility> _existingFacilities;
        private  List<Camp> _camps;
        private  List<Block> _blocks;
        private  List<Union> _unionList;
        private  List<Upazila> _upazilaList;
        private  List<EducationSectorPartner> _espList;
        private  List<UserViewModel> _teachers;
        private  List<CampCoordinate> _campCoordinates;
        private long _instanceId;
        public FacilityVersionImporter(
            ILogger<FacilityDynamicCellAddViewModel> logger,
            IBlockRepository blockRepository,
            IEducationSectorPartnerRepository espRepository,
            IUpazilaRepository upazilaRepository,
            IUnionRepository unionRepository,
            IUserService userService,
            IFacilityRepository facilityRepository,
            IModelToIndicatorConverter converter,
            ICampRepository campRepository,
            IIndicatorRepository indicatorRepository) : base(logger)
        {
            _blockRepository = blockRepository;
            _espRepository = espRepository;
            _upazilaRepository = upazilaRepository;
            _unionRepository = unionRepository;
            _userService = userService;
            _facilityRepository = facilityRepository;
            _converter = converter;
            _campRepository = campRepository;
            _indicatorRepository = indicatorRepository;

            
            _indicators = new List<IndicatorSelectViewModel>();
            _existingFacilities = new List<Facility>();
            _camps = new List<Camp>();
            _blocks = new List<Block>();
            _unionList = new List<Union>();
            _upazilaList = new List<Upazila>();
            _espList = new List<EducationSectorPartner>();
            _teachers = new List<UserViewModel>();
            _campCoordinates = new List<CampCoordinate>();
        }


       
        public async Task<ImportResult<FacilityDynamicCellAddViewModel>> Import(Stream stream, long instanceId)
        {
            await InitializeWorkbook(stream);
            _instanceId = instanceId;
            await LoadData();
            return await Process();
        }

        private async Task LoadData()
        {
            await LoadExistingFacilities();
            var query = IndicatorsByInstanceQueryModel.GetAllQuery(_instanceId);
            var queryResult = await _indicatorRepository.GetIndicatorsByInstance(query);
            if (!queryResult.Data.Any())
            {
                throw new DomainException(Messages.NoInstanceIndicatorFound);
            }

            var mandatoryColumns = FacilityExcelColumns.VersionedMandatoryColumns();
            var toBeExcluded = new List<long>()
            {
                FacilityFixedIndicators.Code,
                FacilityFixedIndicators.Name
            };

            static bool IsMandatory(IndicatorSelectViewModel x) =>
                !FacilityFixedColumns.DamageRelatedIndicators.Contains(x.EntityDynamicColumnId) &&
                !FacilityFixedIndicators.Optional().Contains(x.EntityDynamicColumnId);

            var mandatoryIndicators = queryResult.Data
                .Where(x => !toBeExcluded.Contains(x.EntityDynamicColumnId))
                .Select(x => new FacilityExcelColumn(x.IndicatorName, x.IndicatorName, IsMandatory(x)))
                .ToList();

            mandatoryColumns.AddRange(mandatoryIndicators);
            var autoCalculatedColumn = await _indicatorRepository.GetAutoCalculatedIndicator(EntityType.Facility);

            mandatoryColumns.AddRange(autoCalculatedColumn.Select(x => new FacilityExcelColumn(x.IndicatorName, x.IndicatorName, false, true))
                .ToList());

            _indicators.AddRange(queryResult.Data);
            _indicators.AddRange(autoCalculatedColumn);
            _blocks = _blockRepository.GetAll().ToList();
            _camps = _campRepository.GetAll().ToList();
            _espList = _espRepository.GetAll().ToList();

            _upazilaList = (await _upazilaRepository.All(x => true)).ToList();
            _unionList = (await _unionRepository.All(x => true)).ToList();


            _teachers = (await _userService.GetTeachers(BaseQueryModel.All))
                .Data
                .ToList();

            _campCoordinates = await _campRepository.GetAllCoordinates();
            _allColumns.AddRange(mandatoryColumns);
        }
        private async Task LoadExistingFacilities()
        {

            var fac = GetWorksheetColumn(FacilityExcelColumns.FacilityIdIndex);
            var ids = new List<long>();
            fac.ForEach(x =>
            {
                var id = x.Value.ToString();
                if (string.IsNullOrEmpty(id)) return;
                if (long.TryParse(id, out var val))
                {
                    ids.Add(val);
                }
            });

            var facilities = await _facilityRepository.GetFacilityView()
                .Where(x => ids.Contains(x.Id))
                .Select(x => new Facility
                {
                    Id = x.Id,
                    FacilityCode = x.FacilityCode
                })
                .ToListAsync();
            _existingFacilities.AddRange(facilities);
        }
        public override async Task<bool> SetPropertyValue(ExcelCell column, FacilityDynamicCellAddViewModel entity)
        {
            entity.InstanceId = _instanceId;
            if (column.DisplayName == FacilityExcelColumns.Id.ToLower())
            {
                if (string.IsNullOrEmpty(column.Value))
                {
                    return entity.IsNew = true;
                }
                
                var isSuccess = long.TryParse(column.Value, out var id);
                if (!isSuccess || id == 0)
                {
                    return entity.IsNew = true;
                }

                var fac = _existingFacilities.FirstOrDefault(x => x.Id == id);
                if (fac == null)
                {
                    return false;
                }
                entity.FacilityId = fac.Id;
                //entity.DynamicCells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Code, fac.FacilityCode));
                return true;
            }

            var indicator = _indicators.SingleOrDefault(x => x.IndicatorName.ToLower().Trim() == column.DisplayName.Trim());
            if (indicator == null)
            {
                return false;
            }

            if (indicator.EntityDynamicColumnId == FacilityFixedIndicators.Code)
            {
                if (string.IsNullOrEmpty(column.Value))
                {
                    entity.HasCode = false;
                }
                else
                {
                    entity.DynamicCells.Add(new DynamicCellViewModel(FacilityFixedIndicators.Code, column.Value));
                    entity.HasCode = true;
                }
                return true;
            }

            if (indicator.EntityDynamicColumnId == FacilityFixedIndicators.Name ||
                indicator.EntityDynamicColumnId == FacilityFixedIndicators.Remarks ||
                indicator.EntityDynamicColumnId == FacilityFixedIndicators.Latitude ||
                indicator.EntityDynamicColumnId == FacilityFixedIndicators.Longitude ||
                indicator.EntityDynamicColumnId == FacilityFixedIndicators.Donors ||
                indicator.EntityDynamicColumnId == FacilityFixedIndicators.NonEducationPartner ||
                indicator.EntityDynamicColumnId == FacilityFixedIndicators.ParaName
            )
            {
                entity.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, column.Value));
                return true;
            }

            if (indicator.EntityDynamicColumnId == FacilityFixedIndicators.Upazila)
            {
                var upazila = _upazilaList.SingleOrDefault(x => x.Name == column.Value);
                if (upazila == null)
                {
                    return false;
                }
                entity.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, upazila.Id));
                return true;
            }
            if (indicator.EntityDynamicColumnId == FacilityFixedIndicators.Union)
            {
                var union = _unionList.SingleOrDefault(x => x.Name == column.Value);
                if (union == null)
                {
                    return false;
                }
                entity.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, union.Id));
                return true;
            }
           
            if (indicator.EntityDynamicColumnId == FacilityFixedIndicators.ProgramPartner)
            {
                var esp = _espList.SingleOrDefault(x => x.PartnerName == column.Value);
                if (esp == null)
                {
                    return false;
                }
                entity.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, esp.Id.ToString()));
                return true;
            }
            if (indicator.EntityDynamicColumnId == FacilityFixedIndicators.ImplementationPartner)
            {
                var esp = _espList.SingleOrDefault(x => x.PartnerName == column.Value);
                if (esp == null)
                {
                    return false;
                }
                entity.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, esp.Id.ToString()));
                return true;
            }
            if (indicator.EntityDynamicColumnId == FacilityFixedIndicators.Camp)
            {
                var camp = _camps.SingleOrDefault(x => x.SSID == column.Value);
                if (camp != null)
                {
                    entity.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, camp.Id));
                }
                return true;
            }
            if (indicator.EntityDynamicColumnId == FacilityFixedIndicators.Block)
            {
                var block = _blocks.SingleOrDefault(x => x.Code == column.Value);
                if (block != null)
                {
                    entity.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, block.Id));
                }
                return true;
            }

            if (indicator.EntityDynamicColumnId ==  FacilityFixedIndicators.Status)
            {
                if (string.IsNullOrEmpty(column.Value))
                {
                    return true;
                }
            }

            if (indicator.EntityDynamicColumnId == FacilityFixedIndicators.Type)
            {
                if (string.IsNullOrEmpty(column.Value))
                {
                    return true;
                }
            }

            if (indicator.EntityDynamicColumnId == FacilityFixedIndicators.Teacher)
            {
                var teacher = _teachers.SingleOrDefault(x => x.Email == column.Value);
                if (teacher != null)
                {
                    entity.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, teacher.Id ?? 0));
                }
                return true;
            }

            return HandleIndicator(indicator, column, entity);

        }

        private bool HandleIndicator(IndicatorSelectViewModel indicator, ExcelCell column, FacilityDynamicCellAddViewModel entity)
        {
            if (DynamicColumnValidationService.ValidateColumn(indicator, column.Value, out var checkedValue))
            {
                if (indicator.IsMultivalued.HasValue && indicator.IsMultivalued.Value)
                {
                    entity.DynamicCells.Add(
                        new DynamicCellViewModel(indicator.EntityDynamicColumnId, checkedValue.Split(',').ToList())
                    );
                }
                else
                {
                    entity.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, checkedValue));
                }
                return true;
            }
            return FacilityFixedIndicators.Optional().Contains(indicator.EntityDynamicColumnId);
        }

        public override async Task ApplyBusinessLogic(FacilityDynamicCellAddViewModel entity, int rowIndex)
        {
            var model = _converter.DynamicCellToCreateFacilityViewModels(entity);
            if (!string.IsNullOrEmpty(model.Latitude) && !string.IsNullOrEmpty(model.Longitude))
            {
                var inside = _campCoordinates.Where(x=> x.CampId == model.CampId)
                    .ToList()
                    .IsPointInside(model.Position);
                if (!inside)
                {
                    ImportResult.AddRowError(rowIndex, Messages.CoordindateIsOutsideOfCamp);
                    return;
                }
            }

            if (model.TargetedPopulation != TargetedPopulation.Host_Communities && (
                    model.CampId == 0 || !model.BlockId.HasValue || model.FacilityStatus.NoneMatched() || model.FacilityType.NoneMatched())
                )
            {
                ImportResult.AddRowError(rowIndex, Messages.InvalidCampBlockStatusOrType);
                return;
            }

            var union = _unionList.FirstOrDefault(x => x.Id == model.UnionId);
            if (union == null || union.UpazilaId != model.UpazilaId)
            {
                ImportResult.AddRowError(rowIndex, Messages.UnionNotInUpazilla);
                return;
            }

            if (model.TargetedPopulation != TargetedPopulation.Host_Communities)
            {
                var camp = _camps.FirstOrDefault(x => x.Id == model.CampId);
                
                if (camp == null || camp.UnionId != model.UnionId)
                {
                    ImportResult.AddRowError(rowIndex, Messages.CampNotInUnion);
                    return;
                }

                var block = _blocks.FirstOrDefault(x => x.CampId == model.CampId);
                if (block == null)
                {
                    ImportResult.AddRowError(rowIndex, Messages.BlockNotInCamp);
                    return;
                }
            }
            ImportResult.ImportedObjects.Add(entity);
        }
    }
}
