namespace UnicefEducationMIS.Service.Import
{
    public class CellInfo
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public string Value { get; set; }
        public string CellName { get; set; }
        public bool IsRange { get; set; }
        public int EndRow { get; set; }
        public int EndCol { get; set; }
        public string ColumnName { get; set; }

        public CellInfo Clone()
        {
            return (CellInfo)MemberwiseClone();
        }
    }
}