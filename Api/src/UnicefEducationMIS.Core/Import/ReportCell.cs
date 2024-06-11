namespace UnicefEducationMIS.Core.Import
{
    public class ReportCell : ExcelCell
    {
        
        public ReportCell(string displayName, string actualName) : base(displayName, actualName, string.Empty, false)
        {
        }
        public ReportCell(string displayName, string actualName, uint style) : base(displayName, actualName, style)
        {
        }
        public ReportCell(string displayName, string actualName, string value) : base(displayName, actualName, value, false)
        {
        }
        public ReportCell(string displayName, string actualName, bool isMandatory) : base(displayName, actualName, string.Empty, isMandatory)
        {
        }
        public ReportCell(string displayName, string actualName, string value, bool isMandatory) : base(displayName, actualName, value, isMandatory)
        {
        }
        public ReportCell(string value, uint style, string cellRef) : base(value, style, cellRef)
        {
        }

    }

}
