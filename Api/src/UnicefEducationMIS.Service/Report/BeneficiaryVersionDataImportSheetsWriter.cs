using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public class BeneficiaryVersionDataImportSheetsWriter : ExcelSheetDataWriter
    {
        private List<IndicatorSelectViewModel> _indicators;
        private List<BeneficiaryRawViewModel> _beneficiaries;
        private List<string> _columnPositionList;
        private Dictionary<int, uint> _columnIndexToStyleIndex = new Dictionary<int, uint>();

        public BeneficiaryVersionDataImportSheetsWriter(ExcelAdapter excel) : base(excel)
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
        }

        public async Task Write(List<IndicatorSelectViewModel> indicators, List<BeneficiaryRawViewModel> allBeneficiaries)
        {
            _beneficiaries = allBeneficiaries;
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

            var sheet = workbookPart.Workbook
                .Descendants<Sheet>().FirstOrDefault(s => s.Id.Value.Equals(origninalSheetId));
            if (sheet != null) sheet.Id.Value = replacementPartId;
            workbookPart.DeletePart(worksheetPart);
        }

        private void WriteBeneficiaries(ref UInt32Value startRowIndex, OpenXmlWriter writer)
        {
            for (int i = 0; i < _beneficiaries.Count; i++)
            {
                var beneficiary = _beneficiaries[i];
                StartOpenXmlRow(writer, startRowIndex);
                WriteBeneficiary(startRowIndex, beneficiary, writer);
                EndOpenXmlRow(writer);
                startRowIndex++;
            }
        }

        private void WriteBeneficiary(UInt32Value startRowIndex, BeneficiaryRawViewModel beneficiary, OpenXmlWriter writer)
        {
            for (int colIndex = 0; colIndex < ImportTemplateConstants.FixedColumnsForBeneficiaryVersionData.Count; colIndex++)
            {
                var cellReference = _columnPositionList[colIndex] + startRowIndex;
                object cellValue = GetBeneficiaryPropertyValue(colIndex, beneficiary);
                WriteOpenXmlCell(writer, cellReference, _columnIndexToStyleIndex[colIndex], cellValue);
            }
        }

        private object GetBeneficiaryPropertyValue(in int colIndex, BeneficiaryRawViewModel beneficiary)
        {
            switch (colIndex)
            {
                case 0:
                    return beneficiary.BeneficiaryId;
                case 1:
                    return beneficiary.FacilityCode;
                case 2:
                    return beneficiary.FacilityName;
                case 3:
                    return beneficiary.UnhcrId;
                case 4:
                    return beneficiary.BeneficiaryName;
                case 5:
                    return beneficiary.FatherName;
                case 6:
                    return beneficiary.MotherName;
                case 7:
                    return beneficiary.FCNId;
                case 8:
                    return beneficiary.DateOfBirth;
                case 9:
                    return FromListValue((int) beneficiary.Sex, BeneficairyFixedColumns.Sex);
                case 10:
                    return beneficiary.Disabled ? "Yes" : "No";
                case 11:
                    return FromListValue((int)beneficiary.LevelOfStudy, BeneficairyFixedColumns.LevelOfStudy);
                case 12:
                    return beneficiary.EnrollmentDate;
                case 13:
                    return beneficiary.CampSsId;
                case 14:
                    return beneficiary.BlockCode;
                case 15:
                    return beneficiary.SubBlockName;
                case 16:
                    return beneficiary.Remarks;
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
            var noOfColumns = ImportTemplateConstants.FixedColumnsForBeneficiaryVersionData.Count + _indicators.Count;
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
                index < ImportTemplateConstants.FixedColumnsForBeneficiaryVersionData.Count;
                index++)
            {
                var cellReference = _columnPositionList[index] + startRowIndex;
                WriteOpenXmlCell(writer, cellReference, HeaderStyle, ImportTemplateConstants.FixedColumnsForBeneficiaryVersionData[index]);
            }

            var colIndex = ImportTemplateConstants.FixedColumnsForBeneficiaryVersionData.Count;
            var orderedIndicators = _indicators
                .Where(x => !BeneficairyFixedColumns.Basic().Contains(x.EntityDynamicColumnId))
                .OrderBy(x => x.ColumnOrder)
                .ToList();
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
            for (int i = 0; i < ImportTemplateConstants.FixedColumnsForBeneficiaryVersionData.Count; i++)
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