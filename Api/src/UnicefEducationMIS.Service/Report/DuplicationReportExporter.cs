using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Reporting.Models;

namespace UnicefEducationMIS.Service.Report
{
    public class DuplicationReportExporter : IDuplicationReportExporter
    {
        private readonly IEnvironment _env;

        public DuplicationReportExporter(IEnvironment env)
        {
            _env = env;
        }

        public async Task<byte[]> ExportAll(List<FailityWiseDuplicate> facilityWiseDupliates, List<StudentWiseDuplicate> studentWiseDuplicates)
        {
            var templateFilePath =
                Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(templateFilePath))
            {
                excel.ReadStyles();
                var writer = new DuplicationReportWriter(excel);
                await writer.Write(facilityWiseDupliates, studentWiseDuplicates);
                return excel.GetBytes();
            }
        }
    }
}
