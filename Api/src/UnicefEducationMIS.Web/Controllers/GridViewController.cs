using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Web.Controllers
{
    public class GridViewController : BaseController
    {
        private readonly IGridViewService _gridViewService;
      
        public GridViewController(IGridViewService gridViewService)
        {
            _gridViewService = gridViewService;
        }

        [HttpPost("AddBeneficiaryGridView")]
        [Authorize(AppPermissions.CreateBeneficiaryGridView)]
        public async Task<IActionResult> AddBeneficiaryGridView([FromBody] GridViewViewModel createGridView)
        {
            var gridView = await _gridViewService.Add(createGridView);
            return Ok(gridView);
        }

        [HttpPost("AddBeneficiaryColumnToView")]
        [Authorize(AppPermissions.CreateBeneficiaryGridView)]
        public async Task<IActionResult> AddBeneficiaryColumnToView(AddColumnToGridViewModel addColumnToGridViewModel)
        {
            var gridView = await _gridViewService.AddColumnToView(addColumnToGridViewModel.GridViewId, addColumnToGridViewModel.EntityDynamicColumnId);
            return Ok(gridView);
        }

        [HttpPost("UpdateBeneficiaryGridView")]
        [Authorize(AppPermissions.CreateBeneficiaryGridView)]
        public async Task<IActionResult> UpdateBeneficiaryGridView([FromBody] GridViewViewModel createGridView)
        {
            var gridView = await _gridViewService.Update(createGridView);
            return Ok(gridView);
        }

        [HttpDelete("DeleteBeneficiaryColumnToView")]
        [Authorize(AppPermissions.DeleteBeneficiaryGridView)]
        public async Task<IActionResult> DeleteBeneficiaryColumnToView(long gridViewId,long entityDynamicColumnId)
        {
            var gridView = await _gridViewService.DeleteColumnToView(gridViewId, entityDynamicColumnId);
            return Ok(gridView);
        }

        [HttpPost("DeleteBeneficiaryGridView")]
        [Authorize(AppPermissions.DeleteBeneficiaryGridView)]
        public async Task<IActionResult> DeleteBeneficiaryGridView([FromBody] long gridViewId)
        {
            var gridView = await _gridViewService.Delete(gridViewId);
            return Ok(gridView);
        }
        [HttpGet("GetByIdBeneficiary")]
        [Authorize(AppPermissions.ViewBeneficiaryGrid)] 
        public async Task<IActionResult> GetByIdBeneficiary([FromQuery]long id)
        {
            var gridView = await _gridViewService.GetById(id);
            return Ok(gridView);
        }
        [HttpGet("GetAllGridViewBeneficiary")]
        [Authorize(AppPermissions.ViewBeneficiaryGrid)]
        public async Task<IActionResult> GetAllGridViewBeneficiary([FromQuery] GridViewQueryModel baseQueryModel)
        {
            var gridView = await _gridViewService.GetAll(baseQueryModel);
            return Ok(gridView);
        }


        //Facility

        [HttpPost("AddFacilityGridView")]
        [Authorize(AppPermissions.CreateFacilityGridView)]
        public async Task<IActionResult> AddFacilityGridView([FromBody] GridViewViewModel createGridView)
        {
            var gridView = await _gridViewService.Add(createGridView);
            return Ok(gridView);
        }

        [HttpPost("AddFacilityColumnToView")]
        [Authorize(AppPermissions.CreateFacilityGridView)]
        public async Task<IActionResult> AddFacilityColumnToView(AddColumnToGridViewModel addColumnToGridViewModel)
        {
            var gridView = await _gridViewService.AddColumnToView(addColumnToGridViewModel.GridViewId, addColumnToGridViewModel.EntityDynamicColumnId);
            return Ok(gridView);
        }

        [HttpPost("UpdateFacilityGridView")]
        [Authorize(AppPermissions.CreateFacilityGridView)]
        public async Task<IActionResult> UpdateFacilityGridView([FromBody] GridViewViewModel createGridView)
        {
            var gridView = await _gridViewService.Update(createGridView);
            return Ok(gridView);
        }

        [HttpDelete("DeleteFacilityColumnToView")]
        [Authorize(AppPermissions.DeleteFacilityGridView)]
        public async Task<IActionResult> DeleteFacilityColumnToView(long gridViewId, long entityDynamicColumnId)
        {
            var gridView = await _gridViewService.DeleteColumnToView(gridViewId, entityDynamicColumnId);
            return Ok(gridView);
        }

        [HttpPost("DeleteFacilityGridView")]
        [Authorize(AppPermissions.DeleteFacilityGridView)]
        public async Task<IActionResult> DeleteFacilityGridView([FromBody] long gridViewId)
        {
            var gridView = await _gridViewService.Delete(gridViewId);
            return Ok(gridView);
        }
        [HttpGet("GetByIdFacility")]
        [Authorize(AppPermissions.ViewFacilityGrid)]
        public async Task<IActionResult> GetByIdFacility([FromQuery] long id)
        {
            var gridView = await _gridViewService.GetById(id);
            return Ok(gridView);
        }
        [HttpGet("GetAllGridViewFacility")]
        [Authorize(AppPermissions.ViewFacilityGrid)]
        public async Task<IActionResult> GetAllGridViewFacility([FromQuery] GridViewQueryModel baseQueryModel)
        {
            var gridView = await _gridViewService.GetAll(baseQueryModel);
            return Ok(gridView);
        }
    }
}
