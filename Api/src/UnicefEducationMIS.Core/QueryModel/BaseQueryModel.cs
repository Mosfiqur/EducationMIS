using System;


namespace UnicefEducationMIS.Core.QueryModel
{
    public class BaseQueryModel
    {
        public int PageSize { get; set; } = Int32.MaxValue;
        public int PageNo { get; set; } = 1;
        public int Skip() => (PageNo - 1) * PageSize;

        public string SearchText { get; set; } = "";

        public static BaseQueryModel All = new BaseQueryModel(){PageSize = int.MaxValue, PageNo = 1};
    }
}
