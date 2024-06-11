using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public class FiveWReportBuilder : IFiveWReportBuilder
    {
        public async Task<byte[]> BuildReport(string fileFullPath, List<FacilityViewModel> facilities,
            List<IndicatorSelectViewModel> indicators)
        {
            byte[] report = null;
            using (var excel = ExcelAdapter.Open(fileFullPath))
            {
                excel.ReadStyles(); 
                var sheetDataWriter = new FiveWReportSheetWriter(excel);
                await sheetDataWriter.Write(facilities, indicators);
                report = excel.GetBytes(); 
            }

            return report;
        }
    }
}
