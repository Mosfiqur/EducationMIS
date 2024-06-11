using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ViewModel;
using System.Linq;
using UnicefEducationMIS.Web.ActionResults;

namespace UnicefEducationMIS.Web.Controllers
{
    public class BeneficiaryController : BaseController
    {
        private readonly IBeneficiaryService _beneficiaryService;


        public BeneficiaryController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetById/{id}/{instanceId}")]
        public async Task<ActionResult> GetById([FromRoute] long id, long instanceId)
        {
            var data = await _beneficiaryService.GetById(id, instanceId);
            return Ok(data);
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetByFacilityId")]
        public async Task<ActionResult> GetByFacilityId([FromQuery] BeneficiaryByFacilityIdQueryModel beneficiaryByFacilityId)
        {
            var data = await _beneficiaryService.GetByFacilityId(beneficiaryByFacilityId);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewBeneficiaryGrid)]
        [HttpPost("GetAllByViewId")]
        public async Task<ActionResult> GetAllByViewId([FromBody] BeneficiaryByViewIdQueryModel beneficiaryByViewId)
        {
            var data = await _beneficiaryService.GetAllByViewId(beneficiaryByViewId);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewBeneficiaryGrid)]
        [HttpPost("GetAllByInstanceId")]
        public async Task<ActionResult> GetAllByInstanceId([FromBody] BeneficiaryByViewIdQueryModel beneficiaryByViewId)
        {
            var data = await _beneficiaryService.GetAllByInstanceId(beneficiaryByViewId);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.EditBeneficiaryData)]
        [HttpPost("Add")]
        public async Task<ActionResult> AddBeneficiary([FromBody] BeneficiaryAddViewModel beneficiary)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                throw new Core.Exceptions.DomainException(string.Join(',', errors));
            }
            
            var newBeneficiary = await _beneficiaryService.Add(beneficiary);
            if (newBeneficiary != null)
                return Ok(newBeneficiary);
            else
                throw new Core.Exceptions.DomainException("Value can not be inserted.");
        }

        [Authorize(Policy = AppPermissions.EditBeneficiaryData)]
        [HttpPost("Update")]
        public async Task<ActionResult> UpdateBeneficiary([FromBody] BeneficiaryAddViewModel beneficiary)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                throw new Core.Exceptions.DomainException(string.Join(',', errors));
            }
            await _beneficiaryService.Update(beneficiary);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.CanActivateBeneficiaryData)]
        [HttpPost("ActivateBeneficiary")]
        public async Task<ActionResult> ActivateBeneficiary([FromBody] BeneficiaryStatusChangeViewModel model)
        {
            await _beneficiaryService.ActivateBeneficiary(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeactivateBeneficiaryData)]
        [HttpPost("DeactivateBeneficiary")]
        public async Task<ActionResult> DeactivateBeneficiary([FromBody] BeneficiaryStatusChangeViewModel deactivateBeneficiary)
        {
            await _beneficiaryService.InactivateBeneficiary(deactivateBeneficiary);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteBeneficiaryData)]
        [HttpPost("Delete")]
        public async Task<ActionResult> DeleteBeneficiary([FromBody] BeneficiaryStatusChangeViewModel model)
        {
            await _beneficiaryService.Delete(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.ImportBeneficiariesVersionData)]
        [HttpPost("ImportBeneficiariesVersionData")]
        public async Task<ActionResult> ImportBeneficiariesVersionData([FromForm] VersionDataViewModel data)
        {
            ImportResult<BeneficiaryVersionDataViewModel> result = null;
            var ms = new MemoryStream();
            data.file.CopyTo(ms);
            ms.Position = 0;
            result = await _beneficiaryService.ImportBeneficiariesVersionData(ms, data.InstanceId);
            return Ok(result);
        }


        [Authorize(Policy = AppPermissions.DownloadBeneficiaryVersionDataImportFile)]
        [HttpPost("DownloadVersionDataImportTemplate/{instanceId}")]
        public async Task<IActionResult> DownloadVersionDataImportTemplate([FromRoute] long instanceId)
        {
            byte[] fileAsBytes = await _beneficiaryService.GetVersionDataImportTemplate(instanceId);
            return File(fileAsBytes, FileNames.ContentTypeExcel);
        }
        
        [Authorize(Policy = AppPermissions.ExportVersionedBeneficiaries)]
        [HttpPost("ExportVersionedBeneficiaries/{instanceId}")]
        public async Task<IActionResult> ExportVersionedBeneficiaries([FromRoute] long instanceId)
        {
            if (instanceId == 0)
            {
                return BadRequest(Messages.InvalidInstanceId);
            }
            byte[] fileAsBytes = await _beneficiaryService.ExportBeneficiaries(instanceId);
            return new DocumentResult(fileAsBytes, FileNames.BeneficiaryExportsFilename);
        }


        [Authorize(Policy = AppPermissions.ExportAggBeneficiaries)]
        [HttpPost("ExportAggBeneficiaries")]
        public async Task<IActionResult> ExportAggBeneficiaries([FromBody] BeneficiaryAggExportQueryModel model)
        {
            byte[] fileAsBytes = await _beneficiaryService.ExportAggBeneficiaries(model);
            return new DocumentResult(fileAsBytes, FileNames.BeneficiaryAggExportsFilename);
        }

    }
}
