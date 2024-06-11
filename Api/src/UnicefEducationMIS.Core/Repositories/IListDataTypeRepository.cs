using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IListDataTypeRepository
    {
        Task<ListDataType> Save(ListDataType aListDataType);
        Task Delete(long id);
        Task<ListDataType> GetById(long id);
        Task Update(ListDataType aListDataType);
        Task<PagedResponse<ListDataTypeViewModel>> GetPaginatedList(BaseQueryModel baseQueryModel);
    }
}
