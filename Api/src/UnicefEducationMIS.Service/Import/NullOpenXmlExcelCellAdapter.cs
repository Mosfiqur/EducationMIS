using DocumentFormat.OpenXml.Spreadsheet;

namespace UnicefEducationMIS.Service.Import
{
    internal class NullOpenXmlExcelCellAdapter : CellAdapter
    {
        public NullOpenXmlExcelCellAdapter(Cell cell, WorkbookAdapter workbookAdapter) : base(cell, workbookAdapter)
        {
        }

        public override object Value
        {
            get { return ""; }
        }

    }
}