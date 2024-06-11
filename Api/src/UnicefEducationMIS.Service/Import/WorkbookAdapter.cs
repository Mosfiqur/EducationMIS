using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace UnicefEducationMIS.Service.Import
{
    public class WorkbookAdapter:IDisposable
    {
        private SpreadsheetDocument _excelDoc;
        private List<SharedStringItem> _sharedStringItems;


        public WorkbookAdapter(Stream fs)
        {
            Initialize(fs);
        }

        public WorkbookAdapter(string filePath)
        {
            Initialize(filePath);
        }


        private void Initialize(Stream fs)
        {
            _excelDoc = SpreadsheetDocument.Open(fs, true);
            PrepareStringTable();
        }
        private void Initialize(string filePath)
        {
            _excelDoc = SpreadsheetDocument.Open(filePath, false);
            PrepareStringTable();
        }

        private void PrepareStringTable()
        {
            var stringtable = _excelDoc.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            if (stringtable != null)
                _sharedStringItems = stringtable.SharedStringTable.Elements<SharedStringItem>().ToList();
        }

        internal WorksheetPart GetWorksheetPart(string sheetName)
        {
            var sheet = _excelDoc.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
            if (sheet != null)
            {
                return (WorksheetPart)_excelDoc.WorkbookPart.GetPartById(sheet.Id);
            }
            return null;
        }

        public string GetWorksheetName(WorksheetPart sheetPart)
        {
            var partId = _excelDoc.WorkbookPart.GetIdOfPart(sheetPart);
            var sheet = _excelDoc.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Id == partId);
            if (sheet != null)
                return sheet.Name;
            return string.Empty;
        }

        /// <summary>
        /// Returns the sheetAdapter if found, else returns null
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public WorksheetAdapter GetExcelWorksheet(string sheetName)
        {
            var sheetPart = GetWorksheetPart(sheetName);
            return sheetPart != null ? new WorksheetAdapter(sheetPart, this) : null;
        }


        internal Stylesheet GetStyleSheet()
        {
            return _excelDoc.WorkbookPart.WorkbookStylesPart.Stylesheet;
        }

        public string GetSharedString(int index)
        {
            return _sharedStringItems[index].InnerText;
        }


        private bool DoesSheetExists(string sheetName)
        {
            if (null == GetWorksheetPart(sheetName))
                return false;
            return true;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (null != _excelDoc)
                _excelDoc.Close();
        }

        #endregion

        public void Close()
        {
            if (_excelDoc != null)
                _excelDoc.Close();
        }

        public SpreadsheetDocument GetExcelDocument()
        {
            return _excelDoc;
        }

        public WorksheetAdapter GetFirstWorksheet()
        {
            var firstSheet = _excelDoc.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault();
            if (firstSheet != null)
            {
                var worksheetPart = (WorksheetPart)_excelDoc.WorkbookPart.GetPartById(firstSheet.Id);
                return new WorksheetAdapter(worksheetPart, this);
            }
            return null;
        }

        public WorksheetAdapter GetWorksheetByIndex(int index)
        {
            var sheet = _excelDoc.WorkbookPart.Workbook.Descendants<Sheet>().SingleOrDefault(s => s.SheetId == index);
            if (sheet != null)
            {
                var worksheetPart = (WorksheetPart)_excelDoc.WorkbookPart.GetPartById(sheet.Id);
                return new WorksheetAdapter(worksheetPart, this);
            }
            return null;
        }
    }
}
