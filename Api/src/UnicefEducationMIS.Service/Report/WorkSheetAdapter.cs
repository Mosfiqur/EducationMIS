using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using UnicefEducationMIS.Service.Import;

namespace UnicefEducationMIS.Service.Report
{
    public class WorkSheetAdapter
    {
        private ExcelAdapter _excel;

        public WorkSheetAdapter(ExcelAdapter excel, WorksheetPart sheetPart)
        {
            SheetPart = sheetPart;
            _excel = excel;
        }

        public WorksheetPart SheetPart { get; set; }

        public void PutValueInCell(CellDefinition cellDefinition)
        {
            var styleIndex = ExcelStyles.Instance.Get(cellDefinition.StyleName);
            PutValueInCell(cellDefinition.Row, cellDefinition.Column, cellDefinition.Content, new UInt32Value(styleIndex));
            if (!string.IsNullOrEmpty(cellDefinition.MergeCellReference))
            {
                // merging cells 
                MergeCells(cellDefinition.MergeCellReference);
            }
        }

        public void PutValueInCell(int row, int column, Object cellValue, UInt32Value styleIndex)
        {
            //var cellRef = ExcelHelper.ColumnIndexToName(column) + row;
            var cellRef = ExcelHelper.GetColumnName(column);
            var range = $"{cellRef}{row}";
            PutValueInCell(cellValue, styleIndex, range);
        }

        public void PutValueInCell(object cellValue, UInt32Value styleIndex, string cellRef)
        {
            var cellDataType = ExcelHelper.GetCellDataType(cellValue);
            var strCellValue = ExcelHelper.GetCellValueAsString(cellValue, cellDataType);

            var newCell = GetNewCell(cellRef);

            if (cellValue != null)
            {
                if (cellDataType == CellValues.SharedString)
                {
                    var sharedStringIndex = _excel.InsertSharedString(strCellValue);
                    newCell.CellValue = new CellValue(sharedStringIndex);
                    newCell.DataType = cellDataType;
                }
                else
                {
                    newCell.CellValue = new CellValue(strCellValue);
                    newCell.DataType = cellDataType;
                }
            }
            if (styleIndex != null)
            {
                newCell.StyleIndex = styleIndex;
            }
            SheetPart.Worksheet.Save();
        }


        public void PutFormula(string cellRef, string formula, UInt32Value styleIndex)
        {
            var newCell = GetNewCell(cellRef);
            newCell.CellFormula = new CellFormula(formula);
            if (styleIndex != null)
            {
                newCell.StyleIndex = styleIndex;
            }
            SheetPart.Worksheet.Save();
        }

        public void PutSumFormula(int startRow, int endRow, int row, int column)
        {
            var startCellRef = ExcelHelper.ColumnIndexToName(column) + startRow;
            var endCellRef = ExcelHelper.ColumnIndexToName(column) + endRow;
            var cellRef = ExcelHelper.ColumnIndexToName(column) + row;
            var formula = string.Format("=SUM({0}:{1})", startCellRef, endCellRef);
            PutFormula(cellRef, formula, null);
        }
        public void PutAverageFormula(int startRow, int endRow, int row, int column)
        {
            var startCellRef = ExcelHelper.ColumnIndexToName(column) + startRow;
            var endCellRef = ExcelHelper.ColumnIndexToName(column) + endRow;
            var cellRef = ExcelHelper.ColumnIndexToName(column) + row;
            var formula = string.Format("=AVERAGE({0}:{1})", startCellRef, endCellRef);
            PutFormula(cellRef, formula, null);
        }

        public void PutCountFormula(int startRow, int endRow, int row, int column)
        {
            var startCellRef = ExcelHelper.ColumnIndexToName(column) + startRow;
            var endCellRef = ExcelHelper.ColumnIndexToName(column) + endRow;
            var cellRef = ExcelHelper.ColumnIndexToName(column) + row;
            var formula = string.Format("=ROWS({0}:{1})", startCellRef, endCellRef);
            PutFormula(cellRef, formula, null);
        }


