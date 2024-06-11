using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Web.ActionResults;
using UnicefEducationMIS.Web.Helpers;

namespace UnicefEducationMIS.Web.Controllers
{
    public class FacilityController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IFacilityService _facilityService;
        public FacilityController(IFacilityService facilityService, IWebHostEnvironment hostEnvironment)
        {
            _facilityService = facilityService;
            _hostEnvironment = hostEnvironment;
        }


        //[Authorize(Policy = AppPermissions.ViewFacilityGrid)]
        //[HttpGet("GetAllForGrid")]
        //public async Task<ActionResult> GetAllForGrid([FromQuery] FacilityQueryModel facilityQueryModel)
        //{
        //    var data = await _facilityService.GetAllWithValue(facilityQueryModel);
        //    return Ok(data);
        //}

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetAllForDevice")]
        public async Task<ActionResult> GetAllForDevice([FromQuery] FacilityQueryModel baseQueryModel)
        {
            var data = await _facilityService.GetAllForDevice(baseQueryModel);
            return Ok(data);
        }
        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll([FromQuery] FacilityQueryModel baseQueryModel)
        {
            var data = await _facilityService.GetAll(baseQueryModel);
            return Ok(data);
        }
        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetAllLatest")]
        public async Task<ActionResult> GetAllLatest([FromQuery] BaseQueryModel baseQueryModel)
        {
            var data = await _facilityService.GetAllLatest(baseQueryModel);
            return Ok(data);
        }
        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetAllByBeneficiaryInstance")]
        public async Task<ActionResult> GetAllByBeneficiaryInstance([FromQuery] FacilityQueryModel baseQueryModel)
        {
            var data = await _facilityService.GetAllByBeneficiaryInstance(baseQueryModel);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewFacilityGrid)]
        [HttpPost("GetAllFilteredData")]
        public async Task<ActionResult> GetAllFilteredData([FromBody] FacilityGetAllQueryModel baseQueryModel)
        {
            var data = await _facilityService.GetAllFilteredData(baseQueryModel);
            return Ok(data);
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetById/{id}/{instanceId}")]
        public async Task<ActionResult> GetById(long id, long instanceId)
        {
            var data = await _facilityService.GetById(id, instanceId);
            return Ok(data);
        }
        [Authorize(Policy = AppPermissions.ViewFacilityGrid)]
        [HttpPost("GetAllByViewId")]
        public async Task<ActionResult> GetAllByViewId([FromBody] FacilityByViewIdQueryModel model)
        {
            var data = await _facilityService.GetAllByViewId(model);
            return Ok(data);
        }
        [Authorize(Policy = AppPermissions.ViewFacilityGrid)]
        [HttpPost("GetAllByInstanceId")]
        public async Task<ActionResult> GetAllByInstanceId([FromBody] FacilityByViewIdQueryModel model)
        {
            var data = await _facilityService.GetAllByInstanceId(model);
            return Ok(data);
        }
        [Authorize(Policy = AppPermissions.EditFacilityData)]
        [HttpPost("Add")]
        public async Task<IActionResult> AddFacility([FromBody] CreateFacilityViewModel createFacilityViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                throw new Core.Exceptions.DomainException(string.Join(',', errors));
            }
            await _facilityService.Add(createFacilityViewModel);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.EditFacilityData)]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateFacility([FromBody] CreateFacilityViewModel createFacilityViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                throw new Core.Exceptions.DomainException(string.Join(',', errors));
            }
            await _facilityService.Update(createFacilityViewModel);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteFacilityData)]
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteFacility([FromBody] DeleteFacilityViewModel model)
        {
            await _facilityService.Delete(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.EditFacilityData)]
        [HttpPost("AssignTeacher")]
        public async Task<IActionResult> AssignTeacher([FromBody] AssignTeacherViewModel assignTeacherViewModel)
        {
            await _facilityService.AssignTeacher(assignTeacherViewModel);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.ImportFacilitiesVersionData)]
        [HttpPost("ImportFacilitiesVersionData")]
        public async Task<ActionResult> ImportFacilitiesVersionData([FromForm] VersionedDataImportModel model)
        {
            var stream = new MemoryStream();
            model.File.CopyTo(stream);
            stream.Position = 0;
            var result = await _facilityService.ImportVersionedFacilities(stream, model.InstanceId);
            return Ok(result);
        }

        [Authorize(Policy = AppPermissions.DownloadFacilityImportFile)]
        [HttpPost("DownloadVersionedDataImportTemplate")]
        public async Task<IActionResult> DownloadVersionedDataImportTemplate([FromBody] DownloadFacilityVersionTemplateViewModel model)
        {
            var bytes = await _facilityService.GetVersionedDataImportTemplate(model.InstanceId);
            return await Task.FromResult(new DocumentResult(bytes, FileNames.FacilityVersionedImportTemplate));
        }

        [Authorize(Policy = AppPermissions.ExportVersionedFacilities)]
        [HttpPost("ExportVersionedFacilities/{instanceId}")]
        public async Task<IActionResult> ExportVersionedFacilities([FromRoute] long instanceId)
        {
            if (instanceId == 0)
            {
                return BadRequest(Messages.InvalidInstanceId);
            }
            byte[] fileAsBytes = await _facilityService.ExportVersionedFacilities(instanceId);
            return new DocumentResult(fileAsBytes, FileNames.FacilityExportsFilename);
        }


        [Authorize(Policy = AppPermissions.ExportAggFacilities)]
        [HttpPost("ExportAggFacilities")]
        public async Task<IActionResult> ExportAggFacilities([FromBody] FacilityAggExportQueryModel model)
        {
            byte[] fileAsBytes = await _facilityService.ExportAggFacilities(model);
            return new DocumentResult(fileAsBytes, FileNames.FacilityAggExportsFilename);
        }
    }
}