using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using UnicefEducationMIS.Core.Extensions;
using UnicefEducationMIS.Service.Import;

namespace UnicefEducationMIS.Service.Report
{
    public class ExcelSheetDataWriter
    {
        protected readonly ExcelAdapter _excel;
        private StringBuilder _stringBuilder;
        protected WorkbookPart workbookPart;
        protected WorksheetPart worksheetPart, replacementPart;
        protected string origninalSheetId, replacementPartId;
        protected uint HeaderStyle;
        protected uint WrappedTextStyle;
        protected uint NumberStyle;
        protected uint DateStyle;
        protected uint TwoDecimalStyle;
        protected uint SixDecimalStyle;

        public ExcelSheetDataWriter(ExcelAdapter excel)
        {
            _excel = excel;
            workbookPart = _excel.Document.WorkbookPart;
            worksheetPart = workbookPart.WorksheetParts.Last();
            origninalSheetId = workbookPart.GetIdOfPart(worksheetPart);
            replacementPart = workbookPart.AddNewPart<WorksheetPart>();
            replacementPartId = workbookPart.GetIdOfPart(replacementPart);
            ReadStyleIndexes();
        }

        protected XmlWriter GetXmlWriter()
        {
            _stringBuilder = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment,
            };

            return XmlWriter.Create(_stringBuilder, settings);
        }

        protected string GetXmlString()
        {
            return _stringBuilder.ToString();
        }

        protected void CreateRowStart(uint rowIndex, XmlWriter writer, int rowLength)
        {
            writer.WriteStartElement("x", "row", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
            writer.WriteAttributeString("r", rowIndex.ToString());
            writer.WriteAttributeString("spans", string.Format("1:{0}", rowLength));
            //writer.WriteAttributeString(XmlConvert.EncodeName("x14ac:dyDescent"), "0.25");
        }
        protected void CreateRowEnd(XmlWriter writer)
        {
            //Create a Row END
            writer.WriteEndElement();
        }
        protected void WriteCell(XmlWriter writer, string cellName, UInt32Value styleIndex, Object cellValue)
        {
            writer.WriteStartElement("x", "c", ExcelConstants.OpenXmlSchemaDefination);
            writer.WriteAttributeString("r", cellName);
            writer.WriteAttributeString("s", styleIndex);
            if (cellValue != null)
            {
                var cellDataType = ExcelHelper.GetCellDataType(cellValue);
                if (cellDataType == CellValues.SharedString)
                {
                    var sharedStrIndex = _excel.InsertSharedString(cellValue.ToString());
                    writer.WriteAttributeString("t", "s");
                    writer.WriteElementString("x", "v", ExcelConstants.OpenXmlSchemaDefination, sharedStrIndex);
                }
                else
                {
                    writer.WriteElementString("x", "v", ExcelConstants.OpenXmlSchemaDefination, cellValue.ToString());
                }
            }
            writer.WriteEndElement();
        }

        protected void WriteCellByIndex(XmlWriter writer, int colIndex, uint rowIndex, UInt32Value styleIndex, Object cellValue)
        {
            string cellName = GetCellName(colIndex, rowIndex);
            writer.WriteStartElement("x", "c", ExcelConstants.OpenXmlSchemaDefination);
            writer.WriteAttributeString("r", cellName);
            writer.WriteAttributeString("s", styleIndex);
            if (cellValue != null)
            {
                var cellDataType = ExcelHelper.GetCellDataType(cellValue);
                if (cellDataType == CellValues.SharedString)
                {
                    var sharedStrIndex = _excel.InsertSharedString(cellValue.ToString());
                    writer.WriteAttributeString("t", "s");
                    writer.WriteElementString("x", "v", ExcelConstants.OpenXmlSchemaDefination, sharedStrIndex);
                }
                else
                {
                    writer.WriteElementString("x", "v", ExcelConstants.OpenXmlSchemaDefination, cellValue.ToString());
                }
            }
            writer.WriteEndElement();
        }
        public static string GetExcelTypeColumnString(int value)
        {
            string result = string.Empty;
            while (--value >= 0)
            {
                result = (char)('A' + value % 26) + result;
                value /= 26;
            }
            return result;
        }

        protected XmlWriter GetWriter()
        {
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment,
            };

            return XmlWriter.Create(sb, settings);
        }
        protected void WriteEmptyRow(XmlWriter writer, uint rowIndex, int rowLength)
        {
            CreateRowStart(rowIndex, writer, rowLength);

            for (var i = 1; i <= rowLength; i++)
            {
                WriteCell(writer, GetCellName(i, rowIndex), UInt32Value.FromUInt32(1), null);
            }
            CreateRowEnd(writer);
        }

        protected void WriteEmptyRow(XmlWriter writer, uint rowIndex, int rowLength, uint styleIndex)
        {
            CreateRowStart(rowIndex, writer, rowLength);

            for (var i = 1; i <= rowLength; i++)
            {
                WriteCell(writer, GetCellName(i, rowIndex), UInt32Value.FromUInt32(styleIndex), null);
            }
            CreateRowEnd(writer);
        }

        protected string GetCellName(string col, uint rowIndex)
        {
            return string.Format(col + "{0}", rowIndex);
        }
        protected string GetCellName(int colIndex, uint rowIndex)
        {
            var col = GetExcelTypeColumnString(colIndex);
            return string.Format(col + "{0}", rowIndex);
        }

        public void WriteColumn(XmlWriter writer, int min, int max, int width)
        {
            writer.WriteStartElement("x", "col", ExcelConstants.OpenXmlSchemaDefination);
            writer.WriteAttributeString("min", min.ToString());
            writer.WriteAttributeString("max", max.ToString());
            writer.WriteAttributeString("width", width.ToString());
            writer.WriteEndElement();
        }

        protected void StartOpenXmlRow(OpenXmlWriter writer, UInt32Value rowNumber)
        {
            writer.WriteStartElement(new Row() { RowIndex = rowNumber });
        }

        protected void EndOpenXmlRow(OpenXmlWriter writer)
        {
            writer.WriteEndElement();
        }

        protected void WriteOpenXmlCell(OpenXmlWriter writer, string cellReference, UInt32Value styleIndex, Object cellValue, string formula = null)
        {


            Cell cell = new Cell()
            {
                StyleIndex = styleIndex,
                CellReference = cellReference,                                       
            };

            if(formula != null)
            {
                cell.CellFormula = new CellFormula(formula);
            }

            if (cellValue != null)
            {
                var cellDataType = ExcelHelper.GetCellDataType(cellValue);
                var stringValue = cellValue.ToStringValue();
                if (cellDataType == CellValues.Number)
                {
                    cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                    cell.CellValue = new CellValue(stringValue);
                }
                else
                {
                    var sharedStrIndex = _excel.InsertSharedString(stringValue);
                    cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                    cell.CellValue = new CellValue(sharedStrIndex);
                }
            }
            writer.WriteElement(cell);
        }

        protected void ReadStyleIndexes()
        {
            HeaderStyle = ExcelStyles.Instance.Get(ReportConstants.HeaderCellStyleName);
            WrappedTextStyle = ExcelStyles.Instance.Get(ReportConstants.WrappedTextStyleName);
            DateStyle = ExcelStyles.Instance.Get(ReportConstants.DateStyleName);
            NumberStyle = ExcelStyles.Instance.Get(ReportConstants.IntegerStyleName);
            TwoDecimalStyle = ExcelStyles.Instance.Get(ReportConstants.TwoDecimalStyleName);
            SixDecimalStyle = ExcelStyles.Instance.Get(ReportConstants.SixDecimalStyleName);
        }
    }
}
