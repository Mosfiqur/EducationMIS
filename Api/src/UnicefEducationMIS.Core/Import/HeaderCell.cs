namespace UnicefEducationMIS.Core.Import
{
    public class HeaderCell : ExcelCell
    {        
        public HeaderCell(string value, string cellRef):base(value, 1, cellRef)
        {
        }
    }

    public class MergedCell : ExcelCell
    {
        public MergedCell(string cellRef) : base(cellRef)
        {
        }
    }

}
