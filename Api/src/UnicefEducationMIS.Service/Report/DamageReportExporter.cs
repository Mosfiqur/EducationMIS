using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Reporting.Models;

namespace UnicefEducationMIS.Service.Report
{
    public class DamageReportExporter : IDamageReportExporter
    {
        private readonly IEnvironment _env;

        public DamageReportExporter(IEnvironment env)
        {
            _env = env;
        }
        public async Task<byte[]> ExportAll(DamageSummary damageSummary, List<PartnerWiseDamage> partnerWiseDamages, List<YearlyDamageSummary> yearlyDamages)
        {
            var templateFilePath =
                Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(templateFilePath))
            {
                excel.ReadStyles();
                var writer = new DamageReportWriter(excel);
                await writer.Write(damageSummary, partnerWiseDamages, yearlyDamages);
                return excel.GetBytes();
            }
        }
    }
}
