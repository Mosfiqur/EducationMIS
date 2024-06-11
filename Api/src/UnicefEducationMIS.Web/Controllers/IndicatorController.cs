using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Web.Controllers
{
    public class IndicatorController : BaseController
    {
        private readonly IIndicatorService _indicatorService;
        public IndicatorController(IIndicatorService indicatorService)
        {
            _indicatorService = indicatorService;
        }

        //[Authorize(AppPermissions.AddIndicator)]
        //[HttpPost("AddInstanceIndicator")]
        //public async Task<IActionResult> AddInstanceIndicator([FromQuery] long instanceId)
        //{
        //    await _indicatorService.AddInstanceIndicator(instanceId);
        //    return Ok();
        //}

        //[Authorize(AppPermissions.AddIndicator)]
        //[HttpPut("UpdateInstanceIndicator")]
        //public async Task<IActionResult> UpdateInstanceIndicator([FromQuery] long instanceId)
        //{
        //    await _indicatorService.UpdateInstanceIndicator(instanceId);
        //    return Ok();
        //}

        //[Authorize(AppPermissions.ViewIndicator)]
        //[HttpGet("GetIndicatorsByEntityType")]
        //public async Task<IActionResult> GetIndicatorsByEntityType([FromQuery] IndicatorsByEntityTypeQueryModel indicatorsQueryModel)
        //{
        //    var data = await _indicatorService.GetIndicatorsByEntityType(indicatorsQueryModel);
        //    return Ok(data);
        //}


        [Authorize(HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetIndicatorsByInstance")]
        public async Task<IActionResult> GetIndicatorsByInstance([FromQuery] IndicatorsByInstanceQueryModel indicatorsQueryModel)
        {
            var data = await _indicatorService.GetIndicatorsByInstance(indicatorsQueryModel);
            return Ok(data);
        }

        [Authorize(AppPermissions.ViewBeneficiaryIndicator)]
        [HttpGet("GetBeneficiaryIndicatorsByInstance")]
        public async Task<IActionResult> GetBeneficiaryIndicatorsByInstance([FromQuery] IndicatorsByInstanceQueryModel indicatorsQueryModel)
        {
            var data = await _indicatorService.GetIndicatorsByInstance(indicatorsQueryModel);
            return Ok(data);
        }

        [Authorize(AppPermissions.ViewFacilityIndicator)]
        [HttpGet("GetFacilityIndicatorsByInstance")]
        public async Task<IActionResult> GetFacilityIndicatorsByInstance([FromQuery] IndicatorsByInstanceQueryModel indicatorsQueryModel)
        {
            var data = await _indicatorService.GetIndicatorsByInstance(indicatorsQueryModel);
            return Ok(data);
        }

        [Authorize(AppPermissions.ViewBeneficiaryIndicator)]
        [HttpGet("GetBeneficiaryIndicator")]
        public async Task<IActionResult> GetBeneficiaryIndicator([FromQuery] IndicatorByBeneficairyWiseQueryModel indicatorsQueryModel)
        {
            var data = await _indicatorService.GetBeneficiaryIndicators(indicatorsQueryModel);
            return Ok(data);
        }

        [Authorize(AppPermissions.ViewFacilityIndicator)]
        [HttpGet("GetFacilityIndicator")]
        public async Task<IActionResult> GetFacilityIndicator([FromQuery] IndicatorByFacilityWiseQueryModel indicatorsQueryModel)
        {
            var data = await _indicatorService.GetFacilityIndicators(indicatorsQueryModel);
            return Ok(data);
        }

        [Authorize(AppPermissions.AddBeneficiaryIndicator)]
        [HttpPost("AddBeneficiaryIndicator")]
        public async Task<IActionResult> AddBeneficiaryIndicator([FromBody] List<AddInstanceIndicatorViewModel> instanceIndicators)
        {
            await _indicatorService.Insert(instanceIndicators);
            return Ok();
        }

        [Authorize(AppPermissions.AddFacilityIndicator)]
        [HttpPost("AddFacilityIndicator")]
        public async Task<IActionResult> AddFacilityIndicator([FromBody] List<AddInstanceIndicatorViewModel> instanceIndicators)
        {
            await _indicatorService.Insert(instanceIndicators);
            return Ok();
        }

        [Authorize(AppPermissions.AddBeneficiaryIndicator)]
        [HttpPost("UpdateBeneficiaryIndicator")]
        public async Task<IActionResult> UpdateBeneficiaryIndicator([FromBody] List<AddInstanceIndicatorViewModel> instanceIndicators)
        {
            await _indicatorService.Update(instanceIndicators);
            return Ok();
        }

        [Authorize(AppPermissions.AddFacilityIndicator)]
        [HttpPost("UpdateFacilityIndicator")]
        public async Task<IActionResult> UpdateFacilityIndicator([FromBody] List<AddInstanceIndicatorViewModel> instanceIndicators)
        {
            await _indicatorService.Update(instanceIndicators);
            return Ok();
        }

        [Authorize(AppPermissions.DeleteBeneficiaryIndicator)]
        [HttpPost("DeleteBeneficiaryIndicator")]
        public async Task<IActionResult> DeleteBeneficiaryIndicator(List<DeleteIndicatorViewModel> indicator)
        {
            await _indicatorService.Delete(indicator);
            return Ok();
        }

        [Authorize(AppPermissions.DeleteFacilityIndicator)]
        [HttpPost("DeleteFacilityIndicator")]
        public async Task<IActionResult> DeleteFacilityIndicator(List<DeleteIndicatorViewModel> indicator)
        {
            await _indicatorService.Delete(indicator);
            return Ok();
        }
    }
}