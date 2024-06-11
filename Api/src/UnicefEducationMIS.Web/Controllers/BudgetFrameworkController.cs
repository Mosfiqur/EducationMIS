using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Web.Controllers
{
    public class BudgetFrameworkController : BaseController
    {
        private readonly IBudgetFrameworkService _budgetFrameworkService;
        public BudgetFrameworkController(IBudgetFrameworkService budgetFrameworkService)
        {
            _budgetFrameworkService = budgetFrameworkService;
        }

        [Authorize(Policy = AppPermissions.ViewBudgetFramework)]
        [HttpGet("GetAll")]
        public async Task<PagedResponse<BudgetFrameworkViewModel>> GetAllBudgets([FromQuery] BaseQueryModel model)
        {
            return await _budgetFrameworkService.GetAllBudgets(model);
        }

        [Authorize(Policy = AppPermissions.ViewBudgetFramework)]
        [HttpGet("GetById")]
        public async Task<BudgetFrameworkViewModel> GetById(long id)
        {
            return await _budgetFrameworkService.GetById(id);
        }

        [Authorize(Policy = AppPermissions.AddBudgetFramework)]
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BudgetFrameworkCreateViewModel model)
        {
            await _budgetFrameworkService.Add(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.EditBudgetFramework)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] BudgetFrameworkUpdateViewModel model)
        {
            await _budgetFrameworkService.Update(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteBudgetFramework)]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int budgetId)
        {
            await _budgetFrameworkService.Delete(budgetId);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteBudgetFramework)]
        [HttpPost("DeleteMultiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody]List<long> budgetIds)
        {
            await _budgetFrameworkService.DeleteMultiple(budgetIds);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.AddBudgetDynamicCells)]
        [HttpPost("InsertDynamicCell")]
        public async Task<IActionResult> InsertDynamicCells([FromBody] BudgetDynamicCellInsertViewModel model)
        {
             await _budgetFrameworkService.InsertDynamicCell(model);
             return Ok();
        }

        [Authorize(Policy = AppPermissions.AddBudgetDynamicCells)]
        [HttpPut("UpdateDynamicCell")]
        public async Task<IActionResult> UpdateDynamicCells([FromBody] BudgetDynamicCellInsertViewModel model)
        {
            await _budgetFrameworkService.InsertDynamicCell(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteBudgetDynamicCells)]
        [HttpPost("DeleteDynamicCell")]
        public async Task<IActionResult> DeleteDynamicCells(BudgetDynamicCellViewModel model)
        {
            await _budgetFrameworkService.DeleteDynamicCell(model);
            return Ok();
        }



        [Authorize(Policy = AppPermissions.AddBudgetFramework)]
        [HttpPost("AddProject")]
        public async Task<ActionResult<ProjectViewModel>> AddProject([FromBody] ProjectViewModel model)
        {
            return Ok(await _budgetFrameworkService.AddProject(model));
        }

        [Authorize(Policy = AppPermissions.AddBudgetFramework)]
        [HttpPost("AddDonor")]
        public async Task<ActionResult<ProjectViewModel>> AddDonor([FromBody] DonorViewModel model)
        {
            return Ok(await _budgetFrameworkService.AddDonor(model));
        }


        [Authorize(Policy = AppPermissions.AddBudgetFramework)]
        [HttpGet("GetAllProjects")]
        public async Task<PagedResponse<ProjectViewModel>> GetAllProjects([FromQuery] BaseQueryModel model)
        {
            return await _budgetFrameworkService.GetAllProjects(model);
        }

        [Authorize(Policy = AppPermissions.AddBudgetFramework)]
        [HttpGet("GetAllDonors")]
        public async Task<PagedResponse<DonorViewModel>> GetAllDonors([FromQuery] BaseQueryModel model)
        {
            return await _budgetFrameworkService.GetAllDonors(model);
        }
    }
}