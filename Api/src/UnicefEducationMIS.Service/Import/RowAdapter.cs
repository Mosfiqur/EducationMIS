using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;

namespace UnicefEducationMIS.Service.Import
{
    public class RowAdapter
    {
        private readonly Row _excelRow;
        private readonly WorkbookAdapter _workbookAdapter;
        private readonly WorksheetAdapter _worksheetAdapter;
        private readonly Dictionary<string, CellAdapter> _cellRefToExcelCell;
        private readonly Dictionary<int, CellAdapter> _colIndexToExcelCell;

        public RowAdapter(Row excelRow, WorkbookAdapter workbookAdapter, WorksheetAdapter worksheetAdapter)
        {
            _excelRow = excelRow;
            _workbookAdapter = workbookAdapter;
            _worksheetAdapter = worksheetAdapter;
            Cells = new List<CellAdapter>();
            _cellRefToExcelCell = new Dictionary<string, CellAdapter>();
            _colIndexToExcelCell = new Dictionary<int, CellAdapter>();
        }

        public int RowIndex
        {
            get
            {
                return Convert.ToInt32(_excelRow.RowIndex.Value);
            }
        }

        public List<CellAdapter> Cells { get; set; }

        public string SheetName
        {
            get
            {
                return _worksheetAdapter.SheetName;
            }
        }

        public void Read()
        {
            foreach (var cellElement in _excelRow.ChildElements)
            {
                if ((cellElement is Cell) == false) continue;
                var cell = ((Cell)cellElement);
                var excelCell = new CellAdapter(cell, _workbookAdapter);
                excelCell.Read();
                Cells.Add(excelCell);
                _cellRefToExcelCell.Add(excelCell.CellReference, excelCell);
                _colIndexToExcelCell.Add(excelCell.ColumnIndex, excelCell);
            }
        }

        public CellAdapter this[string cellReference]
        {
            get
            {
                if (_cellRefToExcelCell.ContainsKey(cellReference))
                    return _cellRefToExcelCell[cellReference];
                return null;
            }
        }

        public CellAdapter this[int colIndex]
        {
            get
            {
                if (_colIndexToExcelCell.ContainsKey(colIndex))
                    return _colIndexToExcelCell[colIndex];
                return null;
            }
        }

        public CellAdapter GetCellByReference(string cellReference)
        {
            foreach (var cell in Cells)
            {
                if (cell.CellReference.Equals(cellReference))
                    return cell;
            }
            var nullCell = new NullOpenXmlExcelCellAdapter(null, _workbookAdapter);
            return nullCell;
        }

        public CellAdapter GetCell(string columnName, int rowNumber)
        {
            return GetCellByReference(columnName + rowNumber);
        }

        private string GetExcelColumnName(int columnNumber)
        {
            var dividend = columnNumber;
            var columnName = String.Empty;

            while (dividend > 0)
            {
                var modulo = (dividend) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }

        /// <summary>
        /// columnNumber is zero based.
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <param name="rowNumber"></param>
        /// <returns></returns>
        public CellAdapter GetCell(int columnNumber, int rowNumber)
        {
            return Cells.SingleOrDefault(c => c.ColumnIndex.Equals(columnNumber) && c.RowIndex.Equals(rowNumber));
        }

        public CellAdapter GetCell(int columnNumber)
        {
            return Cells.SingleOrDefault(c => c.ColumnIndex.Equals(columnNumber) && c.RowIndex.Equals(RowIndex));
        }

        public bool IsBlankRow()
        {
            foreach (var cell in Cells)
            {
                if (cell.Value != DBNull.Value)
                    return false;
            }
            return true;
        }

        public bool ContainsValue(int columnIndex)
        {
            var colName = ExcelHelper.GetColumnName(columnIndex);
            var cellRef = string.Format("{0}{1}", colName, RowIndex);
            if (_cellRefToExcelCell.ContainsKey(cellRef))
            {
                return _cellRefToExcelCell[cellRef].ContainsValue();
            }
            return false;
        }

        public bool ContainsValue(string columnName)
        {
            var cellRef = ExcelHelper.GetCellRef(RowIndex, columnName);
            if (_cellRefToExcelCell.ContainsKey(cellRef))
            {
                return _cellRefToExcelCell[cellRef].ContainsValue();
            }
            return false;
        }

        public bool ContainsValueInCells(params int[] colIndexes)
        {
            foreach (var colIndex in colIndexes)
            {
                if (ContainsValue(colIndex) == false)
                    return false;
            }
            return true;
        }

        public bool ContainsValueInCells(params string[] columnNames)
        {
            foreach (var columnName in columnNames)
            {
                if (ContainsValue(columnName) == false)
                    return false;
            }
            return true;
        }

        public bool ContainsValues(out List<string> emptyCells, params string[] strValues)
        {
            emptyCells = new List<string>();
            foreach (var strValue in strValues)
            {
                var containsValue = false;
                foreach (var cell in Cells)
                {
                    if (cell.Value == DBNull.Value)
                        continue;

                    if (strValue.Equals(cell.Value.ToString()))
                    {
                        containsValue = true;
                        break;
                    }
                }
                if (containsValue == false)
                {
                    emptyCells.Add(strValue);
                }
            }
            return emptyCells.Count == 0;
        }
    }
}
