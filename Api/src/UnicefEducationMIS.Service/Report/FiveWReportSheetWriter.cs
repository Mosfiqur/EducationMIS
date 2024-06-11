using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public class FiveWReportSheetWriter : ExcelSheetDataWriter
    {
        private List<string> _columnPositionList;
        private List<FacilityViewModel> _facilities;
        private List<IndicatorSelectViewModel> _indicators;  
        private Dictionary<int, uint> _columnIndexToStyleIndex = new Dictionary<int, uint>(); 
        private Dictionary<long, int> _indicatoroIdToColumnIndex = new Dictionary<long, int>();
        private Dictionary<long, int> _reportingPersonToColumnIndex = new Dictionary<long, int>();
        private Dictionary<long, int> _calculatedFieldToColumnIndex = new Dictionary<long, int>();

        public FiveWReportSheetWriter(ExcelAdapter excel) : base(excel)
        {
            SetStyleIndexes();
        }

        private void SetStyleIndexes()
        {
            for (int i = 0; i < 12; i++)
            {
                _columnIndexToStyleIndex.Add(i, WrappedTextStyle);
            }
            _columnIndexToStyleIndex.Add(12, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(13, WrappedTextStyle);
            for (int i = 14; i < 18; i++)
            {
                _columnIndexToStyleIndex.Add(i, WrappedTextStyle);
            }
        }


        public async Task Write(List<FacilityViewModel> facilities, List<IndicatorSelectViewModel> indicators )
        {
            _indicators = indicators.OrderBy(a=>a.EntityDynamicColumnId).ToList();
            _facilities = facilities; 
            UInt32Value startRowIndex = 1;
            _columnPositionList = GetColumnPositionOfRows();
            OpenXmlReader reader = OpenXmlReader.Create(worksheetPart);
            OpenXmlWriter writer = OpenXmlWriter.Create(replacementPart);
            while (reader.Read())
            {
                if (reader.ElementType == typeof(SheetData))
                {
                    if (reader.IsEndElement)
                        continue;

                    writer.WriteElement(GetColumnsWithWidth()); // set column width
                    writer.WriteStartElement(new SheetData()); // start of sheet data
                    WriteHeaders(ref startRowIndex, writer);
                    WriteRecords(ref startRowIndex, writer);
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

            Sheet sheet = workbookPart.Workbook.Descendants<Sheet>()
                .Where(s => s.Id.Value.Equals(origninalSheetId)).FirstOrDefault();
            sheet.Id.Value = replacementPartId;
            workbookPart.DeletePart(worksheetPart);
        }

        private void WriteRecords(ref UInt32Value startRowIndex, OpenXmlWriter writer)
        {
            for (int i = 0; i < _facilities.Count; i++)
            {
                var facility = _facilities[i];
                StartOpenXmlRow(writer, startRowIndex);
                WriteFixedProperties(startRowIndex, facility, writer);
                WriteIndicators(startRowIndex, facility, writer);
                WriteReportingInfo(startRowIndex, facility, writer);
                WriteCalcultedProperties(startRowIndex, facility, writer);
                EndOpenXmlRow(writer);
                startRowIndex++;
            }
        }

        private void WriteIndicators(UInt32Value rowIndex, FacilityViewModel facility, OpenXmlWriter writer)
        {
            if(_indicators == null || _indicators.Count == 0) return;
            var enumerator = _indicatoroIdToColumnIndex.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var indicatorId = enumerator.Current;
                var indicator = facility.Properties.FirstOrDefault(p => p.EntityColumnId.Equals(indicatorId));
                if (indicator != null)
                {
                    var colIndex = _indicatoroIdToColumnIndex[indicatorId]; 
                    var cellReference = _columnPositionList[colIndex] + rowIndex;
                    uint styleIndex = GetDynamicPropertyStyleIndex(indicator.DataType);
                    object cellValue = GetTypesValue(indicator);
                    WriteOpenXmlCell(writer, cellReference, styleIndex, cellValue);
                }
            }
        }

        private object GetTypesValue(PropertiesInfo indicator)
        {
            switch (indicator.DataType)
            {
                case ColumnDataType.Integer:
                {
                    return Convert.ToInt32(indicator.Values[0]);
                }
                case ColumnDataType.Text:
                    return indicator.Values[0]; 
                case ColumnDataType.Decimal:
                    return Convert.ToDecimal(indicator.Values[0]); 
                case ColumnDataType.Datetime:
                    //return Convert.ToDateTime(indicator.Values[0]);
                    return indicator.Values[0];
                case ColumnDataType.Boolean:
                    return indicator.Values[0];
                case ColumnDataType.List:
                {
                    var cellValue = string.Join(",", indicator.Values);
                    return cellValue;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private uint GetDynamicPropertyStyleIndex(ColumnDataType dataType)
        {
            switch (dataType)
            {
                case ColumnDataType.Integer:
                    return NumberStyle;
                case ColumnDataType.Text:
                    return WrappedTextStyle;
                case ColumnDataType.Decimal:
                    return TwoDecimalStyle;
                case ColumnDataType.Datetime:
                    //return DateStyle; 
                    return WrappedTextStyle;
                case ColumnDataType.Boolean:
                    return WrappedTextStyle;
                case ColumnDataType.List:
                    return WrappedTextStyle; 
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }
        }

        private object GetFixedPropertyValue(int index, FacilityViewModel facility)
        {
            switch (index)
            {
                case 0:
                    return facility.UpazilaName;
                case 1:
                    return facility.UnionName;
                case 2:
                    return facility.CampName??"";
                case 3:
                    return facility.CampSSID??"";
                case 4:
                    return facility.ParaName??"";
                case 5:
                    return facility.ParaId??"";
                case 6:
                    return facility.BlockName??"";
                case 7:
                    return facility.FacilityName;
                case 8:
                    return facility.FacilityCode;
                case 9:
                    return facility.TargetedPopulation.GetDescription();//((TargetedPopulation) facility.TargetedPopulation).ToString();
                case 10:
                    return facility.FacilityType !=null?facility.FacilityType.GetDescription():"";
                case 11:
                    return facility.FacilityStatus!=null? ((FacilityStatus)facility.FacilityStatus).ToString() : "";
                case 12:
                    return facility.Latitude??"";
                case 13:
                    return facility.longitude??"";
                case 14:
                    return facility.Donors??"";
                case 15:
                    return facility.ProgrammingPartnerName;
                case 16:
                    return facility.ImplemantationPartnerName;
                case 17:
                    return facility.NonEducationPartner??""; 
            }

            return "";
        }
        private object GetReportingPersonyValue(int index, FacilityViewModel facility)
        {
            switch (index)
            {
                case 0:
                    return facility.ApproveByUserName;
                case 1:
                    return facility.ApproveByUserEmail;
                case 2:
                    return facility.ApproveByUserPhone;
                case 3:
                    return facility.ApproveDate;
            }

            return "";
        }
        private object GetCalculatedValue(int index,FacilityViewModel facilityViewModel)
        {
            switch (index)
            {
                case 0:
                    return facilityViewModel.Properties
                        .Where(a => FiveWReportConstants.AllRohingyaGirlsIndicator.Contains(a.EntityColumnId)
                        && decimal.TryParse(a.Values[0], out var val) && val > 0)
                        .Sum(a => Convert.ToDecimal(a.Values[0]));
                case 1:
                    return facilityViewModel.Properties
                      .Where(a => FiveWReportConstants.AllRohingyaBoysIndicator.Contains(a.EntityColumnId)
                      && decimal.TryParse(a.Values[0], out var val) && val > 0)
                      .Sum(a => Convert.ToDecimal(a.Values[0]));
                case 2:
                    return facilityViewModel.Properties
                       .Where(a => FiveWReportConstants.AllHostGirlsIndicator.Contains(a.EntityColumnId)
                       && decimal.TryParse(a.Values[0], out var val) && val > 0)
                       .Sum(a => Convert.ToDecimal(a.Values[0]));
                case 3:
                    return facilityViewModel.Properties
                       .Where(a => FiveWReportConstants.AllHostBoysIndicator.Contains(a.EntityColumnId)
                       && decimal.TryParse(a.Values[0], out var val) && val > 0)
                       .Sum(a => Convert.ToDecimal(a.Values[0]));
                case 4:
                    return facilityViewModel.Properties
                       .Where(a => FiveWReportConstants.AllEnrollmentIndicator.Contains(a.EntityColumnId)
                       && decimal.TryParse(a.Values[0], out var val) && val > 0)
                       .Sum(a => Convert.ToDecimal(a.Values[0]));
                case 5:
                    return facilityViewModel.Properties
                       .Where(a => FiveWReportConstants.AllTeachers.Contains(a.EntityColumnId)
                       && decimal.TryParse(a.Values[0], out var val) && val > 0)
                       .Sum(a => Convert.ToDecimal(a.Values[0]));
                case 6:
                    return facilityViewModel.ProgrammingPartnerName+"/"+facilityViewModel.ImplemantationPartnerName;
            }

            return "";
        }
        private void WriteFixedProperties(UInt32Value rowIndex, FacilityViewModel facility, OpenXmlWriter writer)
        {
            var enumerator = FiveWReportConstants.IndexToColumns.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var colIndex = enumerator.Current;
                var cellReference = _columnPositionList[colIndex] + rowIndex;
                var cellValue = GetFixedPropertyValue(colIndex, facility); 
                WriteOpenXmlCell(writer, cellReference, _columnIndexToStyleIndex[colIndex], cellValue);
            }
        }
        private void WriteCalcultedProperties(UInt32Value rowIndex, FacilityViewModel facility, OpenXmlWriter writer)
        {
            var enumerator = FiveWReportConstants.IndexToCalculatedField.Keys.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                //var colIndex = enumerator.Current;
                var colIndex = _calculatedFieldToColumnIndex[enumerator.Current];
                var cellReference = _columnPositionList[colIndex] + rowIndex;
                var cellValue = GetCalculatedValue(i, facility);
                WriteOpenXmlCell(writer, cellReference, WrappedTextStyle, cellValue);
                i++;
            }
        }
        private void WriteReportingInfo(UInt32Value rowIndex, FacilityViewModel facility, OpenXmlWriter writer)
        {
            var enumerator = FiveWReportConstants.IndexToReportingPerson.Keys.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
               // var colIndex = enumerator.Current;

                var colIndex = _reportingPersonToColumnIndex[enumerator.Current];
              
                var cellReference = _columnPositionList[colIndex] + rowIndex;
                var cellValue = GetReportingPersonyValue(i, facility);
                WriteOpenXmlCell(writer, cellReference, WrappedTextStyle, cellValue);
                i++;
            }
        }
        private void WriteHeaders(ref UInt32Value startRowIndex, OpenXmlWriter writer)
        {
            StartOpenXmlRow(writer, startRowIndex);
            var noOfColumns = FiveWReportConstants.FixedColumns.Count + _indicators.Count;
            var colIndex = 0;
            for (colIndex = 0; colIndex < FiveWReportConstants.FixedColumns.Count; colIndex++)
            {
                var cellReference = _columnPositionList[colIndex] + startRowIndex;
                WriteOpenXmlCell(writer, cellReference, HeaderStyle, FiveWReportConstants.FixedColumns[colIndex]);
            }

            colIndex = FiveWReportConstants.FixedColumns.Count;
            for (int i = 0; i < _indicators.Count; i++)
            {
                var cellReference = _columnPositionList[colIndex] + startRowIndex;
                var indicator = _indicators[i];
                WriteOpenXmlCell(writer, cellReference, HeaderStyle, indicator.IndicatorName);
                _indicatoroIdToColumnIndex.Add(indicator.EntityDynamicColumnId, colIndex);
                colIndex++; 
            }

            for (int i = 0; i < FiveWReportConstants.ReportingPersonInfo.Count; i++)
            {
                var cellReference = _columnPositionList[colIndex] + startRowIndex;
                WriteOpenXmlCell(writer, cellReference, HeaderStyle, FiveWReportConstants.ReportingPersonInfo[i]);
                _reportingPersonToColumnIndex.Add(i, colIndex);
                colIndex++;
            }
            for (int i = 0; i < FiveWReportConstants.CalculatedField.Count; i++)
            {
                var cellReference = _columnPositionList[colIndex] + startRowIndex;
                WriteOpenXmlCell(writer, cellReference, HeaderStyle, FiveWReportConstants.CalculatedField[i]);
                _calculatedFieldToColumnIndex.Add(i, colIndex);
                colIndex++;
            }
            EndOpenXmlRow(writer);
            startRowIndex++;
        }

        private List<string> GetColumnPositionOfRows()
        {
            List<string> columnPositionList = new List<string>();
            var noOfColumns = FiveWReportConstants.FixedColumns.Count + _indicators.Count + FiveWReportConstants.ReportingPersonInfo.Count+FiveWReportConstants.CalculatedField.Count;
            for (int i = 1; i <= noOfColumns; i++)
            {
                columnPositionList.Add(GetExcelTypeColumnString(i));
            }
            return columnPositionList;
        }


        private OpenXmlElement GetColumnsWithWidth()
        {
            UInt32Value columnNumber = 1;
            Columns columns = new Columns();
            for (int i = 0; i < FiveWReportConstants.FixedColumns.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            for (int i = 0; i < _indicators.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            for (int i = 0; i < FiveWReportConstants.ReportingPersonInfo.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }
            for (int i = 0; i < FiveWReportConstants.CalculatedField.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }
            return columns;
        }
    }
}
