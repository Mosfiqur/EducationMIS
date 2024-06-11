using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Web.Controllers
{
    public class MonitoringFrameworkController : BaseController
    {
        private readonly IMonitoringFrameworkService _monitoringFrameworkService;
        private readonly IObjectiveIndicatorService _indicatorService;
        public MonitoringFrameworkController(IMonitoringFrameworkService monitoringFrameworkService, 
            IObjectiveIndicatorService indicatorService)
        {
            _monitoringFrameworkService = monitoringFrameworkService;
            _indicatorService = indicatorService;
        }

        [Authorize(Policy = AppPermissions.ViewMonitoringFramework)]
        [HttpGet("GetAll")]
        public async Task<PagedResponse<MonitoringFrameworkViewModel>> GetAll([FromQuery] BaseQueryModel model)
        {
            return await _monitoringFrameworkService.GetAll(model);
        }

        [Authorize(Policy = AppPermissions.AddMonitoringFramework)]
        [HttpPost("Add")]
        public async Task<MonitoringFrameworkUpdateViewModel> Add([FromBody] MonitoringFrameworkCreateViewModel model)
        {
            return await _monitoringFrameworkService.Add(model);
        }

        [Authorize(Policy = AppPermissions.EditMonitoringFramework)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] MonitoringFrameworkUpdateViewModel model)
        {
            await _monitoringFrameworkService.Update(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteMonitoringFramework)]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            await _monitoringFrameworkService.Delete(id);
            return Ok();
        }
               
        [Authorize(Policy = AppPermissions.AddMonitoringDynamicCells)]
        [HttpPost("InsertDynamicCell")]
        public async Task<IActionResult> InsertDynamicCells([FromBody] ObjectiveIndicatorDynamicCellInsertViewModel model)
        {
            await _monitoringFrameworkService.InsertDynamicCell(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.AddMonitoringDynamicCells)]
        [HttpPut("UpdateDynamicCell")]
        public async Task<IActionResult> UpdateDynamicCells([FromBody] ObjectiveIndicatorDynamicCellInsertViewModel model)
        {
            await _monitoringFrameworkService.UpdateDynamicCell(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteMonitoringDynamicCells)]
        [HttpPost("DeleteDynamicCell")]
        public async Task<IActionResult> DeleteDynamicCells([FromBody] ObjectiveIndicatorDynamicCellViewModel model)
        {
            await _monitoringFrameworkService.DeleteDynamicCell(model);
            return Ok();
        }


        [Authorize(Policy = AppPermissions.AddMonitoringFramework)]
        [HttpPost("AddIndicator")]
        public async Task<ObjectiveIndicatorViewModel> AddIndicator([FromBody] ObjectiveIndicatorCreateViewModel model)
        {
            return await _indicatorService.Create(model);
        }
        
        [Authorize(Policy = AppPermissions.EditMonitoringFramework)]
        [HttpPut("UpdateIndicator")]
        public async Task<IActionResult> UpdateIndicator([FromBody] ObjectiveIndicatorUpdateViewModel model)
        {
            await _indicatorService.Update(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.ViewMonitoringFramework)]
        [HttpGet("GetObjectiveIndicator")]
        public async Task<PagedResponse<ObjectiveIndicatorViewModel>> GetObjectiveIndicator([FromQuery] BaseQueryModel model)
        {
            return await _monitoringFrameworkService.GetObjectiveIndicator(model);
        }
    }
}