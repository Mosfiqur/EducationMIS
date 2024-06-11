using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public class BeneficiaryVersionDataImportFileBuilder : IBeneficiaryVersionDataImportFileBuilder
    {
        private readonly IEnvironment _env;

        public BeneficiaryVersionDataImportFileBuilder(IEnvironment env)
        {
            _env = env;
        }    

        public async Task<byte[]> BuildFile(List<IndicatorSelectViewModel> indicators,
            List<BeneficiaryRawViewModel> allBeneficiaries)
        {
            var filteredIndicators = indicators
                .ToList();

            byte[] fileBytes = null;
            var baseTemplateFilePath =
                Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(baseTemplateFilePath))
            {
                excel.ReadStyles();
                BeneficiaryVersionDataImportSheetsWriter writer = new BeneficiaryVersionDataImportSheetsWriter(excel);
                await writer.Write(filteredIndicators, allBeneficiaries);
                fileBytes = excel.GetBytes(); 
            }
            return fileBytes; 
        }
    }
}
