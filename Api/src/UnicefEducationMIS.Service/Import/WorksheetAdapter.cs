using System;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace UnicefEducationMIS.Service.Import
{
    public class WorksheetAdapter
    {
        private readonly WorksheetPart _sheetPart;
        private readonly WorkbookAdapter _workbookAdapter;
        private readonly SheetData _sheetData;
        private int _maxRowIndex;
        private readonly string _sheetName;

        public WorksheetAdapter(WorksheetPart sheetPart, WorkbookAdapter workbookAdapter)
        {
            _sheetPart = sheetPart;
            _workbookAdapter = workbookAdapter;
            _sheetName = WorkbookAdapter.GetWorksheetName(_sheetPart);
            _sheetData = _sheetPart.Worksheet.GetFirstChild<SheetData>();
        }

        public RowAdapter ReadRow(int rowIndex)
        {
            var row = _sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex != null && r.RowIndex == rowIndex);
            if (null == row)
                return null;
            var excelRow = new RowAdapter(row, WorkbookAdapter, this);
            excelRow.Read();
            return excelRow;
        }

        public string SheetName
        {
            get
            {
                return _sheetName;
            }
        }

        public int MaxRowIndex
        {
            get
            {
                if (_maxRowIndex > 0)
                    return _maxRowIndex;

                var lastRow = _sheetData.Elements<Row>().LastOrDefault();
                if (lastRow != null)
                {
                    _maxRowIndex = Convert.ToInt32(lastRow.RowIndex.Value);
                    return _maxRowIndex;
                }
                return 0;
            }
        }

        public WorkbookAdapter WorkbookAdapter
        {
            get { return _workbookAdapter; }
        }
    }
}
