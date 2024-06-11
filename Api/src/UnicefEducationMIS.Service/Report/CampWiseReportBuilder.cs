using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Service.Report
{
    public class CampWiseReportBuilder : ICampWiseReportBuilder
    {
        public async Task<byte[]> BuildReport(string fileFullPath, List<FacilityViewModel> facilities,
            List<TargetForCampWiseReport> indicators)
        {
            byte[] report = null;
            using (var excel = ExcelAdapter.Open(fileFullPath))
            {
                excel.ReadStyles(); 
                var sheetDataWriter = new CampWiseReportSheetWriter(excel);
                await sheetDataWriter.Write(facilities, indicators);
                report = excel.GetBytes(); 
            }

            return report;
        }
    }
}
