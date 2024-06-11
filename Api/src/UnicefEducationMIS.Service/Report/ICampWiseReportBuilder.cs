using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Service.Report
{
    public interface ICampWiseReportBuilder
    {
        Task<byte[]> BuildReport(string fileFullPath, List<FacilityViewModel> facilities,
            List<TargetForCampWiseReport> indicators);
    }
}