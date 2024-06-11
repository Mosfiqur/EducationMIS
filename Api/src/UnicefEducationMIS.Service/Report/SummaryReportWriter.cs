using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Reporting.Models;

namespace UnicefEducationMIS.Service.Report
{
    public class SummaryReportWriter : ExcelSheetDataWriter
    {
        OpenXmlReader _reader;
        OpenXmlWriter _writer;
        UInt32Value _startRowIndex;
        List<string> _mergedCells = new List<string>();

        SummaryReport _summaryReport = new SummaryReport();

        public SummaryReportWriter(ExcelAdapter excel) : base(excel)
        {
            _startRowIndex = 1;
            _reader = OpenXmlReader.Create(worksheetPart);
            _writer = OpenXmlWriter.Create(replacementPart);


        }

        public async Task Write(SummaryReport summaryReport)
        {
            _summaryReport = summaryReport;
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
                    WriteMergedCells();
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
            WriteStudentEnrollmentSummary1();
            _startRowIndex += 2;
            WriteStudentEnrollmentSummary2();
            _startRowIndex += 2;
            WriteFacilitatorSummary();
            _startRowIndex += 2;
            WriteFacilityTypewiseStatus();
            _startRowIndex += 2;
            WriteRefugeeJrp();
            _startRowIndex += 2;
            WriteHostJrp();
            _startRowIndex += 2;
            WriteHostAndRefugeeJrpSummary();
            _startRowIndex += 2;
            WriteDisabilitySummary();
            _startRowIndex += 2;
            WriteCampwiseDisabilitySummary();
            _startRowIndex += 2;
            WriteWashSummary();
            _startRowIndex += 2;
            WriteStudyLevelwiseSummary();
            _startRowIndex += 2;
            WriteCareGiverSummary();
        }

