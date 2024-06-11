namespace UnicefEducationMIS.Core.Import
{
    public abstract class ExcelCell
    {
        
        public string ActualName { get; set; }
        public string DisplayName { get; set; }
        public bool IsMandatory { get; set; }
        public string Value { get; set; }
        public bool IsAutoCalculated { get; set; } 

        public uint Style { get; set; }
        public string CellReference { get; set; }

        
        public ExcelCell(string displayName, string actualName, string value, bool isMandatory,bool isCalculated=false)
        {
            DisplayName = displayName;
            ActualName = actualName;
            Value = value;
            IsMandatory = isMandatory;
            IsAutoCalculated = isCalculated;
        }

        public ExcelCell(string value, uint style, string cellRef)
        {
            Value = value;
            Style = style;
            CellReference = cellRef;
        }

        public ExcelCell(string cellRef)
        {            
            CellReference = cellRef;
        }

        public ExcelCell(string displayName, string actualName, uint style)
        {
            DisplayName = displayName;
            ActualName = actualName;
            Style = style;
        }
    }

    
}