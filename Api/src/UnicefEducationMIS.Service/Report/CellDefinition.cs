namespace UnicefEducationMIS.Service.Report
{
    public class CellDefinition
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public object Content { get; set; }
        public string StyleName { get; set; }
        public string MergeCellReference { get; set; }
    }
}
