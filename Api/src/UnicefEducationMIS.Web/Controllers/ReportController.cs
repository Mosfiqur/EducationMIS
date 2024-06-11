using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel.Reporting;
using UnicefEducationMIS.Web.ActionResults;

namespace UnicefEducationMIS.Web.Controllers
{
    public class ReportController:BaseController
    {
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IReportService _reportService;

        public ReportController(IWebHostEnvironment hostEnv, IReportService reportService)
        {
            _hostEnv = hostEnv;
            _reportService = reportService;
        }

        [Authorize(Policy = AppPermissions.Download5WReport)]
        [HttpPost("Generate5WReport/{facilityInstanceId}")]
        public async Task<IActionResult> Generate5WReport(long facilityInstanceId)
        {
            var fileFullPath = Path.Combine(_hostEnv.WebRootPath, FileNames.ReportTemplateFolder, FileNames.FiveWTemplateFile);
            byte[] report = await _reportService.Generate5wReport(fileFullPath,facilityInstanceId); //System.IO.File.ReadAllBytes(fileFullPath);
           
            //return File(report, FileNames.ContentTypeExcel);
            return new DocumentResult(report, FileNames.FiveWReport);
        }

        [Authorize(Policy = AppPermissions.GetCampWiseReport)]
        [HttpPost("GenerateCampWiseReport/{facilityInstanceId}")]
        public async Task<IActionResult> GenerateCampWiseReport(long facilityInstanceId)
        {
            var fileFullPath = Path.Combine(_hostEnv.WebRootPath, FileNames.ReportTemplateFolder, FileNames.CampWiseReportTemplateFile);
            byte[] report = await _reportService.GenerateCampWiseReport(fileFullPath, facilityInstanceId); //System.IO.File.ReadAllBytes(fileFullPath);

            //return File(report, FileNames.ContentTypeExcel);
            return new DocumentResult(report, FileNames.CampWiseReport);
        }

        [Authorize(Policy = AppPermissions.GetDuplicationReport)]
        [HttpPost("GetDuplicationReport")]
        public async Task<IActionResult> GetDuplicationReport([FromBody] DuplicationReportQueryModel model)
        {
            var bytes =  await _reportService.GetDuplicationReport(model);
            return new DocumentResult(bytes, FileNames.DuplicationReportFilename);
        }

        [Authorize(Policy = AppPermissions.GetGapAnalysisReport)]
        [HttpPost("GetGapAnalysisReport")]
        public async Task<IActionResult> GetGapAnalysisReport([FromBody] GapAnalysisReportQueryModel model)
        {
            var bytes = await _reportService.GetGapAnalysisReport(model);
            return new DocumentResult(bytes, FileNames.GapAnalysisReportFilename);
        }

        [Authorize(Policy = AppPermissions.GetDamageReport)]
        [HttpPost("GetDamageReport")]
        public async Task<IActionResult> GetDamageReport([FromBody] DamageReportQueryModel model)
        {
            var bytes = await _reportService.GetDamageReport(model);
            return new DocumentResult(bytes, FileNames.DamageReportFilename);
        }

        [Authorize(Policy = AppPermissions.GetSummaryReport)]
        [HttpPost("GetSummaryReport")]
        public async Task<IActionResult> GetSummaryReport([FromBody] SummaryReportQueryModel model)
        {
            var bytes = await _reportService.GetSummaryReport(model);
            return new DocumentResult(bytes, FileNames.SummaryReportFilename);
        }
    }
}
