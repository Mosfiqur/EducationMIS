using System.Collections.Generic;
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
    public class TargetFrameworkController : BaseController
    {
        private readonly ITargetFrameworkService _targetFrameworkService;
        public TargetFrameworkController(ITargetFrameworkService targetFrameworkService)
        {
            _targetFrameworkService = targetFrameworkService;
        }

        [Authorize(AppPermissions.ViewTargetFramework)]
        [HttpGet("GetAll")]
        public async Task<PagedResponse<TargetFrameworkViewModel>> GetAll([FromQuery] BaseQueryModel model)
        {
            return await _targetFrameworkService.GetTargetFrameworks(model);
        }

        [Authorize(AppPermissions.ViewTargetFramework)]
        [HttpGet("GetById")]
        public async Task<TargetFrameworkViewModel> GetById(long id)
        {
            return await _targetFrameworkService.GetById(id);
        }


        [Authorize(AppPermissions.AddTargetFramework)]
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] TargetFrameworkCreateViewModel model)
        {
            await _targetFrameworkService.AddTargetFramework(model);
            return Ok();
        }

        [Authorize(AppPermissions.AddTargetFramework)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TargetFrameworkUpdateViewModel model)
        {
            await _targetFrameworkService.UpdateTargetFramework(model);
            return Ok();
        }

        [Authorize(AppPermissions.DeleteTargetFramework)]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int targetFrameworkId)
        {
            await _targetFrameworkService.DeleteTargetFramework(targetFrameworkId);
            return Ok();
        }
        [Authorize(AppPermissions.DeleteTargetFramework)]
        [HttpPost("DeleteMultiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody]List<long> targetFrameworkIds)
        {
            await _targetFrameworkService.DeleteMultipleTargetFramework(targetFrameworkIds);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.AddTargetDynamicCells)]
        [HttpPost("InsertDynamicCell")]
        public async Task<IActionResult> InsertDynamicCells([FromBody] TargetDynamicCellInsertViewModel model)
        {
            await _targetFrameworkService.InsertDynamicCell(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.AddTargetDynamicCells)]
        [HttpPut("UpdateDynamicCell")]
        public async Task<IActionResult> UpdateDynamicCells([FromBody] TargetDynamicCellInsertViewModel model)
        {
            await _targetFrameworkService.UpdateDynamicCell(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteTargetDynamicCells)]
        [HttpPost("DeleteDynamicCell")]
        public async Task<IActionResult> DeleteDynamicCells(TargetDynamicCellViewModel model)
        {
            await _targetFrameworkService.DeleteDynamicCell(model);
            return Ok();
        }
    }
}