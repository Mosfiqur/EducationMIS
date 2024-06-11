using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Reporting.Models;

namespace UnicefEducationMIS.Service.Report
{
    public class DamageReportWriter : ExcelSheetDataWriter
    {
        OpenXmlReader _reader;
        OpenXmlWriter _writer;
        UInt32Value _startRowIndex;

        private DamageSummary _damageSummary;
        private List<PartnerWiseDamage> _partnerWiseDamages;
        private List<YearlyDamageSummary> _yearlyDamages;

        private List<ExcelCell> _partnerWiseDamageColumns;
        private List<ExcelCell> _yearlyDamageColumns;

        public DamageReportWriter(ExcelAdapter excel) : base(excel)
        {
            _startRowIndex = 1;
            _reader = OpenXmlReader.Create(worksheetPart);
            _writer = OpenXmlWriter.Create(replacementPart);
            CreateColumns();
        }

        private void CreateColumns()
        {

            _partnerWiseDamageColumns = new List<ExcelCell>()
            {
                new ReportCell("Partner(PP)", "PpName", WrappedTextStyle),
                new ReportCell("Partner(IP)", "IpName", WrappedTextStyle),
                new ReportCell("LCs affected ({0})", "Affected", NumberStyle),
                new ReportCell("LCs repair work started ({0})", "RepairStarted", NumberStyle),
                new ReportCell("LCs repair work finished ({0})", "RepairFinished", NumberStyle),
                new ReportCell("LCs repair work ongoing ({0})", "RepairOngoing", NumberStyle),
            };

            _yearlyDamageColumns = new List<ExcelCell>()
            {
                 new ReportCell("Facility Count", "NumOfFacility", NumberStyle),
                 new ReportCell("Damage occurance", "TimesDamageOccured", NumberStyle),
            };
        }


        public async Task Write(DamageSummary damageSummary, List<PartnerWiseDamage> partnerWiseDamages, 
            List<YearlyDamageSummary> yearlyDamages)
        {
            _partnerWiseDamageColumns.ForEach(x => x.DisplayName = string.Format(x.DisplayName, damageSummary.InstanceName));
            _damageSummary = damageSummary;
            _partnerWiseDamages = partnerWiseDamages;
            _yearlyDamages = yearlyDamages;

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
            WriteDamageSummary();
                        
            WriteHeaders(_partnerWiseDamageColumns, 1);
            foreach (var item in _partnerWiseDamages)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, _partnerWiseDamageColumns, 1);
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }
            
            _startRowIndex += 2;

            WriteHeaders(_yearlyDamageColumns, 1);
            foreach (var item in _yearlyDamages)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, _yearlyDamageColumns, 1);
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }

        }

        private void WriteDamageSummary()
        {
            List<string> values = new List<string>()
            {
                $"Total {_damageSummary.Affected} LFs affected",
                $"Total {_damageSummary.RepairStarted} LFs repair work started",
                $"Total {_damageSummary.RepairFinished} LFs repair work finished",
                $"Total {_damageSummary.RepairOngoing} LFs repair work ongoing"
            };

            List<string> cumulatives = new List<string>()
            {
                $"Total {_damageSummary.TotalAffected} LCs Affected",
                $"Total {_damageSummary.TotalRepaired} LCs Repaired"
            };

            var head1 = new ReportCell(string.Format("Damage Tracker ({0}) summary information", _damageSummary.InstanceName), "");
            WriteHeaders(new List<ExcelCell>() { head1 });
            _startRowIndex++;

            foreach (var val in values)
            {

                StartOpenXmlRow(_writer, _startRowIndex);
                WriteOpenXmlCell(_writer, string.Concat("A", _startRowIndex), WrappedTextStyle, val);
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }
            _startRowIndex++;

            var head2 = new ReportCell($"Cumulative Status till {_damageSummary.InstanceName}", "");
            WriteHeaders(new List<ExcelCell>() { head2 });

            foreach (var val in cumulatives)
            {

                StartOpenXmlRow(_writer, _startRowIndex);
                WriteOpenXmlCell(_writer, string.Concat("A", _startRowIndex), WrappedTextStyle, val);
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }

            _startRowIndex++;
        }

        private void WriteItem<T>(T item, List<ExcelCell> columns, int columnOffset = 0)
        {
            var cellReferenceList = GetColumnPositions(columns, columnOffset);
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

        private List<string> GetColumnPositions(List<ExcelCell> columns, int columnOffset = 0)
        {
            List<string> columnPositionList = new List<string>();
            var noOfColumns = columns.Count;
            for (int i = columnOffset + 1; i <= columnOffset + noOfColumns; i++)
            {
                columnPositionList.Add(GetExcelTypeColumnString(i));
            }
            return columnPositionList;
        }

        private void WriteHeaders(List<ExcelCell> columns, int columnOffset = 0)
        {
            var cellReferenceList = GetColumnPositions(columns, columnOffset);
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
            columns.Append(new Column() { CustomWidth = true, Width = 25, Min = 1, Max = 1 });
            for (int i = 0; i < _yearlyDamageColumns.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            for (int i = 0; i < _partnerWiseDamageColumns.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            return columns;
        }
    }
}
