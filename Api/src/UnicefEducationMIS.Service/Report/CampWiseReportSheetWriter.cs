using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Service.Report
{
    public class CampWiseReportSheetWriter : ExcelSheetDataWriter
    {
        private List<string> _columnPositionList;
        private List<FacilityViewModel> _facilities;
        private List<IndicatorSelectViewModel> _indicators;
        private List<TargetForCampWiseReport> _targetForCampWiseReports;

        private Dictionary<int, uint> _columnIndexToStyleIndex = new Dictionary<int, uint>();
        private Dictionary<long, int> _indicatoroIdToColumnIndex = new Dictionary<long, int>();
        private Dictionary<long, int> _reportingPersonToColumnIndex = new Dictionary<long, int>();
        private Dictionary<long, int> _calculatedFieldToColumnIndex = new Dictionary<long, int>();

        public CampWiseReportSheetWriter(ExcelAdapter excel) : base(excel)
        {
            SetStyleIndexes();
        }

        private void SetStyleIndexes()
        {
            for (int i = 0; i < 30; i++)
            {
                _columnIndexToStyleIndex.Add(i, WrappedTextStyle);
            }

        }


        public async Task Write(List<FacilityViewModel> facilities, List<TargetForCampWiseReport> targetForCamps)
        {
            _targetForCampWiseReports = targetForCamps;
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
            var data = _facilities.GroupBy(a => a.CampId).ToList();
            

            foreach (var item in data)
            {
                
                    StartOpenXmlRow(writer, startRowIndex);
                    WriteProperties(startRowIndex, item.ToList(), writer);

                    EndOpenXmlRow(writer);
                    startRowIndex++;
                
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
        private decimal getPropertiesSum(long columnId, List<FacilityViewModel> facilities)
        {
            return facilities.Sum(a => a.Properties
                    .Where(b => b.EntityColumnId.Equals(columnId)
                    && decimal.TryParse(b.Values[0], out var val) && val > 0).Select(c => Convert.ToDecimal(c.Values[0])).Sum());
        }
        private object GetPropertyValue(int index, List<FacilityViewModel> facility)
        {
            var campId = facility.Select(a => Convert.ToInt32(a.CampId)).FirstOrDefault();
            switch (index)
            {
                case 0:
                    return facility[0].CampName??"";
                case 1:
                    return string.Join(',', facility.Select(a => a.ProgrammingPartnerName).Distinct().ToArray());
                case 2:
                    return string.Join(',', facility.Select(a => a.ImplemantationPartnerName).Distinct().ToArray());
                case 3:
                    return facility.Count;
                case 4:
                    return _targetForCampWiseReports.Where(a => a.CampId == campId).Select(a => a.PeopleInNeed).Sum();
                case 5:
                    return _targetForCampWiseReports.Where(a => a.CampId == campId).Select(a => a.Target).Sum();
                case 6:
                    return facility.Sum(a => a.Properties
                   .Where(b => CampWiseReportConstants.OutReachCalculationIndicator.Contains(b.EntityColumnId)
                   && decimal.TryParse(b.Values[0], out var val) && val > 0).Select(c => Convert.ToDecimal(c.Values[0])).Sum());
                case 7:
                    return getPropertiesSum(CampWiseReportConstants.TotalFemaleRefugeeFacilitator, facility);
                case 8:
                    return getPropertiesSum(CampWiseReportConstants.TotalMaleRefugeeFacilitator, facility);
                case 9:
                    return getPropertiesSum(CampWiseReportConstants.TotalFemaleHostFacilitator, facility);
                case 10:
                    return getPropertiesSum(CampWiseReportConstants.TotalMaleHostFacilitator, facility);
                case 11:
                    return getPropertiesSum(CampWiseReportConstants.NoOfRohingyaGirlsLevel1, facility);
                case 12:
                    return getPropertiesSum(CampWiseReportConstants.NoOfRohingyaBoysLevel1, facility);
                case 13:
                    return getPropertiesSum(CampWiseReportConstants.NoOfRohingyaGirlsLevel2, facility);
                case 14:
                    return getPropertiesSum(CampWiseReportConstants.NoOfRohingyaBoysLevel2, facility);
                case 15:
                    return getPropertiesSum(CampWiseReportConstants.NoOfRohingyaGirlsLevel3, facility);
                case 16:
                    return getPropertiesSum(CampWiseReportConstants.NoOfRohingyaBoysLevel3, facility);
                case 17:
                    return getPropertiesSum(CampWiseReportConstants.NoOfRohingyaGirlsLevel4, facility);
                case 18:
                    return getPropertiesSum(CampWiseReportConstants.NoOfRohingyaBoysLevel4, facility);
                case 19:
                    return getPropertiesSum(CampWiseReportConstants.NoOfDrrAwarness, facility);
                case 20:
                    return getPropertiesSum(CampWiseReportConstants.NoOfLearningFacilityEstablished, facility);
                case 21:
                    return getPropertiesSum(CampWiseReportConstants.NoOfRohingyaFemaleCaregivers, facility);
                case 22:
                    return getPropertiesSum(CampWiseReportConstants.NoOfRohingyaMaleCaregivers, facility);
                case 23:
                    return getPropertiesSum(CampWiseReportConstants.NoOfHcFemaleCaregivers, facility);
                case 24:
                    return getPropertiesSum(CampWiseReportConstants.NoOfHcMaleCaregivers, facility);
                case 25:
                    return getPropertiesSum(CampWiseReportConstants.RohingyaGirlsEngagedSocialCohesion, facility);
                case 26:
                    return getPropertiesSum(CampWiseReportConstants.RohingyaBoysEngagedSocialCohesion, facility);
                case 27:
                    return getPropertiesSum(CampWiseReportConstants.ChildernBenefitingFromFood, facility);
                case 28:
                    return getPropertiesSum(CampWiseReportConstants.NoOfChildernBenefitingFromSchool, facility);
                case 29:
                    return getPropertiesSum(CampWiseReportConstants.ChildernBenefitingFood, facility);
            }

            return "";
        }

        private void WriteProperties(UInt32Value rowIndex, List<FacilityViewModel> facilities, OpenXmlWriter writer)
        {
            var enumerator = CampWiseReportConstants.IndexToColumns.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var colIndex = enumerator.Current;
                var cellReference = _columnPositionList[colIndex] + rowIndex;
                var cellValue = GetPropertyValue(colIndex, facilities);
                WriteOpenXmlCell(writer, cellReference, _columnIndexToStyleIndex[colIndex], cellValue);
            }
        }

        private void WriteHeaders(ref UInt32Value startRowIndex, OpenXmlWriter writer)
        {
            StartOpenXmlRow(writer, startRowIndex);
            var noOfColumns = CampWiseReportConstants.FixedColumns.Count;
            var colIndex = 0;
            for (colIndex = 0; colIndex < CampWiseReportConstants.FixedColumns.Count; colIndex++)
            {
                var cellReference = _columnPositionList[colIndex] + startRowIndex;
                WriteOpenXmlCell(writer, cellReference, HeaderStyle, CampWiseReportConstants.FixedColumns[colIndex]);
            }
            EndOpenXmlRow(writer);
            startRowIndex++;
        }

        private List<string> GetColumnPositionOfRows()
        {
            List<string> columnPositionList = new List<string>();
            var noOfColumns = CampWiseReportConstants.FixedColumns.Count;
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
            for (int i = 0; i < CampWiseReportConstants.FixedColumns.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            return columns;
        }
    }
}
