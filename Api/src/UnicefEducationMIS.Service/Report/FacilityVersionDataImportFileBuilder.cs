using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public class FacilityVersionDataImportFileBuilder : IFacilityVersionDataImportFileBuilder
    {
        private readonly IEnvironment _env;

        public FacilityVersionDataImportFileBuilder(IEnvironment env)
        {
            _env = env;
        }

        public async Task<byte[]> BuildFile(List<IndicatorSelectViewModel> indicators,
            List<FacilityViewModel> allFacilities)
        {
            byte[] fileBytes = null;
            var baseTemplateFilePath =
                Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(baseTemplateFilePath))
            {
                excel.ReadStyles();
                var writer = new FacilityVersionDataImportSheetsWriter(excel);
                await writer.Write(indicators, allFacilities);
                fileBytes = excel.GetBytes();
            }
            return fileBytes;
        }
    }
}