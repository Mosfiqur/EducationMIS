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
    public interface IGridViewRepository
    {
        Task<GridView> Add(GridView gridView);
        Task<GridViewDetails> AddColumnToView(long gridViewId, long entityDynamicColumnId);
        Task<GridView> Update(GridView gridView);
        Task<GridView> Delete(long gridViewId);
        Task<GridViewModel> GetById(long id);
        Task<PagedResponse<GridViewViewModel>> GetAll(GridViewQueryModel baseQueryModel);
        Task<GridViewDetails> DeleteColumnToView(long gridViewId, long entityDynamicColumnId);
    }
}
