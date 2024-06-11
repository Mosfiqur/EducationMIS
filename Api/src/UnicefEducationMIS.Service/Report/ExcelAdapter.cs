using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using UnicefEducationMIS.Service.Import;

namespace UnicefEducationMIS.Service.Report
{
    public class ExcelAdapter : IDisposable
    {
        private readonly SharedStrings _sharedStrings;
        private string _filePath;
        private ExcelAdapter(Stream stream)
        {
            FileStream = stream;
            Document = SpreadsheetDocument.Open(stream, true);
            _sharedStrings = new SharedStrings(Document);
        }

        private ExcelAdapter(string filePath)
        {
            _filePath = filePath;
            Document = SpreadsheetDocument.Open(filePath, true);
            _sharedStrings = new SharedStrings(Document);
        }

        public SpreadsheetDocument Document { get; private set; }

        private Stream FileStream { get; set; }


        public void Dispose()
        {
            if (Document != null)
                Document.Dispose();
            if (FileStream != null)
                FileStream.Close();
        }

        public void Save(string saveLocation)
        {
            if (File.Exists(saveLocation))
                File.Delete(saveLocation);

            Document.WorkbookPart.SharedStringTablePart.SharedStringTable.Save();
            Document.WorkbookPart.Workbook.Save();
            Document.Dispose();
            FileStream.Position = 0;

            using (var fs = File.Create(saveLocation))
            {
                FileStream.CopyTo(fs);
            }
        }

        public WorkSheetAdapter AddWorkSheet(string sheetName, int sheetIndex)
        {
            var worksheetPart = Document.WorkbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = Document.WorkbookPart.Workbook.GetFirstChild<Sheets>();

            string relationshipId = Document.WorkbookPart.GetIdOfPart(worksheetPart);
            var sheetId = ExcelHelper.GetSheetId(sheets);
            var sheet = new Sheet { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.InsertAt(sheet, sheetIndex);
            worksheetPart.Worksheet.Save();
            Document.WorkbookPart.Workbook.Save();
            return new WorkSheetAdapter(this, worksheetPart);
        }

        public void DeleteWorkSheet(string sheetName)
        {
            var sheet = Document.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
            if (sheet == null)
                return;
            Document.WorkbookPart.DeletePart(ExcelHelper.GetWorkSheetPartByName(Document, sheetName));
            sheet.Remove();
            var nameDefinations = Document.WorkbookPart.Workbook.Descendants<DefinedName>();
            var nameDef = nameDefinations.FirstOrDefault(d => d.Text.Contains(sheetName));
            if (nameDef != null)
            {
                nameDef.Remove();
            }
            Document.WorkbookPart.Workbook.Save();
        }

        public void ReadStyles()
        {
            ExcelStyles.Instance.CleareStyles();
            var workSheetAdapter = GetWorkSheet("StyleSheet");
            var styleNameColumn = "A";
            var styleColumn = "B";
            var rowIndex = 1;
            var styleNameCellRef = string.Empty;
            var styleCellRef = string.Empty;
            var containsStyle = true;
            Cell styleNameCell = null;
            Cell styleCell = null;
            uint sharedStringIndex;
            while (containsStyle)
            {
                styleNameCellRef = styleNameColumn + rowIndex;
                styleCellRef = styleColumn + rowIndex;
                styleNameCell = workSheetAdapter.GetCell(styleNameCellRef);
                if (styleNameCell == null)
                    break;
                if (styleNameCell.CellValue == null)
                    break;

                styleCell = workSheetAdapter.GetCell(styleCellRef);
                sharedStringIndex = uint.Parse(styleNameCell.CellValue.Text);
                var styleName = GetSharedString(sharedStringIndex);
                ExcelStyles.Instance.Add(styleName, styleCell.StyleIndex);
                rowIndex++;
            }
        }

        public WorkSheetAdapter GetWorkSheet(string sheetName)
        {
            var worksheetPart = ExcelHelper.GetWorkSheetPartByName(Document, sheetName);
            return new WorkSheetAdapter(this, worksheetPart);
        }

        public UInt32Value InsertSharedString(string stringItem)
        {
            return _sharedStrings.Get(stringItem);
        }

        public string GetSharedString(uint index)
        {
            return _sharedStrings.GetString(index);
        }

        public static ExcelAdapter Open(string filePath)
        {
            if (File.Exists(filePath) == false)
                throw new FileNotFoundException(Path.GetFileName(filePath));

            var ms = CommonUtil.GetDisconnectedMemoryStream(filePath);
            return new ExcelAdapter(ms);
        }

        public byte[] GetBytes()
        {
            Document.WorkbookPart.SharedStringTablePart.SharedStringTable.Save();
            Document.WorkbookPart.Workbook.Save();
            Document.Dispose();
            if (FileStream != null)
            {
                FileStream.Position = 0;
                return ExcelHelper.GetFileAsBytes(FileStream);
            }
            return File.ReadAllBytes(_filePath);
        }
    }
}
