using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Reporting.Models;

namespace UnicefEducationMIS.Service.Report
{
    public class DuplicationReportWriter : ExcelSheetDataWriter
    {
        OpenXmlReader _reader;
        OpenXmlWriter _writer;
        UInt32Value _startRowIndex;


        

        public DuplicationReportWriter(ExcelAdapter excel) : base(excel)
        {
        }

        private const string FacilityName = "Facility Name";
        private const string FacilityId = "Facility ID";
        private const string CampName = "Camp";
        private const string ImplementationPartnerName = "Implementation partner name";
        private const string ProgrammingPartnerName = "Programming partner name";
        private const string BlockName = "Block";
        private const string UniqueStudents = "# of unique students";
        private const string DuplicateStudents = "# of duplicate students";

        private const string ProgressId = "Progress_id";
        private const string BeneficiaryName = "Name of the student";
        private const string FatherName = "Father's_Name";
        private const string MotherName = "Mother's_Name";
        private const string EnrollmentCount = "The student enrolled in how many LCs";
        private const string LevelOfStudy = "Level of study";

        private List<FailityWiseDuplicate> _facilityWiseDupliates;
        private List<StudentWiseDuplicate> _studentWiseDuplicates;

        private List<ExcelCell> _facilityWiseReportColumns;
        private List<ExcelCell> _studentWiseReportColumns;

        private void CreateColumns()
        {
            _facilityWiseReportColumns = new List<ExcelCell>()
            {
                new ReportCell(FacilityName, "FacilityName", WrappedTextStyle),
                new ReportCell(FacilityId, "FacilityCode", WrappedTextStyle),
                new ReportCell(ProgrammingPartnerName, "ProgrammingPartnerName", WrappedTextStyle),
                new ReportCell(ImplementationPartnerName, "ImplementationPartnerName", WrappedTextStyle),
                new ReportCell(CampName, "CampName", WrappedTextStyle),
                new ReportCell(BlockName, "BlockName", WrappedTextStyle),
                new ReportCell(UniqueStudents, "UniqueStudents", NumberStyle),
                new ReportCell(DuplicateStudents, "DuplicateStudents", NumberStyle)
            };

            _studentWiseReportColumns = new List<ExcelCell>()
            {
                new ReportCell(ProgressId, "ProgressId", WrappedTextStyle),
                new ReportCell(BeneficiaryName, "BeneficiaryName", WrappedTextStyle),
                new ReportCell(FatherName, "FatherName", WrappedTextStyle),
                new ReportCell(MotherName, "MotherName", WrappedTextStyle),
                new ReportCell(EnrollmentCount, "EnrollmentCount", WrappedTextStyle),               

                new ReportCell(FacilityId, "FacilityCode", WrappedTextStyle),
                new ReportCell(FacilityName, "FacilityName", WrappedTextStyle),
                new ReportCell(ProgrammingPartnerName, "ProgrammingPartnerName", WrappedTextStyle),
                new ReportCell(ImplementationPartnerName, "ImplementationPartnerName", WrappedTextStyle),
                new ReportCell(CampName, "CampName", WrappedTextStyle),
                new ReportCell(BlockName, "BlockName", WrappedTextStyle),
                new ReportCell(LevelOfStudy, "LevelOfStudy", WrappedTextStyle)
            };
        }


        public async Task Write(List<FailityWiseDuplicate> facilityWiseDupliates, List<StudentWiseDuplicate> studentWiseDuplicates)
        {
            CreateColumns();
            
            _facilityWiseDupliates = facilityWiseDupliates;
            _studentWiseDuplicates = studentWiseDuplicates;


            _startRowIndex = 1;
            
            _reader = OpenXmlReader.Create(worksheetPart);
             _writer = OpenXmlWriter.Create(replacementPart);
            while (_reader.Read())
            {
                if (_reader.ElementType == typeof(SheetData))
                {
                    if (_reader.IsEndElement)
                        continue;
                    var columns = GetColumnsWithWidth();
                    _writer.WriteElement(columns);
                    _writer.WriteStartElement(new SheetData());
                    WriteData();
                    _writer.WriteEndElement();
                }
                else
                {
                    if (_reader.IsStartElement)
                        _writer.WriteStartElement(_reader);
                    else if (_reader.IsEndElement)
                        _writer.WriteEndElement();
                }
            }
            _reader.Close();
            _writer.Close();

            var sheet = workbookPart.Workbook
                .Descendants<Sheet>().FirstOrDefault(s => s.Id.Value.Equals(origninalSheetId));
            if (sheet != null) sheet.Id.Value = replacementPartId;
            workbookPart.DeletePart(worksheetPart);
        }

        private void WriteData()
        {
            //WriteHeaders(_facilityWiseReportColumns);
            //foreach (var item in _facilityWiseDupliates)
            //{
            //    StartOpenXmlRow(_writer, _startRowIndex);
            //    WriteItem(item, _facilityWiseReportColumns);                
            //    EndOpenXmlRow(_writer);
            //    _startRowIndex++;
            //}
            //_startRowIndex += 2;
            StartOpenXmlRow(_writer, _startRowIndex);
            WriteOpenXmlCell(_writer, "A1", HeaderStyle, (object)"Duplication Report");
            EndOpenXmlRow(_writer);
            _startRowIndex += 2;

            WriteHeaders(_studentWiseReportColumns);
            foreach (var item in _studentWiseDuplicates)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, _studentWiseReportColumns);
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }
        }
   
        private void WriteItem<T>(T item, List<ExcelCell> columns)
        {
            var cellReferenceList = GetColumnPositions(columns);
            var t = item.GetType();
            var colIndex = 0;
            for (; colIndex < columns.Count; colIndex++)
            {
                var propInfo = t.GetProperty(columns[colIndex].ActualName);

                if (propInfo == null) continue;
                
                object cellValue = propInfo.GetValue(item);

                var cellReference = cellReferenceList[colIndex] + _startRowIndex;

                WriteOpenXmlCell(_writer, cellReference, columns[colIndex].Style, cellValue);
            }
        }

        private List<string> GetColumnPositions(List<ExcelCell> columns)
        {
            List<string> columnPositionList = new List<string>();
            var noOfColumns = columns.Count;
            for (int i = 1; i <= noOfColumns; i++)
            {
                columnPositionList.Add(GetExcelTypeColumnString(i));
            }
            return columnPositionList;
        }

        private void WriteHeaders(List<ExcelCell> columns)
        {
            var cellReferenceList = GetColumnPositions(columns);
            StartOpenXmlRow(_writer, _startRowIndex);
            var index = 0;
            for (; index < columns.Count; index++)
            {
                var cellReference = cellReferenceList[index] + _startRowIndex;
                WriteOpenXmlCell(_writer, cellReference, HeaderStyle, columns[index].DisplayName);
            }
            EndOpenXmlRow(_writer);
            _startRowIndex++;
        }

        private OpenXmlElement GetColumnsWithWidth()
        {
            UInt32Value columnNumber = 1;
            Columns columns = new Columns();
            for (int i = 0; i < _facilityWiseReportColumns.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            for (int i = 0; i < _facilityWiseReportColumns.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            return columns;
        }
    }
}
