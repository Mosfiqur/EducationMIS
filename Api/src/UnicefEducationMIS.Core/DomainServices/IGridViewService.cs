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
    public interface IGridViewService
    {
        Task<GridView> Add(GridViewViewModel gridView);
        Task<GridViewDetails> AddColumnToView(long gridViewId, long entityDynamicColumnId);
        Task<GridView> Update(GridViewViewModel gridView);
        Task<GridView> Delete(long gridViewId);
        Task<GridViewModel> GetById(long id);
        Task<PagedResponse<GridViewViewModel>> GetAll(GridViewQueryModel baseQueryModel);
        Task<GridViewDetails> DeleteColumnToView(long gridViewId, long entityDynamicColumnId);
    }
}
