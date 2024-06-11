using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Org.BouncyCastle.Asn1.X509;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public class FacilityVersionDataImportSheetsWriter : ExcelSheetDataWriter
    {
        private List<IndicatorSelectViewModel> _indicators;
        private List<FacilityViewModel> _facilities;
        private List<string> _columnPositionList;
        private readonly Dictionary<int, uint> _columnIndexToStyleIndex = new Dictionary<int, uint>();

        public FacilityVersionDataImportSheetsWriter(ExcelAdapter excel) : base(excel)
        {
            SetStyleIndexes();
        }

        private void SetStyleIndexes()
        {
            _columnIndexToStyleIndex.Add(0, NumberStyle);
            _columnIndexToStyleIndex.Add(1, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(2, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(3, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(4, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(5, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(6, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(7, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(8, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(9, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(10, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(11, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(12, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(13, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(14, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(15, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(16, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(17, WrappedTextStyle);
            _columnIndexToStyleIndex.Add(18, WrappedTextStyle);
        }

        public async Task Write(List<IndicatorSelectViewModel> indicators, List<FacilityViewModel> allFacilities)
        {
            _facilities = allFacilities.OrderBy(x=> x.Id).ToList();
            _indicators = indicators;
            UInt32Value startRowIndex = 1;
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

            Sheet sheet = workbookPart.Workbook
                .Descendants<Sheet>().FirstOrDefault(s => s.Id.Value.Equals(origninalSheetId));
            sheet.Id.Value = replacementPartId;
            workbookPart.DeletePart(worksheetPart);
        }

        private void WriteBeneficiaries(ref UInt32Value startRowIndex, OpenXmlWriter writer)
        {
            for (int i = 0; i < _facilities.Count; i++)
            {
                var facility = _facilities[i];
                StartOpenXmlRow(writer, startRowIndex);
                WriteBeneficiary(startRowIndex, facility, writer);
                EndOpenXmlRow(writer);
                startRowIndex++;
            }
        }

        private void WriteBeneficiary(UInt32Value startRowIndex, FacilityViewModel facility, OpenXmlWriter writer)
        {
            for (int colIndex = 0; colIndex < ImportTemplateConstants.FixedColumnsForFacilityVersionData.Count; colIndex++)
            {
                var cellReference = _columnPositionList[colIndex] + startRowIndex;
                object cellValue = GetBeneficiaryPropertyValue(colIndex, facility);
                WriteOpenXmlCell(writer, cellReference, _columnIndexToStyleIndex[colIndex], cellValue);
            }
        }

        private object GetBeneficiaryPropertyValue(in int colIndex, FacilityViewModel facility)
        {
            switch (colIndex)
            {
                case 0:
                    return facility.Id;
                case 1:
                    return facility.FacilityCode;
                case 2:
                    return facility.FacilityName;
                case 3:
                    return FromListValue((int) facility.TargetedPopulation, FacilityFixedIndicators.TargetedPopulation);
                case 4:
                    return facility.FacilityStatus != null ? FromListValue((int) facility.FacilityStatus, FacilityFixedIndicators.Status) : string.Empty;
                case 5:
                    return facility.Latitude;
                case 6:
                    return facility.longitude;
                case 7:
                    return facility.Donors;
                case 8:
                    return facility.NonEducationPartner;
                case 9:
                    return facility.ProgrammingPartnerName;
                case 10:
                    return facility.ImplemantationPartnerName;
                case 11:
                    return facility.Remarks;
                case 12:
                    return facility.UpazilaName;
                case 13:
                    return facility.UnionName;
                case 14:
                    return facility.CampSSID;
                case 15:
                    return facility.ParaName;
                case 16:
                    return facility.BlockCode;
                case 17:
                    return facility.TeacherEmail;
                case 18:
                    return facility.FacilityType != null ? FromListValue((int) facility.FacilityType, FacilityFixedIndicators.Type) : string.Empty;
                default:
                    return string.Empty;
            }
        }

        private object FromListValue(int selectedValue, long columnId)
        {
            var indicator =
                _indicators.FirstOrDefault(x => x.EntityDynamicColumnId == columnId);
            return indicator?.ListItems.FirstOrDefault(x => x.Value == selectedValue)?.Title;
        }

        private List<string> GetColumnPositions()
        {
            List<string> columnPositionList = new List<string>();
            var noOfColumns = ImportTemplateConstants.FixedColumnsForFacilityVersionData.Count + _indicators.Count;
            for (int i = 1; i <= noOfColumns; i++)
            {
                columnPositionList.Add(GetExcelTypeColumnString(i));
            }
            return columnPositionList;
        }

        private void WriteHeaders(ref UInt32Value startRowIndex, OpenXmlWriter writer)
        {
            StartOpenXmlRow(writer, startRowIndex);
            for (var index = 0;
                index < ImportTemplateConstants.FixedColumnsForFacilityVersionData.Count;
                index++)
            {
                var cellReference = _columnPositionList[index] + startRowIndex;
                WriteOpenXmlCell(writer, cellReference, HeaderStyle, ImportTemplateConstants.FixedColumnsForFacilityVersionData[index]);
            }

            var colIndex = ImportTemplateConstants.FixedColumnsForFacilityVersionData.Count;
            var orderedIndicators = _indicators
                .Where(x => !FacilityFixedIndicators.All().Contains(x.EntityDynamicColumnId))
                .OrderBy(x => x.ColumnOrder).ToList();

            for (int i = 0; i < orderedIndicators.Count; i++)
            {
                var cellReference = _columnPositionList[colIndex] + startRowIndex;
                var indicator = orderedIndicators[i];
                WriteOpenXmlCell(writer, cellReference, HeaderStyle, indicator.IndicatorName);
                colIndex++;
            }

            EndOpenXmlRow(writer);
            startRowIndex++;
        }


        private OpenXmlElement GetColumnsWithWidth()
        {
            UInt32Value columnNumber = 1;
            Columns columns = new Columns();
            for (int i = 0; i < ImportTemplateConstants.FixedColumnsForFacilityVersionData.Count; i++)
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