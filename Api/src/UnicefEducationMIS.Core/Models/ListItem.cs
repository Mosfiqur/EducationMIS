namespace UnicefEducationMIS.Core.Models
{
    public class ListItem : BaseModel<long>
    {
        public string Title { get; set; }
        public int Value { get; set; }
        public long ColumnListId { get; set; }
        public ListDataType ColumnList { get; set; }
         
    }
}
