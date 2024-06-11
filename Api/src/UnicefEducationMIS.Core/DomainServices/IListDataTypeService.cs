using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IListDataTypeService
    {
        Task<ListDataType> Add(ListDataTypeCreateViewModel model);      
        Task Delete(long columnListId);
        Task<PagedResponse<ListDataTypeViewModel>> GetAll(BaseQueryModel model);
        Task<ListDataTypeViewModel> GetById(long id);
        Task Update(ListDataTypeUpdateViewModel model);
    }
}
