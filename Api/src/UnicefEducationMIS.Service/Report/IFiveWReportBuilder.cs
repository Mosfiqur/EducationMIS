using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public interface IFiveWReportBuilder
    {
        Task<byte[]> BuildReport(string fileFullPath, List<FacilityViewModel> facilities,
            List<IndicatorSelectViewModel> indicators);
    }
}