        public Cell GetCell(string cellRef)
        {
            uint rowIndex = GetRowIndex(cellRef);
            Row desiredRow = SheetPart.Worksheet.Descendants<Row>().FirstOrDefault(r => r.RowIndex.Value == rowIndex);
            if (desiredRow != null)
            {
                return desiredRow.Elements<Cell>().FirstOrDefault(c => c.CellReference.Value == cellRef);
            }
            return null;
        }

        public void SetColumnWidth(string columnName, double width)
        {
            // checking for SheetData part
            var sheetData = GetSheetData();
            Columns cols = null;
            if (!SheetPart.Worksheet.Elements<Columns>().Any())
            {
                cols = SheetPart.Worksheet.InsertBefore(new Columns(), sheetData);
            }
            else
            {
                cols = SheetPart.Worksheet.GetFirstChild<Columns>();
            }
            var index = new UInt32Value((uint)(ExcelHelper.ColumnNameToIndex(columnName)));

            var column = new Column { CustomWidth = new BooleanValue(true), Min = index, Max = index, Width = new DoubleValue(width) };

            cols.AppendChild(column);
            SheetPart.Worksheet.Save();
        }

        private SheetData GetSheetData()
        {
            SheetData sheetData = null;
            if (!SheetPart.Worksheet.Elements<SheetData>().Any())
            {
                sheetData = SheetPart.Worksheet.AppendChild(new SheetData());
            }
            else
            {
                sheetData = SheetPart.Worksheet.GetFirstChild<SheetData>();
            }
            return sheetData;
        }

        public void MergeCells(string range)
        {
            var sheetData = GetSheetData();
            MergeCells mergeCells = null;
            if (!SheetPart.Worksheet.Elements<MergeCells>().Any())
            {
                mergeCells = SheetPart.Worksheet.InsertAfter(new MergeCells(), sheetData);
            }
            else
            {
                mergeCells = SheetPart.Worksheet.GetFirstChild<MergeCells>();
            }
            var mergeCell = new MergeCell { Reference = range };
            mergeCells.AppendChild(mergeCell);
            SheetPart.Worksheet.Save();
        }

        public Row InsertRowBefore(uint rowIndex)
        {
            var refRow = SheetPart.Worksheet.Descendants<Row>().FirstOrDefault(r => r.RowIndex.Value == rowIndex);
            if (refRow != null)
            {
                var newRow = (Row)refRow.CloneNode(true);
                var sheetData = SheetPart.Worksheet.GetFirstChild<SheetData>();
                uint newRowIndex;
                IEnumerable<Row> rows = sheetData.Descendants<Row>().Where(r => r.RowIndex.Value >= rowIndex);
                foreach (Row row in rows)
                {
                    newRowIndex = Convert.ToUInt32(row.RowIndex.Value + 1);

                    foreach (Cell cell in row.Elements<Cell>())
                    {
                        // Update the references for reserved cells.
                        string cellReference = cell.CellReference.Value;
                        cell.CellReference = new StringValue(cellReference.Replace(row.RowIndex.Value.ToString(), newRowIndex.ToString()));
                    }
                    // Update the row index.
                    row.RowIndex = new UInt32Value(newRowIndex);
                }
                return sheetData.InsertBefore(newRow, refRow);
            }
            return null;
        }

        #region Private Methods

