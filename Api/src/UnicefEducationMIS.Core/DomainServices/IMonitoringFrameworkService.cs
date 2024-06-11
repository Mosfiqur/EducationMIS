using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IMonitoringFrameworkService
    {
        Task<MonitoringFrameworkUpdateViewModel> Add(MonitoringFrameworkCreateViewModel model);
        Task Update(MonitoringFrameworkUpdateViewModel model);
        Task Delete(long id);
        Task<PagedResponse<MonitoringFrameworkViewModel>> GetAll(BaseQueryModel model);
        Task InsertDynamicCell(ObjectiveIndicatorDynamicCellInsertViewModel model);
        Task UpdateDynamicCell(ObjectiveIndicatorDynamicCellInsertViewModel model);
        Task DeleteDynamicCell(ObjectiveIndicatorDynamicCellViewModel model);
        Task<PagedResponse<ObjectiveIndicatorViewModel>> GetObjectiveIndicator(BaseQueryModel baseQueryModel);
    }
}