        private void WriteStudentEnrollmentSummary1()
        {
            var rowIndex = _startRowIndex;
            var cellRefs = GetCellReferences(6, 1);

            var row2Titles = new List<string>() { "Children Age Group", "Rohingya", "", "Host", "", "" };
            var row3Titles = new List<string>() { "", "Female", "Male", "Female", "Male", "Total" };

            var row1 = new List<HeaderCell>()
            {
                new HeaderCell("Student enrollment Summary", cellRefs[0] + rowIndex),
                new HeaderCell("", cellRefs[1] + rowIndex),
                new HeaderCell("", cellRefs[2] + rowIndex),
                new HeaderCell("", cellRefs[3] + rowIndex),
                new HeaderCell("", cellRefs[4] + rowIndex),
                new HeaderCell("", cellRefs[5] + rowIndex),
            };
            _mergedCells.Add($"{cellRefs.First() + rowIndex}:{cellRefs.Last() + rowIndex}");
            rowIndex++;
            var row2 = row2Titles
                .Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex))
                .ToList();
            _mergedCells.Add($"{cellRefs[1] + rowIndex}:{cellRefs[2] + rowIndex}");
            _mergedCells.Add($"{cellRefs[3] + rowIndex}:{cellRefs[4] + rowIndex}");
            rowIndex++;
            var row3 = row3Titles
                .Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex))
                .ToList();

            WriteHeaders(row1);
            WriteHeaders(row2);
            WriteHeaders(row3);

            var columns = new List<ExcelCell>()
            {
                new ReportCell("", "AgeGroup", WrappedTextStyle),
                new ReportCell("", "RFemale", NumberStyle),
                new ReportCell("", "RMale", NumberStyle),
                new ReportCell("", "HFemale", NumberStyle),
                new ReportCell("", "HMale", NumberStyle),
            };

            var data = _summaryReport.EnrollmentSummary;

            var dataRow = _startRowIndex;
            foreach (var item in data)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, columns, 1);
                WriteOpenXmlCell(_writer, cellRefs.Last() + _startRowIndex, NumberStyle, "", $"=Sum({cellRefs.Second() + _startRowIndex}:{cellRefs.SecondLast() + _startRowIndex})");
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }

            StartOpenXmlRow(_writer, _startRowIndex);
            WriteOpenXmlCell(_writer, cellRefs.First() + _startRowIndex, WrappedTextStyle, "Total");
            cellRefs.Skip(1).ToList().ForEach(x =>
            {

                WriteOpenXmlCell(_writer, x + _startRowIndex, NumberStyle, "", $"=Sum({x + dataRow}:{x + (_startRowIndex - 1)})");
            });
            EndOpenXmlRow(_writer);
            _startRowIndex++;
        }

        private void WriteStudentEnrollmentSummary2()
        {
            var cellRefs = GetCellReferences(4, 1);
            var rowIndex = _startRowIndex;

            var row1 = new List<HeaderCell>()
            {
                new HeaderCell("Student enrollment Summary", cellRefs[0] + rowIndex),
                new HeaderCell("", cellRefs[1] + rowIndex),
                new HeaderCell("", cellRefs[2] + rowIndex),
                new HeaderCell("", cellRefs[3] + rowIndex),
            };
            _mergedCells.Add($"{cellRefs.First() + rowIndex}:{cellRefs.Last() + rowIndex}");
            rowIndex++;
            var row1Titles = new List<string>() { "Children Age Group", "Refugee", "Host", "Total" };
            var row2 = row1Titles
                .Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex))
                .ToList();

            WriteHeaders(row1);
            WriteHeaders(row2);

            var data = _summaryReport.EnrollmentSummary;

            var groupedData = data.GroupBy(x => x.AgeGroup)
                .Select(g => new EnrollmentSummaryWithoutGender
                {
                    AgeGroup = g.Key,
                    RefugeeTotal = g.Sum(x => x.RFemale) + g.Sum(x => x.RMale),
                    HostTotal = g.Sum(x => x.HFemale) + g.Sum(x => x.HMale),

                })
                .ToList();

            groupedData.ForEach(x => x.Total = x.RefugeeTotal + x.HostTotal);
            var columns = new List<ExcelCell>()
            {
                new ReportCell("", "AgeGroup", WrappedTextStyle),
                new ReportCell("", "RefugeeTotal", NumberStyle),
                new ReportCell("", "HostTotal", NumberStyle),
            };

            var dataRow = _startRowIndex;
            foreach (var item in groupedData)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, columns, 1);
                WriteOpenXmlCell(_writer, cellRefs.Last() + _startRowIndex, NumberStyle, "", $"=Sum({cellRefs.Second() + _startRowIndex}:{cellRefs.SecondLast() + _startRowIndex})");
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }

            StartOpenXmlRow(_writer, _startRowIndex);
            WriteOpenXmlCell(_writer, cellRefs.First() + _startRowIndex, WrappedTextStyle, "Total");
            cellRefs.Skip(1).ToList().ForEach(x =>
            {

                WriteOpenXmlCell(_writer, x + _startRowIndex, NumberStyle, "", $"=Sum({x + dataRow}:{x + (_startRowIndex - 1)})");
            });
            EndOpenXmlRow(_writer);
            _startRowIndex++;
        }

        private void WriteFacilitatorSummary()
        {
            var rowIndex = _startRowIndex;
            var cellRefs = GetCellReferences(5, 1);

            var header = new List<string>() { "Facilitators Summary", "", "", "", "" };
            _mergedCells.Add($"{cellRefs.First() + rowIndex}:{cellRefs.Last() + rowIndex}");

            var titles = new List<string>() { "Female Refugee Facilitator", "Male Refugee Facilitator", "Female Host Facilitator", "Male Host Facilitator", "Total" };

            var row1 = header.Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex)).ToList();
            rowIndex++;
            var row2 = titles
                .Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex))
                .ToList();

            WriteHeaders(row1);
            WriteHeaders(row2);

            var cells = new List<ExcelCell>()
            {
                new ReportCell("", "RFemale", NumberStyle),
                new ReportCell("", "RMale", NumberStyle),
                new ReportCell("", "HFemale", NumberStyle),
                new ReportCell("", "HMale", NumberStyle),
            };

            var data = _summaryReport.FacilitatorSummary;
            foreach (var item in data)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, cells, 1);
                WriteOpenXmlCell(_writer, cellRefs.Last() + _startRowIndex, NumberStyle, "", $"=Sum({cellRefs.First() + _startRowIndex}:{cellRefs.SecondLast() + _startRowIndex})");
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }

        }
        private void WriteFacilityTypewiseStatus()
        {
            var rowIndex = _startRowIndex;
            var cellRefs = GetCellReferences(2, 1);

            var header = new List<string>() { "Facility Type wise status", "" };
            _mergedCells.Add($"{cellRefs.First() + rowIndex}:{cellRefs.Last() + rowIndex}");
            var titles = new List<string>() { "Type", "Status (Completed)" };

            var row1 = header.Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex)).ToList();
            rowIndex++;
            var row2 = titles
                .Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex))
                .ToList();

            WriteHeaders(row1);
            WriteHeaders(row2);

            var columns = new List<ExcelCell>()
            {
                new ReportCell("", "Type", WrappedTextStyle),
                new ReportCell("", "Status", NumberStyle),
            };

            var data = _summaryReport.FacilityTypewiseStatus;

            var dataRow = _startRowIndex;
            foreach (var item in data)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, columns, 1);
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }
        }
        private void WriteJrp(List<string> row2Titles, List<JRP> data)
        {
            var rowIndex = _startRowIndex;
            var cellRefs = GetCellReferences(3, 1);

            var row1 = new List<HeaderCell>() {
                new HeaderCell($"JRP-{_summaryReport.Instance.DataCollectionDate.Year}", cellRefs[0] + rowIndex),
                new HeaderCell("", cellRefs[1] + rowIndex),
                new HeaderCell("", cellRefs[2] + rowIndex),
            };

            _mergedCells.Add($"{cellRefs.First() + rowIndex}:{cellRefs.Last() + rowIndex}");
            rowIndex++;

            var row2 = row2Titles
                .Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex))
                .ToList();
            rowIndex++;

            WriteHeaders(row1);
            WriteHeaders(row2);

            var columns = new List<ExcelCell>()
            {
                new ReportCell("", "AgeGroup", WrappedTextStyle),
                new ReportCell("", "Target", NumberStyle),
                new ReportCell("", "Reached", NumberStyle),
            };

            var dataRow = _startRowIndex;
            foreach (var item in data)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, columns, 1);
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }
        }
        private void WriteRefugeeJrp()
        {
            var row2Titles = new List<string>() { "Population Age group Refugee", "Refugee Target", "Refugee Reached" };
            var data = _summaryReport.RefugeeJrps;
            WriteJrp(row2Titles, data);
        }
        private void WriteHostJrp()
        {
            var row2Titles = new List<string>() { "Population Age group Host", "Host Target", "Host Reached" };
            var data = _summaryReport.HostJrps;
            WriteJrp(row2Titles, data);
        }

        private void WriteHostAndRefugeeJrpSummary()
        {
            var row2Titles = new List<string>() { "Population Age group", "Target", "Reached" };


            var summaryReportHostJrp = _summaryReport.HostJrps;
            summaryReportHostJrp.AddRange(_summaryReport.RefugeeJrps);
            var summary =
            summaryReportHostJrp.GroupBy(x => new { x.AgeGroupId, x.AgeGroup })
                .Select(g => new JRP
                {
                    Target = g.Sum(x => x.Target),
                    Reached = g.Sum(x => x.Reached),
                    AgeGroupId = g.Key.AgeGroupId,
                    AgeGroup = g.Key.AgeGroup
                }).ToList();

            WriteJrp(row2Titles, summary);
        }
        private void WriteDisabilitySummary()
        {
            var rowIndex = _startRowIndex;
            var cellRefs = GetCellReferences(6, 1);
            var row1Titles = new List<string>() { "Disability Summary", "", "", "", "", "" };

            var row1 = row1Titles
               .Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex))
               .ToList();
            _mergedCells.Add($"{cellRefs.First() + rowIndex}:{cellRefs.Last() + rowIndex}");
            rowIndex++;

            var row2Titles = new List<string>() { "Children Age Group", "Rohingya", "", "Host", "", "" };
            var row3Titles = new List<string>() { "", "Female", "Male", "Female", "Male", "Total" };

            var row2 = row2Titles
                .Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex))
                .ToList();
            _mergedCells.Add($"{cellRefs[1] + rowIndex}:{cellRefs[2] + rowIndex}");
            _mergedCells.Add($"{cellRefs[3] + rowIndex}:{cellRefs[4] + rowIndex}");
            rowIndex++;
            var row3 = row3Titles
                .Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex))
                .ToList();

            WriteHeaders(row1);
            WriteHeaders(row2);
            WriteHeaders(row3);

            var columns = new List<ExcelCell>()
            {
                new ReportCell("", "AgeGroup", WrappedTextStyle),
                new ReportCell("", "RFemale", NumberStyle),
                new ReportCell("", "RMale", NumberStyle),
                new ReportCell("", "HFemale", NumberStyle),
                new ReportCell("", "HMale", NumberStyle),
            };

            var data = _summaryReport.DisabilitySummary;

            var data1 = data.GroupBy(x => x.AgeGroup)
                .Select(g => new DisabilitySummary
                {
                    AgeGroup = g.Key,

                    HFemale = g.Sum(x => x.HFemale),
                    HMale = g.Sum(x => x.HMale),
                    RFemale = g.Sum(x => x.RFemale),
                    RMale = g.Sum(x => x.RMale)

                })
                .ToList();


            var dataRow = _startRowIndex;
            foreach (var item in data1)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, columns, 1);
                WriteOpenXmlCell(_writer, cellRefs.Last() + _startRowIndex, NumberStyle, "", $"=Sum({cellRefs.Second() + _startRowIndex}:{cellRefs.SecondLast() + _startRowIndex})");
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }

            StartOpenXmlRow(_writer, _startRowIndex);
            WriteOpenXmlCell(_writer, cellRefs.First() + _startRowIndex, WrappedTextStyle, "Total");
            cellRefs.Skip(1).ToList().ForEach(x =>
            {
                WriteOpenXmlCell(_writer, x + _startRowIndex, NumberStyle, "", $"=Sum({x + dataRow}:{x + (_startRowIndex - 1)})");
            });
            EndOpenXmlRow(_writer);
            _startRowIndex++;

            StartOpenXmlRow(_writer, _startRowIndex);
            WriteOpenXmlCell(_writer, cellRefs.First() + _startRowIndex, WrappedTextStyle, "Grand Total");
            WriteOpenXmlCell(_writer, cellRefs.Second() + _startRowIndex, NumberStyle, "", $"=Sum({cellRefs.Second() + (_startRowIndex - 1)}:{ cellRefs[2] + (_startRowIndex - 1)})");
            WriteOpenXmlCell(_writer, cellRefs[2] + _startRowIndex, NumberStyle, "");
            WriteOpenXmlCell(_writer, cellRefs[3] + _startRowIndex, NumberStyle, "", $"=Sum({cellRefs[3] + (_startRowIndex - 1)}:{ cellRefs[4] + (_startRowIndex - 1)})");
            WriteOpenXmlCell(_writer, cellRefs[4] + _startRowIndex, NumberStyle, "");

            EndOpenXmlRow(_writer);
            _mergedCells.Add($"{cellRefs.Second() + _startRowIndex}:{cellRefs[2] + _startRowIndex}");
            _mergedCells.Add($"{cellRefs[3] + _startRowIndex}:{cellRefs[4] + _startRowIndex}");
            _startRowIndex++;
        }

        private void WriteCampwiseDisabilitySummary()
        {

            var cellRefs = GetCellReferences(21, 1);
            StartOpenXmlRow(_writer, _startRowIndex);
            WriteOpenXmlCell(_writer, cellRefs.First() + _startRowIndex, HeaderStyle, "Disability Summary");
            var index = 1;
            cellRefs.Skip(1).ToList().ForEach(x =>
            {
                WriteOpenXmlCell(_writer, cellRefs[index] + _startRowIndex, HeaderStyle, "");
                index++;
            });
            EndOpenXmlRow(_writer);
            _mergedCells.Add($"{cellRefs.First() + _startRowIndex}:{cellRefs.Last() + _startRowIndex}");
            _startRowIndex++;

            var rowIndex = _startRowIndex;
            var ageGroups = new List<string>() { "3", "4 to 5", "6 to 14", "15 to 18", "19 to 24" };

            var row1Titles = new List<string>() { "Children Age Group", };
            var row2Titles = new List<string>();
            var row3Titles = new List<string>();

            var header = new List<string>() { "Disability Summary", "", "", "", "", "" };



            var mergCellIndex = 1;
            ageGroups.ForEach(ageGroup =>
            {
                row1Titles.Add(ageGroup);
                row1Titles.Add("");
                row1Titles.Add("");
                row1Titles.Add("");
                _mergedCells.Add($"{cellRefs[mergCellIndex] + rowIndex}:{cellRefs[mergCellIndex + 3] + rowIndex}");
                _mergedCells.Add($"{cellRefs[mergCellIndex] + (rowIndex + 1)}:{cellRefs[mergCellIndex + 1] + (rowIndex + 1)}");
                _mergedCells.Add($"{cellRefs[mergCellIndex + 2] + (rowIndex + 1)}:{cellRefs[mergCellIndex + 3] + (rowIndex + 1)}");
                mergCellIndex += 4;
                row2Titles.AddRange(new List<string>() { "Rohingya", "", "Host", "" });
                row3Titles.AddRange(new List<string>() { "Female", "Male", "Female", "Male" });
            });

            row2Titles.Insert(0, "Camp");
            row3Titles.Insert(0, "");

            var row1 = row1Titles
                .Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex))
                .ToList();
            rowIndex++;
            var row2 = row2Titles
                .Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex))
                .ToList();

            rowIndex++;
            var row3 = row3Titles
                .Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex))
                .ToList();

            WriteHeaders(row1);
            WriteHeaders(row2);
            WriteHeaders(row3);

            var columns = new List<ExcelCell>()
            {
                new ReportCell("", "RFemale", NumberStyle),
                new ReportCell("", "RMale", NumberStyle),
                new ReportCell("", "HFemale", NumberStyle),
                new ReportCell("", "HMale", NumberStyle),
            };

            var data = _summaryReport.DisabilitySummary;

            var camps = data.Select(x => x.CampName).Distinct().ToList();
            var ids = new List<int>() { 1, 2, 3, 4, 5 };

            var offset = 2;

            foreach (var camp in camps)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteOpenXmlCell(_writer, cellRefs[0] + _startRowIndex, WrappedTextStyle, camp);

                foreach (var ageGroup in ids)
                {
                    var item = data.FirstOrDefault(x => x.AgeGroupId == ageGroup && x.CampName == camp);
                    if (item == null)
                    {
                        item = new DisabilitySummary();
                    }
                    WriteItem(item, columns, offset);
                    offset += 4;
                }
                EndOpenXmlRow(_writer);
                _startRowIndex++;
                offset = 2;
            }
        }

        private void WriteWashSummary()
        {
            var rowIndex = _startRowIndex;
            var cellRefs = GetCellReferences(7, 1);

            var tittles = new List<string>() { "Wash summary table", "", "", "", "", "", "" };
            var row1 = tittles.Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex)).ToList();
            _mergedCells.Add($"{cellRefs.First() + rowIndex}:{cellRefs.Last() + rowIndex}");
            rowIndex++;


            var headers = new List<string>() { "Camp", "PP", "IP", "# of Functional Community Latrines near LF", "# of Latrines established for Boys", "# of Latrines established for Girls", "# of handwashing station established" };
            var row2 = headers.Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex)).ToList();
            rowIndex++;

            WriteHeaders(row1);
            WriteHeaders(row2);


            var columns = new List<ExcelCell>()
            {
                new ReportCell("", "CampName", WrappedTextStyle),
                new ReportCell("", "PP", WrappedTextStyle),
                new ReportCell("", "IP", WrappedTextStyle),
                new ReportCell("", "CommunityLatrines", NumberStyle),
                new ReportCell("", "BoysLatrines", NumberStyle),
                new ReportCell("", "GirlsLatrines", NumberStyle),
                new ReportCell("", "HandWashingStations", NumberStyle),
            };

            var data = _summaryReport.WashSummary;

            var dataRow = _startRowIndex;
            foreach (var item in data)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, columns, 1);
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }
        }

        private void WriteStudyLevelwiseSummary()
        {
            var rowIndex = _startRowIndex;
            var cellRefs = GetCellReferences(13, 1);

            var tittles = new List<string>() { "Study level/status wise summary table", "", "", "", "", "", "", "", "", "", "", "", "" };
            var row1 = tittles.Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex)).ToList();
            _mergedCells.Add($"{cellRefs.First() + rowIndex}:{cellRefs.Last() + rowIndex}");
            rowIndex++;


            var headers = new List<string>() {
                "PP",
                 "IP",
                 "# of rohingya girls in level-1",
                 "# of rohingya boys in level-1",
                 "# of rohingya girls in level-2",
                 "# of rohingya boys in level-2",
                 "# of rohingya girls in level-3",
                 "# of rohingya boys in level-3",
                 "# of rohingya girls in level-4",
                 "# of rohingya boys in level-4",
                 "Other study status",
                 "Total facility",
                 "Total enrollment"
            };
            var row2 = headers.Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex)).ToList();
            rowIndex++;

            WriteHeaders(row1);
            WriteHeaders(row2);


            var columns = new List<ExcelCell>()
            {
                new ReportCell("", "PP", WrappedTextStyle),
                new ReportCell("", "IP", WrappedTextStyle),
                new ReportCell("", "RGLevel1", NumberStyle),
                new ReportCell("", "RBLevel1", NumberStyle),
                new ReportCell("", "RGLevel2", NumberStyle),
                new ReportCell("", "RBLevel2", NumberStyle),
                new ReportCell("", "RGLevel3", NumberStyle),
                new ReportCell("", "RBLevel3", NumberStyle),
                new ReportCell("", "RGLevel4", NumberStyle),
                new ReportCell("", "RBLevel4", NumberStyle),
                new ReportCell("", "OtherStudyStatus", NumberStyle),
                new ReportCell("", "TotalFacility", NumberStyle),
                new ReportCell("", "TotalEnrollment", NumberStyle),
            };

            var data = _summaryReport.StudyLevelWiseSummary;

            var dataRow = _startRowIndex;
            foreach (var item in data)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, columns, 1);
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }
        }


        private void WriteCareGiverSummary()
        {
            var rowIndex = _startRowIndex;
            var cellRefs = GetCellReferences(15, 1);

            var tittles = new List<string>() { "Summary of DRR, Caregiver, social cohesion initiativesm etc", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            var row1 = tittles.Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex)).ToList();
            _mergedCells.Add($"{cellRefs.First() + rowIndex}:{cellRefs.Last() + rowIndex}");
            rowIndex++;

            var headers = new List<string>() {
                "Camp",
                 "PP",
                 "IP",
                 "Number of DRR awareness sessions conducted in HC (non-formal/community-based schools)",
                 "# of Rohingya Learning Facility Education Committees  established in rohingya camps",
                 "# of female rohingya caregivers sensitized on child/youth rights protection and parenting",
                 "# of male rohingya caregivers sensitized on child/youth rights protection and parenting",
                 "# of female HC caregivers sensitized on child/youth rights protection and parenting",
                 "# of male HC caregivers sensitized on child/youth rights protection and parenting",
                 "# of rohingya girls aged 3-24 years old engaged in social cohesion initiatives",
                 "# of rohingya boys aged 3-24 years old engaged in social cohesion initiatives",
                 "# of children benefitting from food",
                 "# of children benefitting from school/classroom/toilet rehabilitation",
                 "# of children benefitting food",
                 "Total facility"
            };
            var row2 = headers.Select((title, index) => new HeaderCell(title, cellRefs[index] + rowIndex)).ToList();
            rowIndex++;

            WriteHeaders(row1);
            WriteHeaders(row2);


            var columns = new List<ExcelCell>()
            {
                new ReportCell("", "CampName", NumberStyle),
                new ReportCell("", "PP", WrappedTextStyle),
                new ReportCell("", "IP", WrappedTextStyle),
                new ReportCell("", "NumOfEduCommitee", NumberStyle),
                new ReportCell("", "NumOfDrr", NumberStyle),
                new ReportCell("", "FemaleRCareGiver", NumberStyle),
                new ReportCell("", "MaleRCareGiver", NumberStyle),
                new ReportCell("", "FemaleHCareGiver", NumberStyle),
                new ReportCell("", "MaleHCareGiver", NumberStyle),
                new ReportCell("", "RGirls3_24", NumberStyle),
                new ReportCell("", "RBoys3_24", NumberStyle),
                new ReportCell("", "BenefittingFromFood", NumberStyle),
                new ReportCell("", "BenefittingFromClassRoom", NumberStyle),
                new ReportCell("", "BenefittingFood", NumberStyle),
                new ReportCell("", "TotalFacility", NumberStyle),
            };

            var data = _summaryReport.DrrCareGiverSummary;

            var dataRow = _startRowIndex;
            foreach (var item in data)
            {
                StartOpenXmlRow(_writer, _startRowIndex);
                WriteItem(item, columns, 1);
                EndOpenXmlRow(_writer);
                _startRowIndex++;
            }
        }

        private void WriteHeaders(List<HeaderCell> row)
        {
            StartOpenXmlRow(_writer, _startRowIndex);
            row.ForEach(cell =>
            {
                WriteOpenXmlCell(_writer, cell.CellReference, cell.Style, cell.Value);
            });

            EndOpenXmlRow(_writer);
            _startRowIndex++;
        }

        private void WriteItem<T>(T item, List<ExcelCell> columns, int columnOffset = 0)
        {
            List<string> cellReferenceList = GetCellReferences(columns.Count, columnOffset);


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

        private List<string> GetCellReferences(int noOfColumns, int columnOffset = 0)
        {
            List<string> columnPositionList = new List<string>();
            for (int i = columnOffset + 1; i <= columnOffset + noOfColumns; i++)
            {
                columnPositionList.Add(GetExcelTypeColumnString(i));
            }
            return columnPositionList;
        }

        private OpenXmlElement GetColumnsWithWidth()
        {
            UInt32Value columnNumber = 1;
            Columns columns = new Columns();
            for (int i = 0; i < 100; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            for (int i = 0; i < 100; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            return columns;
        }

        private void WriteMergedCells()
        {
            if (_mergedCells.Count > 0)
            {
                var mergeCells = new MergeCells();
                _mergedCells.ForEach(cellRef =>
                {
                    mergeCells.Append(new MergeCell()
                    {
                        Reference = cellRef,
                    });
                });
                _writer.WriteElement(mergeCells);
            }

        }


    }

}
