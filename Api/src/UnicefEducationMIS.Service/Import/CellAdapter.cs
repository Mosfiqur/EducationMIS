using System;
using System.Globalization;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;

namespace UnicefEducationMIS.Service.Import
{
    public class CellAdapter
    {
        private readonly Cell _cell;
        private readonly WorkbookAdapter _workbookAdapter;
        private string _cellReference;

        public CellAdapter(Cell cell, WorkbookAdapter workbookAdapter)
        {
            _cell = cell;
            _workbookAdapter = workbookAdapter;
        }

        public virtual object Value { get; set; }

        public ExcelDataTypes DataType { get; set; }

        public string CellReference
        {
            get
            {
                return _cellReference;
            }
            set
            {
                _cellReference = value;
                var cellInfo = ExcelHelper.GetCellInfo(_cellReference);
                ColumnName = cellInfo.ColumnName;
                ColumnIndex = cellInfo.ColumnIndex;
                RowIndex = cellInfo.RowIndex;
            }
        }

        public int RowIndex { get; set; }

        public int ColumnIndex { get; set; }

        public string ColumnName { get; set; }

        public void Read()
        {
            CellReference = _cell.CellReference;
            if (_cell.DataType == null)
            {
                if (_cell.StyleIndex == null)
                    ReadAsString();
                else
                    ReadFromStyle();
                return;
            }

            if ((_cell.DataType == CellValues.InlineString) || (_cell.DataType == CellValues.String))
            {
                ReadAsString();
                return;
            }
            if (_cell.DataType.Value == CellValues.SharedString)
            {
                ReadAsSharedString();
                return;
            }
            ReadFromStyle();
        }

        private void ReadAsSharedString()
        {
            DataType = ExcelDataTypes.String;
            Value = _workbookAdapter.GetSharedString(int.Parse(_cell.CellValue.Text.Trim())).Trim();
        }

        private void ReadAsString()
        {
            DataType = ExcelDataTypes.String;
            if (_cell.CellValue == null || _cell.CellValue.Text.Trim().Equals(string.Empty))
            {
                Value = DBNull.Value;
            }
            else
            {
                Value = _cell.CellValue.Text.Trim();
            }
        }

        private void ReadFromStyle()
        {
            if (_cell.StyleIndex == null)
            {
                ReadAsString();
                return;
            }
            var stylesheetPart = _workbookAdapter.GetStyleSheet();
            var cellFormat = stylesheetPart.CellFormats.ChildElements[int.Parse(_cell.StyleIndex.InnerText)] as CellFormat;
            if (cellFormat != null)
            {
                if (IsDate(cellFormat, stylesheetPart))
                {
                    ReadAsDateTime();
                }
                else if (IsNumber(cellFormat))
                {
                    ReadAsNumber(cellFormat);
                }
                else if (IsPercentage(cellFormat, stylesheetPart))
                {
                    ReadAsPercentage(cellFormat);
                }
                else
                {
                    ReadAsString();
                }
            }
            else
            {
                ReadAsString();
            }
        }

        private void ReadAsDateTime()
        {
            DataType = ExcelDataTypes.DateTime;
            if (null != _cell.CellValue)
            {
                if (string.IsNullOrEmpty(_cell.CellValue.Text.Trim()))
                {
                    Value = DBNull.Value;
                }
                else
                {
                    Value = DateTime.FromOADate(double.Parse(_cell.CellValue.Text.Trim()));
                }
            }
            else
            {
                Value = DBNull.Value;
            }
        }

        private void ReadAsPercentage(CellFormat cellFormat)
        {
            DataType = ExcelDataTypes.Number;
            if (_cell.CellValue == null || string.IsNullOrEmpty(_cell.CellValue.Text.Trim()))
            {
                Value = DBNull.Value;
            }
            else
            {
                double value;
                if (double.TryParse(_cell.CellValue.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                {
                    if (IsPercentageNumberFormat(cellFormat))
                    {
                        Value = value * 100;
                    }
                    else
                    {
                        Value = value;
                    }
                }
                else
                {
                    Value = _cell.CellValue.Text.Trim();
                }
            }
        }

        private static bool IsPercentageNumberFormat(CellFormat cellFormat)
        {
            return cellFormat.NumberFormatId == 169 || cellFormat.NumberFormatId == 9 || cellFormat.NumberFormatId == 10 || cellFormat.NumberFormatId == 166 || cellFormat.NumberFormatId == 167 || cellFormat.NumberFormatId == 168 || cellFormat.NumberFormatId == 165;
        }

        private void ReadAsNumber(CellFormat cellFormat)
        {
            DataType = ExcelDataTypes.Number;
            if (_cell.CellValue == null || string.IsNullOrEmpty(_cell.CellValue.Text.Trim()))
            {
                Value = DBNull.Value;
            }
            else
            {
                double value;
                if (double.TryParse(_cell.CellValue.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                {
                    if (cellFormat.NumberFormatId == 9 || cellFormat.NumberFormatId == 10)
                        Value = value * 100;
                    else
                    {
                        Value = value;
                    }
                }
                else
                {
                    Value = _cell.CellValue.Text.Trim();
                }
            }
        }

        private bool IsNumber(CellFormat cf)
        {
            if ((cf.NumberFormatId >= 1 && cf.NumberFormatId <= 4) || cf.NumberFormatId == 44)
            {
                return true;
            }
            return false;
        }

        private bool IsDate(CellFormat cf, Stylesheet stylesheetPart)
        {
            if (cf.NumberFormatId >= 14 && cf.NumberFormatId <= 22)
            {
                return true;
            }
            if (stylesheetPart.NumberingFormats == null) return false;
            var numberFormat = stylesheetPart.NumberingFormats.Descendants<NumberingFormat>().SingleOrDefault(n => n.NumberFormatId.Value.Equals(cf.NumberFormatId.Value));
            if (numberFormat != null)
            {
                if (numberFormat.FormatCode.InnerText.Contains("yy"))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsPercentage(CellFormat cellFormat, Stylesheet stylesheetPart)
        {
            if (cellFormat.NumberFormatId == 9 || cellFormat.NumberFormatId == 10)
                return true;
            if (stylesheetPart.NumberingFormats == null)
                return false;
            var numberFormat = stylesheetPart.NumberingFormats.Descendants<NumberingFormat>().SingleOrDefault(n => n.NumberFormatId.Value.Equals(cellFormat.NumberFormatId.Value));
            if (numberFormat != null)
            {
                if (numberFormat.FormatCode.InnerText.EndsWith("%"))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsValue()
        {
            if (Value is string)
            {
                if (string.IsNullOrEmpty(Value as string))
                    return false;
            }
            return Value != DBNull.Value;
        }
    }
}
