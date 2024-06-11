using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Reporting.Models;

namespace UnicefEducationMIS.Service.Report
{
    public class GapAnalysisReportWriter : ExcelSheetDataWriter
    {
        OpenXmlReader _reader;
        OpenXmlWriter _writer;
        UInt32Value _startRowIndex;

        public GapAnalysisReportWriter(ExcelAdapter excel) : base(excel)
        {
        }
              
        private const string CampName = "Camp";
        private const string Target = "Target";
        private const string Outreach = "Outreach";
        private const string Gap = "Gap";
        private const string Ratio = "Teacher student ratio";

        private List<GapAnalysisReport> _gapAnalysis;
        private List<ExcelCell> _columns;

        private void CreateColumns()
        {
            _columns = new List<ExcelCell>()
            {
                new ReportCell(CampName, "CampName", WrappedTextStyle),
                new ReportCell(Target, "Target", NumberStyle),
                new ReportCell(Outreach, "Outreach", NumberStyle),
                new ReportCell(Gap, "Gap", NumberStyle),
                new ReportCell(Ratio, "Ratio", WrappedTextStyle)
            };
        }


        public async Task Write(List<GapAnalysisReport> gapAnalysis)
        {
            CreateColumns();
            _gapAnalysis = gapAnalysis;
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
            WriteHeaders(_columns);
            foreach (var item in _gapAnalysis)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, _columns);
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
            for (int i = 0; i < _columns.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            for (int i = 0; i < _columns.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            return columns;
        }
    }
}
