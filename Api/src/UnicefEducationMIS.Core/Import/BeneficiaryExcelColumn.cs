namespace UnicefEducationMIS.Core.Import
{
    public class BeneficiaryExcelColumn : ExcelCell
    {
        public BeneficiaryExcelColumn(string displayName, string actualName) : base(displayName.ToLower(), actualName, string.Empty, false)
        {
        }
        public BeneficiaryExcelColumn(string displayName, string actualName, uint style) : base(displayName.ToLower(), actualName, style)
        {            
        }
        public BeneficiaryExcelColumn(string displayName, string actualName, string value) : base(displayName.ToLower(), actualName, value, false)
        {
        }
        public BeneficiaryExcelColumn(string displayName, string actualName, bool isMandatory) : base(displayName.ToLower(), actualName, string.Empty, isMandatory)
        {
        }
        public BeneficiaryExcelColumn(string displayName, string actualName, string value, bool isMandatory) : base(displayName.ToLower(), actualName, value, isMandatory)
        {
        }

    }
}
