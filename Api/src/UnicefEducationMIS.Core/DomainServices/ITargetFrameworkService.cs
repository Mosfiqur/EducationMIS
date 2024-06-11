using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface ITargetFrameworkService
    {
        Task AddTargetFramework(TargetFrameworkCreateViewModel  model);
        Task UpdateTargetFramework(TargetFrameworkUpdateViewModel model);
        Task DeleteTargetFramework(long targetFrameworkId);
        Task DeleteMultipleTargetFramework(List<long> targetFrameworkIds);
        Task<PagedResponse<TargetFrameworkViewModel>> GetTargetFrameworks(BaseQueryModel model);
        Task<TargetFrameworkViewModel> GetById(long id);

        Task InsertDynamicCell(TargetDynamicCellInsertViewModel model);
        Task UpdateDynamicCell(TargetDynamicCellInsertViewModel model);
        Task DeleteDynamicCell(TargetDynamicCellViewModel model);
    }
}