        private Row GetNewRow(uint rowIndex)
        {
            var desiredRow = SheetPart.Worksheet.Descendants<Row>().FirstOrDefault(r => r.RowIndex.Value == rowIndex);
            if (desiredRow == null)
            {
                var newRow = new Row
                {
                    RowIndex = new UInt32Value(rowIndex)
                };

                //Previous Row
                var prevoiusRow = SheetPart.Worksheet.Descendants<Row>().FirstOrDefault(r => r.RowIndex.Value == rowIndex - 1);
                if (prevoiusRow != null)
                {
                    return SheetPart.Worksheet.GetFirstChild<SheetData>().InsertAfter(newRow, prevoiusRow);
                }

                //Next Row
                var nextRow = SheetPart.Worksheet.Descendants<Row>().FirstOrDefault(r => r.RowIndex.Value == rowIndex + 1);
                if (nextRow != null)
                {
                    return SheetPart.Worksheet.GetFirstChild<SheetData>().InsertAfter(newRow, nextRow);
                }

                //Insert after/before last Cell 
                var lastRow = SheetPart.Worksheet.
                                        Descendants<Row>().LastOrDefault();
                var firstRow = SheetPart.Worksheet.
                                      Descendants<Row>().FirstOrDefault();

                if (firstRow != null)
                {
                    if (rowIndex > firstRow.RowIndex && rowIndex < lastRow.RowIndex)
                    {
                        if (lastRow.RowIndex > rowIndex)
                        {
                            return SheetPart.Worksheet.GetFirstChild<SheetData>().InsertBefore(newRow, lastRow);
                        }
                    }
                    else if (rowIndex < firstRow.RowIndex)
                    {
                        return SheetPart.Worksheet.GetFirstChild<SheetData>().InsertBefore(newRow, firstRow);
                    }
                    else if (rowIndex > lastRow.RowIndex)
                    {
                        return SheetPart.Worksheet.GetFirstChild<SheetData>().InsertAfter(newRow, lastRow);
                    }
                }
                return SheetPart.Worksheet.GetFirstChild<SheetData>().AppendChild(newRow);
            }
            return desiredRow;
        }

        private uint GetRowIndex(string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            var regex = new Regex(@"\d+");
            Match match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }

        private string GetColumnName(string cellName)
        {
            // Create a regular expression to match the column name portion of the cell name.
            var regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellName);

            return match.Value;
        }

        private string GetPreviousCellRef(string cellRef)
        {
            uint rowIndex = GetRowIndex(cellRef);

            int columnIndex = ExcelHelper.ColumnNameToIndex(GetColumnName(cellRef));
            string columnName = ExcelHelper.ColumnIndexToName(columnIndex - 1);
            var nextCellRef = columnName + rowIndex;
            return nextCellRef;

        }

        private string GetNextCellRef(string cellRef)
        {
            uint rowIndex = GetRowIndex(cellRef);

            int columnIndex = ExcelHelper.ColumnNameToIndex(GetColumnName(cellRef));
            string columnName = ExcelHelper.ColumnIndexToName(columnIndex + 1);//GetColumnName(columnIndex + 1);
            var nextCellRef = columnName + rowIndex;
            return nextCellRef;
        }
        private bool ShouldInsertAfter(string cellName, Cell lastCell)
        {
            int desiredColumnIndex = ExcelHelper.ColumnNameToIndex(GetColumnName(cellName));
            int lastCellColumnIndex = ExcelHelper.ColumnNameToIndex(GetColumnName(lastCell.CellReference));
            return lastCellColumnIndex < desiredColumnIndex;
        }
        private bool ShouldInsertBefore(string cellName, Cell lastCell)
        {
            int desiredColumnIndex = ExcelHelper.ColumnNameToIndex(GetColumnName(cellName));
            int lastCellColumnIndex = ExcelHelper.ColumnNameToIndex(GetColumnName(lastCell.CellReference));
            return lastCellColumnIndex > desiredColumnIndex;
        }

        private Cell GetNewCell(string cellName)
        {
            uint rowIndex = GetRowIndex(cellName);
            var desiredRow = SheetPart.Worksheet.Descendants<Row>().FirstOrDefault(r => r.RowIndex.Value == rowIndex);
            if (desiredRow == null)
            {
                var row = GetNewRow(rowIndex);

                var cell = new Cell
                {
                    CellReference = new StringValue(cellName)
                };

                row.Append(cell);

                return cell;
            }

            var desiredCell = desiredRow.Elements<Cell>().FirstOrDefault(c => c.CellReference.Value == cellName);
            //Create new Cell
            if (desiredCell == null)
            {
                var newCell = new Cell
                {
                    CellReference = new StringValue(cellName)
                };

                //Previous Cell
                var prevoiusCell = desiredRow.Elements<Cell>().FirstOrDefault(c => c.CellReference == GetPreviousCellRef(cellName));
                if (prevoiusCell != null)
                {
                    return desiredRow.InsertAfter(newCell, prevoiusCell);
                }

                //Next Cell
                var nextCell = desiredRow.Elements<Cell>().FirstOrDefault(c => c.CellReference == GetNextCellRef(cellName));
                if (nextCell != null)
                {
                    return desiredRow.InsertBefore(newCell, nextCell);
                }

                //Insert after/before last Cell 
                var lastCell = desiredRow.Elements<Cell>().LastOrDefault();
                if (lastCell != null)
                {
                    if (ShouldInsertAfter(cellName, lastCell))
                    {
                        return desiredRow.InsertAfter(newCell, lastCell);
                    }
                    if (ShouldInsertBefore(cellName, lastCell))
                    {
                        return desiredRow.InsertBefore(newCell, lastCell);
                    }
                }

                return desiredRow.AppendChild(newCell);
            }
            return desiredCell;
        }
        #endregion

