namespace UnicefEducationMIS.Core.Models.Framework
{
    public class TargetFrameworkDynamicCell : BaseModel<long>
    {
        public long TargetFrameworkId { get; set; }
        public long EntityDynamicColumnId { get; set; }
        public string Value { get; set; }

        public TargetFramework TargetFramework { get; set; }
        public EntityDynamicColumn EntityDynamicColumn { get; set; }
    }
}
