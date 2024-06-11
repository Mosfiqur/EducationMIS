using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.Import;

namespace UnicefEducationMIS.Service.Import
{
    public abstract class AbstractWorkBookImporter<T> : IDisposable
    {
        private WorkbookAdapter _workbook;
        private WorksheetAdapter _worksheet;
        private Dictionary<int, string> _columnIndexToColumnName;
        protected List<ExcelCell> _allColumns;
        private readonly ILogger<T> _logger;

        public ImportResult<T> ImportResult { get; protected set; }

        public AbstractWorkBookImporter(ILogger<T> logger)
        {
            _columnIndexToColumnName = new Dictionary<int, string>();
            ImportResult = new ImportResult<T>();
            _logger = logger;
            _allColumns = new List<ExcelCell>();
        }

        public List<CellAdapter> GetWorksheetColumn(int index)
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

                cells.Add(row.Cells[index]);
            }
            return cells;
        }

        public virtual async Task InitializeWorkbook(Stream stream)
        {
            _workbook = new WorkbookAdapter(stream);
            _worksheet = _workbook.GetFirstWorksheet();
        }

        public virtual async Task<ImportResult<T>> Process()
        {
            ReadColumns();
            CheckRequiredColumnsExists();

            if (ImportResult.InvalidFile)
            {
                return ImportResult;
            }

            await ReadData();
            return await Task.FromResult(ImportResult);
        }

        private void ReadColumns()
        {
            var headerRow = _worksheet.ReadRow(ExcelConstants.HeaderRowIndex);
            if (headerRow.IsBlankRow())
            {
                ImportResult.InvalidFile = true;
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
                _columnIndexToColumnName.Add(columnIndex, columnHeader.ToLower().Trim());
            }

        }

        private void CheckRequiredColumnsExists()
        {
            var columnNames = _columnIndexToColumnName.Values.ToList();
            foreach (var excelColumn in _allColumns.Where(x => x.IsMandatory))
            {
                if (!columnNames.Contains(excelColumn.DisplayName.ToLower().Trim()))
                {
                    ImportResult.InvalidFile = true;
                    ImportResult.Fail(string.Format(Messages.MissingIndicator, excelColumn.DisplayName));
                    break;
                }
            }
        }

        private async Task ReadData()
        {
            var dataStartRowIndex = ExcelConstants.HeaderRowIndex + 1;
            for (int rowIndex = dataStartRowIndex; rowIndex <= _worksheet.MaxRowIndex; rowIndex++)
            {
                var row = _worksheet.ReadRow(rowIndex);
                if (row == null || row.IsBlankRow())
                {
                    continue;
                }

                T entity = await ConvertToEntity(row);
                if (entity != null)
                {
                   await ApplyBusinessLogic(entity, rowIndex);
                }
            }
        }
        // Todo: Need to refactor
        private async Task<T> ConvertToEntity(RowAdapter row)
        {
            var entity = (T)Activator.CreateInstance(typeof(T));
            var isValidEntity = true;
            var enumerator = _columnIndexToColumnName.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var columnIndex = enumerator.Current;
                var columnName = _columnIndexToColumnName[columnIndex];
                var column = _allColumns.SingleOrDefault(x => x.DisplayName.Trim() == columnName.Trim());
                if (column == null)
                {
                    ImportResult.RowErrors.Add(GetRowError(row.RowIndex, $"Column not present - {columnName}"));
                    continue;
                }

                var cell = row.GetCell(columnIndex);
                bool isAutoCalculatedColumn = _allColumns.Where(x => x.IsAutoCalculated).Any(x => x.DisplayName == columnName);
                if (cell?.Value == null && !isAutoCalculatedColumn)
                {
                    if (_allColumns.Where(x => x.IsMandatory).Any(x => x.DisplayName == columnName))
                    {
                        isValidEntity = false;
                        ImportResult.RowErrors.Add(GetRowError(row.RowIndex, $"Column has no value - {columnName}"));

                    }
                }

                if (isValidEntity)
                {
                    if (isAutoCalculatedColumn && cell == null)
                        column.Value = "0";
                    else
                        column.Value = cell?.Value?.ToString();
                    
                    isValidEntity = await SetPropertyValue(column, entity);
                    if (!isValidEntity)
                    {
                        ImportResult.RowErrors.Add(GetRowError(row.RowIndex, $"Column has invalid value - {columnName}"));
                    }
                }
            }

            if (!isValidEntity)
            {
                return default(T);
            }

            return entity;
        }

        private RowError GetRowError(int rowIndex, string errorMsg)
        {
            return new RowError
            {
                RowNumber = rowIndex,
                ErrorMessage = errorMsg
            };
        }

        public abstract Task<bool> SetPropertyValue(ExcelCell column, T entity);

        public abstract Task ApplyBusinessLogic(T entity, int rowIndex);

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
