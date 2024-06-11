using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Import
{
    public class BeneficiaryVersionImporter : IBeneficiaryVersionImporter, IDisposable
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IIndicatorRepository _indicatorRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly ICampRepository _campRepository;
        private readonly ISubBlockRepository _subBlockRepository;
        private readonly IScheduleService _scheduleService;
        private readonly ILogger<BeneficiaryVersionImporter> _logger;

        private WorkbookAdapter _workbook;
        private WorksheetAdapter _worksheet;
        private ImportResult<BeneficiaryVersionDataViewModel> _importResult;
        private List<FacilityObjectViewModel> _allFacilities;
        private Dictionary<int, string> _columnIndexToColumnName;
        private List<IndicatorSelectViewModel> _indicators;
        private long _maxBeneficiaryId = 0L;
        private List<Beneficiary> _existingBeneficiaries;
        private List<long> _beneficiaryIdsFoundInExcel;
        private readonly List<long> _newBeneficiaryIds = new List<long>();

        private long _instanceId;

        private readonly List<Camp> _camps;
        private readonly List<Block> _blocks;
        private readonly List<SubBlock> _subBlocks;



        public BeneficiaryVersionImporter(IFacilityRepository facilityRepository,
            ILogger<BeneficiaryVersionImporter> logger, IIndicatorRepository indicatorRepository,
            IBeneficiaryRepository beneficiaryRepository, IBlockRepository blockRepository,
            ICampRepository campRepository, ISubBlockRepository subBlockRepository
            , IScheduleService scheduleService)
        {
            _facilityRepository = facilityRepository;
            _logger = logger;
            _allFacilities = new List<FacilityObjectViewModel>();
            _indicatorRepository = indicatorRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _blockRepository = blockRepository;
            _campRepository = campRepository;
            _subBlockRepository = subBlockRepository;
            _scheduleService = scheduleService;
            _indicators = new List<IndicatorSelectViewModel>();
            _existingBeneficiaries = new List<Beneficiary>();
            _beneficiaryIdsFoundInExcel = new List<long>();

            _camps = new List<Camp>();
            _blocks = new List<Block>();
            _subBlocks = new List<SubBlock>();
        }


        public async Task<ImportResult<BeneficiaryVersionDataViewModel>> Import(Stream stream, long instanceId)
        {
            _instanceId = instanceId;
            await Initialize(stream, instanceId);
            ReadColumns();
            if (_importResult.InvalidFile)
            {
                return _importResult;
            }

            ReadBeneficiaries();
            return _importResult;
        }



        private async Task Initialize(Stream stream, long instanceId)
        {
            try
            {
                _workbook = new WorkbookAdapter(stream);
                _worksheet = _workbook.GetFirstWorksheet();
                _importResult = new ImportResult<BeneficiaryVersionDataViewModel>();
                var facilityInstanceId = await _scheduleService.GetMappingFacilityInstanceId(instanceId);
                var pagedResponse = await _facilityRepository.GetAll(new BaseQueryModel(), facilityInstanceId);
                _allFacilities.AddRange(pagedResponse.Data);
                var indicatorResponse = await _indicatorRepository.GetIndicatorsByInstance(new IndicatorsByInstanceQueryModel() { InstanceId = instanceId });

                _indicators.AddRange(
                    indicatorResponse
                        .Data.Where(x => x.EntityDynamicColumnId != BeneficairyFixedColumns.FacilityId)
                );
                if (await _beneficiaryRepository.Count(x => true) > 0)
                {
                    _maxBeneficiaryId = _beneficiaryRepository.GetAll().Max(x => x.Id);
                }
                _logger.LogInformation("File opened successfully");
                LoadExistingBeneficiaries();

                _camps.AddRange(await _campRepository.GetAll().ToListAsync());
                _blocks.AddRange(await _blockRepository.GetAll().ToListAsync());
                _subBlocks.AddRange(await _subBlockRepository.GetAll().ToListAsync());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private void LoadExistingBeneficiaries()
        {
            var column = GetFirstColumnOfExcelFile();

            column.ForEach(x =>
            {
                var id = x.Value.ToString();
                if (string.IsNullOrEmpty(id)) return;
                if (long.TryParse(id, out var val))
                {
                    _beneficiaryIdsFoundInExcel.Add(val);
                }
            });

            var bens = _beneficiaryRepository
                .GetAll()
                .Where(x => _beneficiaryIdsFoundInExcel.Contains(x.Id))
                .ToList();

            _existingBeneficiaries.AddRange(bens);
        }

        private void ReadBeneficiaries()
        {
            var dataStartRowIndex = ExcelConstants.HeaderRowIndex + 1;
            for (int rowIndex = dataStartRowIndex; rowIndex <= _worksheet.MaxRowIndex; rowIndex++)
            {
                var row = _worksheet.ReadRow(rowIndex);
                if (row == null || row.IsBlankRow())
                {
                    continue;
                }

                BeneficiaryVersionDataViewModel beneficiary = ReadBeneficiary(row);
                if (beneficiary != null)
                {
                    beneficiary.InstanceId = _instanceId;
                    ApplyBusinessLogic(beneficiary, rowIndex);
                    //_importResult.ImportedObjects.Add(beneficiary);
                }
            }
        }

        private BeneficiaryVersionDataViewModel ReadBeneficiary(RowAdapter row)
        {
            var beneficiary = new BeneficiaryVersionDataViewModel();
            var isValidEntity = true;
            var enumerator = _columnIndexToColumnName.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var columnIndex = enumerator.Current;
                var columnName = _columnIndexToColumnName[columnIndex];
                var cell = row.GetCell(columnIndex);
                if (cell == null || cell.Value == null)
                {
                    if (columnName.Equals(BeneficiaryVersionDataColumnNames.Id,
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        beneficiary.BeneficiaryId = ++_maxBeneficiaryId;
                        _newBeneficiaryIds.Add(beneficiary.BeneficiaryId);
                        continue;
                    }
                    isValidEntity = false;
                    _importResult.RowErrors.Add(GetRowError(row.RowIndex, string.Format("Column {0} has no value", columnName)));
                }


                if (isValidEntity)
                {
                    isValidEntity = SetPropertyValue(row.RowIndex, columnName, cell.Value.ToStringValue().Trim(), beneficiary);
                    if (!isValidEntity)
                    {
                        _importResult.RowErrors.Add(GetRowError(row.RowIndex, string.Format("Column {0} has invalid value", columnName)));
                    }
                }
            }

            if (!isValidEntity)
            {
                return null;
            }

            return beneficiary;
        }

        private bool SetPropertyValue(int rowIndex, string propertyName, string propertyValue,
            BeneficiaryVersionDataViewModel beneficiary)
        {

            if (propertyName.Equals(BeneficiaryVersionDataColumnNames.FacilityName,
                StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            if (propertyName.Equals(BeneficiaryVersionDataColumnNames.FacilityId, StringComparison.InvariantCultureIgnoreCase))
            {
                var fac = _allFacilities.FirstOrDefault(x => String.Equals(x.FacilityCode, propertyValue, StringComparison.CurrentCultureIgnoreCase));
                if (fac == null)
                {
                    return false;
                }
                beneficiary.DynamicCells.Add(new DynamicCellViewModel(BeneficairyFixedColumns.FacilityId, fac.Id));
                return true;
            }

            if (propertyName.Equals(BeneficiaryVersionDataColumnNames.Id, StringComparison.InvariantCultureIgnoreCase))
            {
                var isSuccess = long.TryParse(propertyValue, out var beneficiaryId);
                if (!isSuccess || beneficiaryId == 0)
                {
                    beneficiary.BeneficiaryId = ++_maxBeneficiaryId;
                    _newBeneficiaryIds.Add(beneficiary.BeneficiaryId);
                    return true;
                }

                var existing = _existingBeneficiaries.FirstOrDefault(x => x.Id == beneficiaryId);
                if (existing == null)
                {
                    return false;
                }
                beneficiary.BeneficiaryId = beneficiaryId;
                return true;
            }

            var indicator = _indicators.SingleOrDefault(x => x.IndicatorName.ToLower().Trim() == propertyName);
            if (indicator == null)
            {
                return false;
            }

            if (indicator.EntityDynamicColumnId == BeneficairyFixedColumns.Sex)
            {
                var val = propertyValue.TryParseToEnum<Gender>(out var isSuccess);
                if (isSuccess)
                {
                    beneficiary.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, (int)val));
                    return true;
                }
                return false;
            }

            if (indicator.EntityDynamicColumnId == BeneficairyFixedColumns.CampId)
            {
                var camp = _camps.SingleOrDefault(x => x.SSID == propertyValue);
                if (camp == null)
                {
                    return false;
                }
                beneficiary.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, camp.Id));
                return true;
            }
            if (indicator.EntityDynamicColumnId == BeneficairyFixedColumns.BlockId)
            {
                var block = _blocks.SingleOrDefault(x => x.Code == propertyValue);
                if (block == null)
                {
                    return false;
                }
                beneficiary.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, block.Id));
                return true;
            }

            if (indicator.EntityDynamicColumnId == BeneficairyFixedColumns.SubBlockId)
            {
                var sblock = _subBlocks.SingleOrDefault(x => x.Name == propertyValue);
                if (sblock == null)
                {
                    return false;
                }
                beneficiary.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, sblock.Id));
                return true;
            }

            if (!DynamicColumnValidationService.ValidateColumn(indicator, propertyValue, out var checkedValue))
            {
                return false;
            }

            if (indicator.IsMultivalued.HasValue && indicator.IsMultivalued.Value)
            {
                beneficiary.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, checkedValue.ToMultiValue()));
            }
            else
            {
                beneficiary.DynamicCells.Add(new DynamicCellViewModel(indicator.EntityDynamicColumnId, checkedValue));
            }
            return true;
        }


        public List<CellAdapter> GetFirstColumnOfExcelFile()
        {
            List<CellAdapter> cells = new List<CellAdapter>();
            var dataStartRowIndex = ExcelConstants.HeaderRowIndex + 1;
            for (int rowIndex = dataStartRowIndex; rowIndex <= _worksheet.MaxRowIndex; rowIndex++)
            {
                var row = _worksheet.ReadRow(rowIndex);
                if (row == null || row.IsBlankRow())
                {
                    continue;
                }

                cells.Add(row.Cells[0]);
            }
            return cells;
        }


        private RowError GetRowError(int rowIndex, string errorMsg)
        {
            return new RowError
            {
                RowNumber = rowIndex,
                ErrorMessage = errorMsg
            };
        }

        private void ReadColumns()
        {
            var headerRow = _worksheet.ReadRow(ExcelConstants.HeaderRowIndex);
            if (headerRow.IsBlankRow())
            {
                _importResult.InvalidFile = true;
                return;
            }


            _columnIndexToColumnName = new Dictionary<int, string>();
            foreach (var headerCell in headerRow.Cells)
            {
                var columnHeader = headerCell.Value.ToStringValue();
                var columnIndex = headerCell.ColumnIndex;
                if (string.IsNullOrEmpty(columnHeader))
                {
                    break;
                }
                _columnIndexToColumnName.Add(columnIndex, columnHeader.ToLower());
            }

            // checking all columns exists 
            var columnNames = _columnIndexToColumnName.Values.ToList();
            foreach (var excelColumn in BeneficiaryVersionDataColumnNames.AllColumns)
            {
                if (!columnNames.Contains(excelColumn.ToLower()))
                {
                    _importResult.InvalidFile = true;
                    break;
                }
            }

            foreach (var indicatorColumn in _indicators)
            {
                if (!columnNames.Contains(indicatorColumn.IndicatorName.ToLower()))
                {
                    _importResult.InvalidFile = true;
                    break;
                }
            }
        }

        private void ApplyBusinessLogic(BeneficiaryVersionDataViewModel beneficiary, int rowIndex)
        {
            var campId =
                beneficiary.DynamicCells
                    .First(x => x.EntityDynamicColumnId == BeneficairyFixedColumns.CampId)
                    .ToIntId();

            var blockId = beneficiary.DynamicCells
                .First(x => x.EntityDynamicColumnId == BeneficairyFixedColumns.BlockId)
                .ToIntId();

            var block = _blocks.First(x => x.Id == blockId);
            if (block.CampId != campId)
            {
                _importResult.AddRowError(rowIndex, Messages.BlockNotInCamp);
                return;
            }

            var subBlockId = beneficiary.DynamicCells
                .First(x => x.EntityDynamicColumnId == BeneficairyFixedColumns.SubBlockId)
                .ToIntId();

            var subBlock = _subBlocks.First(x => x.Id == subBlockId);
            if (subBlock.BlockId != blockId)
            {
                _importResult.AddRowError(rowIndex, Messages.SubBlockNotInBlock);
                return;
            }
            _importResult.ImportedObjects.Add(beneficiary);
        }

        public List<long> NewInserts()
        {
            return _newBeneficiaryIds;
        }


        public void Dispose()
        {
            if (null != _workbook)
            {
                _workbook.Dispose();
                _logger.LogInformation("File closed successfully");
            }
        }



        private bool GetBoolean(string value)
        {
            if (value.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private Gender GetGender(string value)
        {
            if (value.Equals("Male", StringComparison.InvariantCultureIgnoreCase))
            {
                return Gender.Male;
            }

            return Gender.Female;
        }

    }
}
