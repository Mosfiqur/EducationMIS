using System.IO;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Reporting.Models;

namespace UnicefEducationMIS.Service.Report
{
    public class SummaryReportExporter : ISummaryReportExporter
    {
        private readonly IEnvironment _env;

        public SummaryReportExporter(IEnvironment env)
        {
            _env = env;
        }
        public async Task<byte[]> ExportAll(SummaryReport summaryReport)
        {
            var templateFilePath =
                Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(templateFilePath))
            {
                excel.ReadStyles();
                var writer = new SummaryReportWriter(excel);
                await writer.Write(summaryReport);
                return excel.GetBytes();
            }
        }
    }
}
