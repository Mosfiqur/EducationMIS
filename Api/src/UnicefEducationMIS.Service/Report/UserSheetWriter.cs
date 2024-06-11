using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public class UserSheetWriter : ExcelSheetDataWriter
    {

        private int _totalColumns = 0;
        private List<UserViewModel> _users = new List<UserViewModel>();
        private List<string> _columnPositionList;
        private List<ReportCell> _fixedColumns = new List<ReportCell>();
        private List<DynamicPropertiesViewModel> _dynamicColumns = new List<DynamicPropertiesViewModel>();


        public UserSheetWriter(ExcelAdapter excel) : base(excel)
        {
        }

        public async Task Write(List<UserViewModel> users, List<ReportCell> fixedColumns, List<DynamicPropertiesViewModel> dynamicColumns)
        {
            UInt32Value startRowIndex = 1;
            _fixedColumns.AddRange(fixedColumns);
            _users.AddRange(users);
            _dynamicColumns.AddRange(dynamicColumns);
            _totalColumns = _fixedColumns.Count + _dynamicColumns.Count;

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
                    WriteItems(ref startRowIndex, writer);
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

        private void WriteItems(ref UInt32Value startRowIndex, OpenXmlWriter writer)
        {
            foreach (var facility in _users)
            {
                StartOpenXmlRow(writer, startRowIndex);
                WriteItem(startRowIndex, facility, writer);
                EndOpenXmlRow(writer);
                startRowIndex++;
            }
        }

        private void WriteItem(UInt32Value startRowIndex, UserViewModel user, OpenXmlWriter writer)
        {
            var t = user.GetType();
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
                    cellValue = propInfo.GetValue(user)?.ToString();
                }
                else if (propInfo.PropertyType == typeof(bool))
                {
                    cellValue = propInfo.GetValue(user);
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
                    cellValue = propInfo.GetValue(user);
                }

                var cellReference = _columnPositionList[colIndex] + startRowIndex;
                WriteOpenXmlCell(writer, cellReference, _fixedColumns[colIndex].Style, cellValue);
            });

        

            _dynamicColumns
                .ForEach(cell =>
                {
                    colIndex++;
                    var cellReference = _columnPositionList[colIndex] + startRowIndex;

                    var property = user.DynamicCells.FirstOrDefault(x => x.EntityDynamicColumnId == cell.Id);
                    if (property == null)
                    {
                        WriteOpenXmlCell(writer, cellReference, GetStyleForColumn(cell), string.Empty);
                        return;
                    }

                    WriteOpenXmlCell(writer, cellReference, GetStyleForColumn(cell), string.Join(",", property.Values));
                });
        }

        private uint GetStyleForColumn(DynamicPropertiesViewModel indicator)
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
            int index = 0;
            for (; index < _fixedColumns.Count; index++)
            {
                var cellReference = _columnPositionList[index] + startRowIndex;
                WriteOpenXmlCell(writer, cellReference, HeaderStyle, _fixedColumns[index].DisplayName);
            }

            for (; index < _totalColumns; index++)
            {
                var cellReference = _columnPositionList[index] + startRowIndex;
                var indicator = _dynamicColumns[index - _fixedColumns.Count];
                WriteOpenXmlCell(writer, cellReference, HeaderStyle, indicator.Name);
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

            for (int i = 0; i < _dynamicColumns.Count; i++)
            {
                columns.Append(new Column() { CustomWidth = true, Width = 25, Min = columnNumber, Max = columnNumber });
                columnNumber++;
            }

            return columns;
        }

    }
}