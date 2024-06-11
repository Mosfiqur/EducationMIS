using System.Collections.Generic;

namespace UnicefEducationMIS.Core.ResponseModel
{
    public class PagedResponse<T>
    {
        public PagedResponse()
        {
            Data = new List<T>();
        }    

        public PagedResponse(IEnumerable<T> data, int total, int pageNumber, int pageSize)
        {
            Data = data;
            Total = total;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }


    }
}
