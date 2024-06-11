using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Reporting.Models;

namespace UnicefEducationMIS.Service.Report
{
    public class GapAnalysisReportExporter : IGapAnalysisReportExporter
    {
        private readonly IEnvironment _env;

        public GapAnalysisReportExporter(IEnvironment env)
        {
            _env = env;
        }

        public async Task<byte[]> ExportAll(List<GapAnalysisReport> data)
        {
            var templateFilePath =
               Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(templateFilePath))
            {
                excel.ReadStyles();
                var writer = new GapAnalysisReportWriter(excel);
                await writer.Write(data);
                return excel.GetBytes();
            }        
        }
    }
}
