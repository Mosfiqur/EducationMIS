using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.QueryModel.Common;

namespace UnicefEducationMIS.Web.Controllers
{
    public class CommonController : BaseController
    {
        private ICommonService _commonService;
        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetCamps")]
        public async Task<IActionResult> GetCamps([FromQuery] CampQueryModel model)
        {
            var data = await _commonService.GetCamps(model);
            return Ok(data);
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetBlocks")]
        public async Task<IActionResult> GetBlocks([FromQuery] BlockQueryModel model)
        {
            var data = await _commonService.GetBlocks(model);
            return Ok(data);
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetSubBlocks")]
        public async Task<IActionResult> GetSubBlocks([FromQuery] SubBlockQueryModel model)
        {
            var data = await _commonService.GetSubBlocks(model);
            return Ok(data);
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetCampWithBlockSubBlock")]
        public async Task<IActionResult> GetCampsWithBlockSubBlock([FromQuery] BaseQueryModel model)
        {
            var data = await _commonService.GetCampsWithBlockSubBlock(model);
            return Ok(data);
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetUpazilas")]
        public async Task<IActionResult> GetUpazilas([FromQuery] UpazilaQueryModel model)
        {
            var data = await _commonService.GetUpazilas(model);
            return Ok(data);
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetUnions")]
        public async Task<IActionResult> GetUnions([FromQuery] UnionQueryModel model)
        {
            var data = await _commonService.GetUnions(model);
            return Ok(data);
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetDistricts")]
        public async Task<IActionResult> GetDistricts([FromQuery] BaseQueryModel model)
        {
            var data = await _commonService.GetDistricts(model);
            return Ok(data);
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetAgeGroups")]
        public async Task<IActionResult> GetAgeGroups([FromQuery] BaseQueryModel model)
        {
            var data = await _commonService.GetAgeGroups(model);
            return Ok(data);
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetReportingFrequencies")]
        public async Task<IActionResult> GetReportingFrequencies([FromQuery] BaseQueryModel model)
        {
            var data = await _commonService.GetReportingFrequencies(model);
            return Ok(data);
        }
    }
}