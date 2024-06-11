using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace UnicefEducationMIS.Service.Import
{
    public class ExcelHelper
    {
        public static int ColumnNameToIndex(string columnName)
        {
            int num = 0;
            for (int i = 0; i < columnName.Length; i++)
            {
                if (!Char.IsLetter(columnName[i]))
                {
                    throw new Exception("Invalid Column name.");
                }
                num = (((num * 0x1a) + columnName[i]) - 0x41) + 1;
            }
            if (num > 0)
            {
                num--;
            }
            if (num > 0x3fff)
            {
                throw new Exception("Invalid Column name.");
            }
            return num;

        } 

        public static CellInfo GetCellInfo(string cellName)
        {
            var match = Regex.Match(cellName, ExcelConstants.OneOrMoreAdjacentDigits);
            var rowIndex = Convert.ToInt32(match.Captures[0].Value);
            var colName = cellName.Substring(0, match.Index);
            int colIndex = ColumnNameToIndex(colName);
            var cellInfo = new CellInfo { ColumnIndex = colIndex, RowIndex = rowIndex, CellName = cellName, ColumnName = colName };
            return cellInfo;
        }

        public static string GetCellRef(int rowIndex, string columnName)
        {
            return string.Format("{0}{1}", columnName, rowIndex);
        }

        private static char GetChart(int index)
        {
            return (char)('A' + index);
        }

        public static string GetColumnName(int colIndex)
        {
            colIndex--;
            return colIndex / 26 > 0 ? String.Format("{0}{1}", GetChart((colIndex / 26) - 1), GetChart(colIndex % 26)) : String.Format("{0}", GetChart(colIndex));
        }


        public static CellValues GetCellDataType(object cellValue)
        {
            var cellDataType = CellValues.SharedString;
            if (cellValue != null)
            {
                if (cellValue.GetType() == typeof(Int16) ||
                    cellValue.GetType() == typeof(Int32) ||
                    cellValue.GetType() == typeof(Int64) ||
                    cellValue.GetType() == typeof(double) ||
                    cellValue.GetType() == typeof(float) ||
                    cellValue.GetType() == typeof(decimal))
                {
                    cellDataType = CellValues.Number;
                }
                else if (cellValue.GetType() == typeof(DateTime))
                {
                    cellDataType = CellValues.Date;
                }
            }
            return cellDataType;
        }

        public static WorksheetPart GetWorkSheetPartByName(SpreadsheetDocument spreadsheetDocument, string name)
        {
            var sheet = spreadsheetDocument.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == name);
            if (sheet != null)
            {
                return (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(sheet.Id);
            }
            return null;
        }


        public static string ColumnIndexToName(int column)
        {
            if ((column < 0) || (column > 0x3fff))
            {
                throw new ArgumentOutOfRangeException();
            }
            var ch = (char)((column % 0x1a) + 0x41);
            string str = ch.ToString();
            column /= 0x1a;
            int num = 0;
            while (column > 0)
            {
                num = column % 0x1a;
                column /= 0x1a;
                if (num == 0)
                {
                    if (column == 1)
                    {
                        str = "Z" + str;
                        column = 0;
                    }
                    else
                    {
                        str = "A" + str;
                    }
                }
                else
                {
                    str = ((char)((num + 0x41) - 1)).ToString() + str;
                }
            }
            return str;
        }

        public static uint GetSheetId(Sheets sheets)
        {
            uint sheetId = 1;

            if (sheets.Elements<Sheet>().Any())
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }
            return sheetId;
        }

        public static Byte[] GetFileAsBytes(Stream ms)
        {
            ms.Seek(0, SeekOrigin.Begin);
            var fileData = new byte[ms.Length];
            ms.Read(fileData, 0, fileData.Length);
            return fileData;
        }

        public static string GetCellValueAsString(object cellValue, CellValues cellDataType)
        {
            string strCellValue = "";

            if (cellDataType == CellValues.Number)
            {
                if (cellValue != null)
                {
                    var numberValue = Double.Parse(cellValue.ToString());

                    strCellValue = numberValue == 0 ? "0" : numberValue.ToString("#.############");
                }
            }
            else
            {
                if (null != cellValue)
                {
                    strCellValue = cellValue.ToString();
                }
            }
            return strCellValue;
        }
    }
}