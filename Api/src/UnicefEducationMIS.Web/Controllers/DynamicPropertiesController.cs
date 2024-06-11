using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Web.Controllers
{
    public class DynamicPropertiesController : BaseController
    {
        private readonly IDynamicPropertiesService _dynamicPropertiesService;
        private readonly IBeneficiaryDynamicCellService _beneficiaryDynamicCellService;
        private readonly IFacilityDynamicCellService _facilityDynamicCellService;
        public DynamicPropertiesController(IDynamicPropertiesService dynamicPropertiesService, IBeneficiaryDynamicCellService beneficiaryDynamicCellService = null, IFacilityDynamicCellService facilityDynamicCellService = null)
        {
            _dynamicPropertiesService = dynamicPropertiesService;
            _beneficiaryDynamicCellService = beneficiaryDynamicCellService;
            _facilityDynamicCellService = facilityDynamicCellService;
        }
        
        //Save Dynamic Column

        [Authorize(Policy = AppPermissions.SaveBeneficiaryDynamicColumn)]
        [HttpPost("SaveBeneficiaryDynamicColumn")]
        public async Task<ActionResult> SaveBeneficiaryDynamicColumn([FromBody] DynamicColumnViewModel dynamicColumnViewModel)
        {
            var entityDynamicColumn = await _dynamicPropertiesService.SaveDynamicColumn(dynamicColumnViewModel);
            return Ok(entityDynamicColumn);
        }

        [Authorize(Policy = AppPermissions.SaveFacilityDynamicColumn)]
        [HttpPost("SaveFacilityDynamicColumn")]
        public async Task<ActionResult> SaveFacilityDynamicColumn([FromBody] DynamicColumnViewModel dynamicColumnViewModel)
        {
            var entityDynamicColumn = await _dynamicPropertiesService.SaveDynamicColumn(dynamicColumnViewModel);
            return Ok(entityDynamicColumn);
        }

        [Authorize(Policy = AppPermissions.SaveMonitoringDynamicColumn)]
        [HttpPost("SaveMonitoringDynamicColumn")]
        public async Task<ActionResult> SaveMonitoringDynamicColumn([FromBody] DynamicColumnViewModel dynamicColumnViewModel)
        {
            var entityDynamicColumn = await _dynamicPropertiesService.SaveDynamicColumn(dynamicColumnViewModel);
            return Ok(entityDynamicColumn);
        }

        [Authorize(Policy = AppPermissions.SaveBudgetDynamicColumn)]
        [HttpPost("SaveBudgetDynamicColumn")]
        public async Task<ActionResult> SaveBudgetDynamicColumn([FromBody] DynamicColumnViewModel dynamicColumnViewModel)
        {
            var entityDynamicColumn = await _dynamicPropertiesService.SaveDynamicColumn(dynamicColumnViewModel);
            return Ok(entityDynamicColumn);
        }

        [Authorize(Policy = AppPermissions.SaveTargetDynamicColumn)]
        [HttpPost("SaveTargetDynamicColumn")]
        public async Task<ActionResult> SaveTargetDynamicColumn([FromBody] DynamicColumnViewModel dynamicColumnViewModel)
        {
            var entityDynamicColumn = await _dynamicPropertiesService.SaveDynamicColumn(dynamicColumnViewModel);
            return Ok(entityDynamicColumn);
        }


        [Authorize(Policy = AppPermissions.SaveUserDynamicColumn)]
        [HttpPost("SaveUserDynamicColumn")]
        public async Task<ActionResult> SaveUserDynamicColumn([FromBody] DynamicColumnViewModel dynamicColumnViewModel)
        {
            var entityDynamicColumn = await _dynamicPropertiesService.SaveDynamicColumn(dynamicColumnViewModel);
            return Ok(entityDynamicColumn);
        }


        //Get All Column

        [Authorize(Policy = AppPermissions.ViewBeneficiaryDynamicColumn)]
        [HttpGet("GetAllBeneficiaryColumn")]
        public async Task<ActionResult> GetAllBeneficiaryColumn([FromQuery] DynamicPropertiesBySearchParamQueryModel dynamicPropertiesBySearchModel)
        {
            var data = await _dynamicPropertiesService.GetPaginatedColumn(dynamicPropertiesBySearchModel);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewFacilityDynamicColumn)]
        [HttpGet("GetAllFacilityColumn")]
        public async Task<ActionResult> GetAllFacilityColumn([FromQuery] DynamicPropertiesBySearchParamQueryModel dynamicPropertiesBySearchModel)
        {
            var data = await _dynamicPropertiesService.GetPaginatedColumn(dynamicPropertiesBySearchModel);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewMonitoringDynamicColumn)]
        [HttpGet("GetAllMonitoringColumn")]
        public async Task<ActionResult> GetAllMonitoringColumn([FromQuery] DynamicPropertiesBySearchParamQueryModel dynamicPropertiesBySearchModel)
        {
            var data = await _dynamicPropertiesService.GetPaginatedColumn(dynamicPropertiesBySearchModel);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewBudgetDynamicColumn)]
        [HttpGet("GetAllBudgetColumn")]
        public async Task<ActionResult> GetAllBudgetColumn([FromQuery] DynamicPropertiesBySearchParamQueryModel dynamicPropertiesBySearchModel)
        {
            var data = await _dynamicPropertiesService.GetPaginatedColumn(dynamicPropertiesBySearchModel);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewTargetDynamicColumn)]
        [HttpGet("GetAllTargetColumn")]
        public async Task<ActionResult> GetAllTargetColumn([FromQuery] DynamicPropertiesBySearchParamQueryModel dynamicPropertiesBySearchModel)
        {
            var data = await _dynamicPropertiesService.GetPaginatedColumn(dynamicPropertiesBySearchModel);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewUserDynamicColumn)]
        [HttpGet("GetAllUserColumn")]
        public async Task<ActionResult> GetAllUserColumn([FromQuery] DynamicPropertiesBySearchParamQueryModel dynamicPropertiesBySearchModel)
        {
            var data = await _dynamicPropertiesService.GetPaginatedColumn(dynamicPropertiesBySearchModel);
            return Ok(data);
        }


        // Delete Dynamic Column

        [Authorize(Policy = AppPermissions.DeleteBudgetDynamicColumn)]
        [HttpDelete("DeleteBudgetDynamicColumn")]
        public async Task<IActionResult> DeleteBudgetDynamicColumn(long entityDynamicColumnId)
        {
            await _dynamicPropertiesService.DeleteDynamicColumn(entityDynamicColumnId);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteTargetDynamicColumn)]
        [HttpDelete("DeleteTargetDynamicColumn")]
        public async Task<IActionResult> DeleteTargetDynamicColumn(long entityDynamicColumnId)
        {
            await _dynamicPropertiesService.DeleteDynamicColumn(entityDynamicColumnId);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteUserDynamicColumn)]
        [HttpDelete("DeleteUserDynamicColumn")]
        public async Task<IActionResult> DeleteUserDynamicColumn(long entityDynamicColumnId)
        {
            await _dynamicPropertiesService.DeleteDynamicColumn(entityDynamicColumnId);
            return Ok();
        }


        // Get By Id Dynamic Column

        [Authorize(Policy = AppPermissions.ViewBeneficiaryDynamicColumn)]
        [HttpGet("GetBeneficiaryDynamicColumnById")]
        public async Task<ActionResult> GetBeneficiaryDynamicColumnById([FromQuery] long id)
        {
            var data = await _dynamicPropertiesService.GetById(id);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewFacilityDynamicColumn)]
        [HttpGet("GetFacilityDynamicColumnById")]
        public async Task<ActionResult> GetFacilityDynamicColumnById([FromQuery] long id)
        {
            var data = await _dynamicPropertiesService.GetById(id);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewMonitoringDynamicColumn)]
        [HttpGet("GetMonitoringDynamicColumnById")]
        public async Task<ActionResult> GetMonitoringDynamicColumnById([FromQuery] long id)
        {
            var data = await _dynamicPropertiesService.GetById(id);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewBudgetDynamicColumn)]
        [HttpGet("GetBudgetDynamicColumnById")]
        public async Task<ActionResult> GetBudgetDynamicColumnById([FromQuery] long id)
        {
            var data = await _dynamicPropertiesService.GetById(id);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewTargetDynamicColumn)]
        [HttpGet("GetTargetDynamicColumnById")]
        public async Task<ActionResult> GetTargetDynamicColumnById([FromQuery] long id)
        {
            var data = await _dynamicPropertiesService.GetById(id);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewUserDynamicColumn)]
        [HttpGet("GetUserDynamicColumnById")]
        public async Task<ActionResult> GetUserDynamicColumnById([FromQuery] long id)
        {
            var data = await _dynamicPropertiesService.GetById(id);
            return Ok(data);
        }

        //Get Numeric column

        [Authorize(HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetNumericColumn")]
        public async Task<ActionResult> GetNumericColumn([FromQuery] DynamicPropertiesBySearchParamQueryModel dynamicPropertiesBySearchModel)
        {
            var data = await _dynamicPropertiesService.GetPaginatedNumericColumn(dynamicPropertiesBySearchModel);
            return Ok(data);
        }


        [Authorize(Policy = AppPermissions.ViewBeneficiaryDynamicColumn)]
        [HttpGet("GetBeneficiaryColumnForIndicator")]
        public async Task<ActionResult> GetBeneficiaryColumnForIndicator([FromQuery] DynamicPropertiesByInstanceIdQueryParam dynamicPropertiesBySearchModel)
        {
            var data = await _dynamicPropertiesService.GetPaginatedColumnForIndicator(dynamicPropertiesBySearchModel);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewFacilityDynamicColumn)]
        [HttpGet("GetFacilityColumnForIndicator")]
        public async Task<ActionResult> GetFacilityColumnForIndicator([FromQuery] DynamicPropertiesByInstanceIdQueryParam dynamicPropertiesBySearchModel)
        {
            var data = await _dynamicPropertiesService.GetPaginatedColumnForIndicator(dynamicPropertiesBySearchModel);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.AddBeneficiaryCell)]
        [HttpPost("SaveBeneficiaryCell")]
        public async Task<ActionResult> AddBeneficiaryCell([FromBody] BeneficiaryDynamicCellAddViewModel beneficiaryDynamicCell)
        {
            await _beneficiaryDynamicCellService.Save(beneficiaryDynamicCell);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteBeneficiaryCell)]
        [HttpDelete("DeleteBeneficiaryCell")]
        public async Task<ActionResult> DeleteBeneficiaryCell([FromBody] BeneficiaryDynamicCellDeleteViewModel dynamicCell)
        {
            await _beneficiaryDynamicCellService.Delete(dynamicCell);
            return Ok();

        }

        [Authorize(Policy = AppPermissions.AddFacilityCell)]
        [HttpPost("SaveFacilityCell")]
        public async Task<ActionResult> SaveFacilityCell([FromBody] FacilityDynamicCellAddViewModel facilityDynamicCell)
        {
            await _facilityDynamicCellService.Save(facilityDynamicCell);
            return Ok();

        }

        [Authorize(Policy = AppPermissions.DeleteFacilityCell)]
        [HttpDelete("DeleteFacilityCell")]
        public async Task<ActionResult> DeleteFacilityCell([FromQuery] FacilityDynamicCellDeleteViewModel dynamicCell)
        {
            await _facilityDynamicCellService.Delete(dynamicCell);
            return Ok();

        }
    }
}
