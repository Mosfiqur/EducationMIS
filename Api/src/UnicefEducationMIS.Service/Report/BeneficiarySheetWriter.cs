using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public class BeneficiarySheetWriter : ExcelSheetDataWriter
    {
        private int _totalColumns = 0;
        private List<BeneficiaryViewModel> _beneficiaries;
        private List<IndicatorSelectViewModel> _indicators;
        private List<string> _columnPositionList;
        private readonly List<ReportCell> _fixedColumns = new List<ReportCell>();
        

        public BeneficiarySheetWriter(ExcelAdapter excel) : base(excel)
        {
            
        }

        public async Task Write(List<BeneficiaryViewModel> allBeneficiaries, List<IndicatorSelectViewModel> indicators, List<ReportCell> cells)
        {
            _fixedColumns.AddRange(cells);
            _beneficiaries = allBeneficiaries;
            _indicators = indicators;
            UInt32Value startRowIndex = 1;
            _totalColumns = _fixedColumns.Count + _indicators.Count;
            _columnPositionList = GetColumnPositions();
            OpenXmlReader reader = OpenXmlReader.Create(worksheetPart);
            OpenXmlWriter writer = OpenXmlWriter.Create(replacementPart);
            while (reader.Read())
            {
                if (reader.ElementType == typeof(SheetData))
                {
                    if (reader.IsEndElement)
                        continue;
                    var columns = GetColumnsWithWidth();
                    writer.WriteElement(columns);
                    writer.WriteStartElement(new SheetData()); // start of sheet data
                    WriteHeaders(ref startRowIndex, writer);
                    WriteBeneficiaries(ref startRowIndex, writer);
                    writer.WriteEndElement(); // end of sheet data
                }
                else
                {
                    if (reader.IsStartElement)
                        writer.WriteStartElement(reader);
                    else if (reader.IsEndElement)
                        writer.WriteEndElement();
                }
            }
            reader.Close();
            writer.Close();

            var sheet = workbookPart.Workbook
                .Descendants<Sheet>().FirstOrDefault(s => s.Id.Value.Equals(origninalSheetId));
            if (sheet != null) sheet.Id.Value = replacementPartId;
            workbookPart.DeletePart(worksheetPart);
        }

        private void WriteBeneficiaries(ref UInt32Value startRowIndex, OpenXmlWriter writer)
        {
            foreach (var beneficiary in _beneficiaries)
            {
                StartOpenXmlRow(writer, startRowIndex);
                WriteBeneficiary(startRowIndex, beneficiary, writer);
                EndOpenXmlRow(writer);
                startRowIndex++;
            }
        }

        private void WriteBeneficiary(UInt32Value startRowIndex, BeneficiaryViewModel beneficiary, OpenXmlWriter writer)
        {
            var t = beneficiary.GetType();
            var colIndex = -1;

            _fixedColumns.ForEach(column =>
            {
                colIndex++;
                object cellValue;
                var propInfo = t.GetProperty(column.ActualName);

                if (propInfo == null)
                {
                    return;
                }

                var type = Nullable.GetUnderlyingType(propInfo.PropertyType);

                if (propInfo.PropertyType.IsEnum || type != null && type.IsEnum)
                {
                    cellValue = propInfo.GetValue(beneficiary).ToString();
                }
                else if (propInfo.PropertyType == typeof(bool))
                {
                    cellValue = propInfo.GetValue(beneficiary);
                    if ((bool)cellValue)
                    {
                        cellValue = "Yes";
                    }
                    else
                    {
                        cellValue = "No";
                    }
                }
                else
                {
                    cellValue = propInfo.GetValue(beneficiary);
                }

                var cellReference = _columnPositionList[colIndex] + startRowIndex;
                WriteOpenXmlCell(writer, cellReference, _fixedColumns[colIndex].Style, cellValue);
            });

            if (beneficiary.Properties.Count <= 0)
            {
                return;
            }

            
            _indicators
                .ForEach(indicator =>
                {
                    colIndex++;
                    var cellReference = _columnPositionList[colIndex] + startRowIndex;

                    var property = beneficiary.Properties.FirstOrDefault(x => x.EntityColumnId == indicator.EntityDynamicColumnId);
                    if (property == null)
                    {
                        WriteOpenXmlCell(writer, cellReference, GetStyleForIndicator(indicator), string.Empty);
                        return;
                    }

                    string value;
                    if (property.DataType == ColumnDataType.List)
                    {
                        var val =
                        property.ListItem.Where(x => property.Values.Select(int.Parse).Contains(x.Value))
                            .Select(x => x.Title);
                        value = string.Join(",", val);
                    }
                    else
                    {
                        value = string.Join(",", property.Values);
                    }

                    WriteOpenXmlCell(writer, cellReference, GetStyleForIndicator(indicator), value);
                });
        }

        private uint GetStyleForIndicator(IndicatorSelectViewModel indicator)
        {
            switch (indicator.ColumnDataType)
            {
                case ColumnDataType.List:
                    return WrappedTextStyle;
                case ColumnDataType.Boolean:
                    return WrappedTextStyle;
                case ColumnDataType.Datetime:
                    return DateStyle;
                case ColumnDataType.Decimal:
                    return SixDecimalStyle;
                case ColumnDataType.Integer:
                    return NumberStyle;
                case ColumnDataType.Text:
                    return WrappedTextStyle;
                default:
                    return WrappedTextStyle;
            }
        }

        private List<string> GetColumnPositions()
        {
            List<string> columnPositionList = new List<string>();
            
            for (int i = 1; i <= _totalColumns; i++)
            {
                columnPositionList.Add(GetExcelTypeColumnString(i));
            }
            return columnPositionList;
        }

        private void WriteHeaders(ref UInt32Value startRowIndex, OpenXmlWriter writer)
        {
            StartOpenXmlRow(writer, startRowIndex);
            var index = 0;
            for (; index < _fixedColumns.Count; index++)
            {
                var cellReference = _columnPositionList[index] + startRowIndex;
                WriteOpenXmlCell(writer, cellReference, HeaderStyle, _fixedColumns[index].DisplayName);
            }

            for (;index < _totalColumns; index++)
            {
                var cellReference = _columnPositionList[index] + startRowIndex;
                var indicator = _indicators[index- _fixedColumns.Count];
                WriteOpenXmlCell(writer, cellReference, HeaderStyle, indicator.IndicatorName);                
            }


            EndOpenXmlRow(writer);
            startRowIndex++;
        }

        private OpenXmlElement GetColumnsWithWidth()
        {
            UInt32Value columnNumber = 1;
            Columns columns = new Columns();
            for (int i = 0; i < _fixedColumns.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            for (int i = 0; i < _indicators.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            return columns;
        }

    }
}