        public Row GetRow(int rowIndex)
        {
            return SheetPart.Worksheet.Descendants<Row>().FirstOrDefault(r => r.RowIndex.Value == rowIndex);
        }

        public void DeleteRow(int rowIndex)
        {
            var refRow = GetRow(rowIndex);
            if (refRow != null)
            {
                refRow.Remove();
                var sheetData = SheetPart.Worksheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>().Where(r => r.RowIndex.Value > rowIndex);
                uint newRowIndex;
                foreach (Row row in rows)
                {
                    newRowIndex = Convert.ToUInt32(row.RowIndex.Value - 1);
                    row.RowIndex = new UInt32Value(newRowIndex);
                    IEnumerable<Cell> cells = row.Descendants<Cell>();
                    var colIndex = 0;
                    foreach (Cell cell in cells)
                    {
                        var cellRef = ExcelHelper.ColumnIndexToName(colIndex) + newRowIndex;
                        cell.CellReference = new StringValue(cellRef);
                        colIndex++;
                    }
                }
                SheetPart.Worksheet.Save();
            }
        }

        public void ApplyStyleInCell(string cellRef, uint styleIndex)
        {
            var cell = GetNewCell(cellRef);
            cell.StyleIndex = styleIndex;
            SheetPart.Worksheet.Save();
        }

        public void ApplyStyleInCellRange(string range, uint styleIndex)
        {
            var array = range.Split(new[] { ':' });
            var upperLeftCell = array[0];
            var lowerRightCell = array[1];
            var upperLeftCellInfo = ExcelHelper.GetCellInfo(upperLeftCell);
            var lowerRightCellInfo = ExcelHelper.GetCellInfo(lowerRightCell);
            for (int colIndex = upperLeftCellInfo.ColumnIndex; colIndex <= lowerRightCellInfo.ColumnIndex; colIndex++)
            {
                for (int rowIndex = upperLeftCellInfo.RowIndex; rowIndex <= lowerRightCellInfo.RowIndex; rowIndex++)
                {
                    var cellName = ExcelHelper.ColumnIndexToName(colIndex) + rowIndex;
                    var cell = GetNewCell(cellName);
                    cell.StyleIndex = styleIndex;
                }
            }
            SheetPart.Worksheet.Save();
        }

        public void ProtectWorksheet()
        {
            SheetProtection sheetProtection = null;
            if (!SheetPart.Worksheet.Elements<SheetProtection>().Any())
            {
                sheetProtection = new SheetProtection();
                SheetPart.Worksheet.InsertAfter(sheetProtection, GetSheetData());
            }
            else
            {
                sheetProtection = SheetPart.Worksheet.GetFirstChild<SheetProtection>();
            }
            var protectionPassword = "Abcd1234";
            if (!string.IsNullOrEmpty(protectionPassword))
            {
                sheetProtection.Password = protectionPassword;
            }
            sheetProtection.DeleteColumns = false;
            sheetProtection.DeleteRows = false;
            sheetProtection.FormatCells = false;
            sheetProtection.FormatRows = false;
            sheetProtection.FormatColumns = false;
            sheetProtection.Sheet = true;
            sheetProtection.AutoFilter = false;
            sheetProtection.InsertColumns = false;
            sheetProtection.InsertRows = false;
            sheetProtection.InsertHyperlinks = false;
            sheetProtection.PivotTables = false;
            sheetProtection.SelectLockedCells = true;
            sheetProtection.SelectUnlockedCells = true;
            SheetPart.Worksheet.Save();
        }
    }
}
