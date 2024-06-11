using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Web.Controllers
{
    public class DataApprovalController : BaseController
    {
        private readonly IDataApprovalService _dataCollectionStatusService;

        public DataApprovalController(IDataApprovalService dataCollectionStatusService)
        {
            _dataCollectionStatusService = dataCollectionStatusService;
        }

        [Authorize(Policy = AppPermissions.ApproveBeneficiaryData)]
        [HttpPost("ApproveBeneficiaries")]
        public async Task<IActionResult> ApproveBeneficiaries([FromBody] BeneficiaryApprovalViewModel model)
        {
            await _dataCollectionStatusService.ApproveBeneficiary(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.ApproveBeneficiaryData)]
        [HttpPost("ApproveInactivateBeneficiaries")]
        public async Task<IActionResult> ApproveInactivateBeneficiaries([FromBody] BeneficiaryApprovalViewModel model)
        {
            await _dataCollectionStatusService.ApproveInactiveBeneficiary(model);
            return Ok();
        }


        [Authorize(Policy = AppPermissions.DisapproveBeneficiaryData)]
        [HttpPost("RecollectBeneficiaries")]
        public async Task<IActionResult> RecollectBeneficiaries([FromBody] BeneficiaryApprovalViewModel model)
        {
            await _dataCollectionStatusService.RecollectBeneficiary(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.ApproveFacilityData)]
        [HttpPost("ApproveFacilities")]
        public async Task<IActionResult> ApproveFacilities([FromBody] FacilityApprovalViewModel model)
        {
            await _dataCollectionStatusService.ApproveFacility(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DisapproveFacilityData)]
        [HttpPost("RecollectFacilities")]
        public async Task<IActionResult> RecollectFacilities([FromBody] FacilityApprovalViewModel model)
        {
            await _dataCollectionStatusService.RecollectFacility(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.ViewBeneficiaryApprovalGrid)]
        [HttpPost("GetSubmittedBeneficiaries")]
        public async Task<PagedResponse<BeneficiaryDataForApproval>> GetSubmittedBeneficiaries([FromBody] SubmittedBeneficiaryQueryModel model)
        {
            return await _dataCollectionStatusService.GetSubmittedBeneficiaries(model);
        }
    }
}
