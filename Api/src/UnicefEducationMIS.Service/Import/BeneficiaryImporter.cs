using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Import
{
    public class BeneficiaryImporter : IBeneficiaryImporter, IDisposable
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly ICampRepository _campRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly ILogger<BeneficiaryImporter> _logger;
        private WorkbookAdapter _workbook;
        private WorksheetAdapter _worksheet;
        private ImportResult<BeneficiaryAddViewModel> _importResult;
        private List<FacilityObjectViewModel> _allFacilities;
        private List<Camp> _allCamps;
        private Dictionary<int, string> _columnIndexToColumnName;
        private Dictionary<string, long> _existingBeneficiaries;

        public BeneficiaryImporter(IFacilityRepository facilityRepository, 
            ICampRepository campRepository,
            IBeneficiaryRepository beneficiaryRepository,
            ILogger<BeneficiaryImporter> logger)
        {
            _facilityRepository = facilityRepository;
            _campRepository = campRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _logger = logger;
            _allFacilities = new List<FacilityObjectViewModel>();
            _allCamps = new List<Camp>();
            _existingBeneficiaries = new Dictionary<string, long>();

        }

        public async Task<ImportResult<BeneficiaryAddViewModel>> Import(Stream stream)
        {
            await Initialize(stream);
            ReadColumns();
            if (_importResult.InvalidFile)
            {
                return _importResult;
            }

            ReadBeneficiaries();
            return _importResult;
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

                BeneficiaryAddViewModel beneficiary = ReadBeneficiary(row);
                if (beneficiary != null)
                {
                    _importResult.ImportedObjects.Add(beneficiary);
                }
            }
        }

        private BeneficiaryAddViewModel ReadBeneficiary(RowAdapter row)
        {
            var beneficiary = new BeneficiaryAddViewModel();
            var isValidEntity = true;
            var enumerator = _columnIndexToColumnName.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var columnIndex = enumerator.Current;
                var columnName = _columnIndexToColumnName[columnIndex];
                var cell = row.GetCell(columnIndex);
                if (cell == null || cell.Value == null)
                {
                    isValidEntity = false;
                    _importResult.RowErrors.Add(GetRowError(row.RowIndex, string.Format("Column {0} has no value", columnName)));
                }


                if (isValidEntity)
                {
                    isValidEntity = SetPropertyValue(columnName, cell.Value.ToStringValue().Trim(), beneficiary);
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

        private bool SetPropertyValue(string propertyName, string propertyValue, BeneficiaryAddViewModel beneficiary)
        {
            if (propertyName.Equals(BeneficiaryExcelColumnNames.UnhcrId.ToLower()))
            {
                beneficiary.UnhcrId = propertyValue;
                if (_existingBeneficiaries.TryGetValue(propertyValue, out var id))
                {
                    beneficiary.Id = id;
                }
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.Name.ToLower()))
            {
                beneficiary.Name = propertyValue;
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.FatherName.ToLower()))
            {
                beneficiary.FatherName = propertyValue;
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.MotherName.ToLower()))
            {
                beneficiary.MotherName = propertyValue;
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.FCNId.ToLower()))
            {
                beneficiary.FCNId = propertyValue;
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.DateOfBirth.ToLower()))
            {
                beneficiary.DateOfBirth = Convert.ToDateTime(propertyValue); 
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.Sex.ToLower()))
            {
                beneficiary.Sex = GetGender(propertyValue);
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.Disabled.ToLower()))
            {
                beneficiary.Disabled = GetBoolean(propertyValue);
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.LevelOfStudy.ToLower()))
            {
                beneficiary.LevelOfStudy = GetLevelOfEducation(propertyValue); 
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.EnrollmentDate.ToLower()))
            {
                beneficiary.EnrollmentDate = Convert.ToDateTime(propertyValue); 
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.FacilityId.ToLower()))
            {
                long facilityId = GetFacilityId(propertyValue);
                if (facilityId == 0)
                {
                    return false; 
                }

                beneficiary.FacilityId = facilityId; 
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.CampSSId.ToLower()))
            {
                int campId = GetCampId(propertyValue);
                if (campId == 0)
                {
                    return false;
                }

                beneficiary.BeneficiaryCampId = campId; 
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.BlockId.ToLower()))
            {
                int blockId = GetBlockId(propertyValue);
                if (blockId == 0)
                {
                    return false;
                }

                beneficiary.BlockId = blockId; 
            }
            else if (propertyName.Equals(BeneficiaryExcelColumnNames.SubBlockId.ToLower()))
            {
                int subBlockId = GetSubBlockId(propertyValue);
                if (subBlockId == 0)
                {
                    return false; 
                }

                beneficiary.SubBlockId = subBlockId; 
            }

            return true;
        }

        private int GetSubBlockId(string subBlockCode)
        {
            var camp = _allCamps.FirstOrDefault(c =>
                c.Blocks.Any(b => b.SubBlocks.Any(sb => !string.IsNullOrEmpty(sb.Name) && sb.Name.Equals(subBlockCode))));
            if (camp != null)
            {
                var block = camp.Blocks.First(b => b.SubBlocks.Any(sb => !string.IsNullOrEmpty(sb.Name) && sb.Name.Equals(subBlockCode)));
                var subBlock = block.SubBlocks.First(sb => !string.IsNullOrEmpty(sb.Name) && 
                    sb.Name.Equals(subBlockCode, StringComparison.InvariantCultureIgnoreCase));
                return subBlock.Id; 
            }
            return 0;
        }

        private int GetBlockId(string blockId)
        {
            var camp = _allCamps.FirstOrDefault(c =>
                c.Blocks.Any(b => !string.IsNullOrEmpty(b.Code) && b.Code.Equals(blockId, StringComparison.InvariantCultureIgnoreCase)));
            if (camp != null)
            {
                var block = camp.Blocks.First(b => !string.IsNullOrEmpty(b.Code) && b.Code.Equals(blockId, StringComparison.InvariantCultureIgnoreCase));
                return block.Id; 
            }

            return 0;
        }

        private int GetCampId(string campSSId)
        {
            var camp = _allCamps.FirstOrDefault(c => !string.IsNullOrEmpty(c.SSID) && 
                c.SSID.Equals(campSSId, StringComparison.InvariantCultureIgnoreCase));
            if (camp != null)
            {
                return camp.Id; 
            }

            return 0;
        }

        private long GetFacilityId(string facilityId)
        {
            var facility = _allFacilities.FirstOrDefault(f => !string.IsNullOrEmpty(f.FacilityCode) &&
                f.FacilityCode.Equals(facilityId, StringComparison.InvariantCultureIgnoreCase));
            if (facility != null)
            {
                return facility.Id;
            }
            return 0;
        }

        private string GetLevelOfEducation(string value)
        {
            if (value.Equals("Level 1", StringComparison.InvariantCultureIgnoreCase))
            {
                return ((int)LevelOfStudy.Level_1).ToString(); 
            }
            if (value.Equals("Level 2", StringComparison.InvariantCultureIgnoreCase))
            {
                return ((int)LevelOfStudy.Level_2).ToString();
            }
            if (value.Equals("Level 3", StringComparison.InvariantCultureIgnoreCase))
            {
                return ((int)LevelOfStudy.Level_3).ToString();
            }

            return ((int)LevelOfStudy.Level_1).ToString(); 
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
            foreach (var excelColumn in BeneficiaryExcelColumnNames.AllColumns)
            {
                if (!columnNames.Contains(excelColumn.ToLower()))
                {
                    _importResult.InvalidFile = true;
                    break;
                }
            }
        }

        private async Task Initialize(Stream stream)
        {
            try
            {
                throw new NotImplementedException();
                //_workbook = new WorkbookAdapter(stream);
                //_worksheet = _workbook.GetFirstWorksheet();
                //_importResult = new ImportResult<BeneficiaryAddViewModel>();
                //var pagedResponse = await _facilityRepository.GetAll(new BaseQueryModel());
                //_allFacilities.AddRange(pagedResponse.Data);
                //var allCamps = await _campRepository.GetAll()
                //    .Include(camp => camp.Blocks)
                //    .ThenInclude(block => block.SubBlocks).ToListAsync();
                //_allCamps.AddRange(allCamps);
                //_logger.LogInformation("File opened successfully");

                //TODO: beneficiary new schema
                //var beneficiary = await _beneficiaryRepository.GetAll()
                //    .Select(x => new { x.UnhcrId, x.Id })
                //    .ToListAsync();

                //beneficiary.ForEach(x => _existingBeneficiaries.TryAdd(x.UnhcrId, x.Id));

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public void Dispose()
        {
            if (null != _workbook)
            {
                _workbook.Dispose();
                _logger.LogInformation("File closed successfully");
            }
        }
    }
}
