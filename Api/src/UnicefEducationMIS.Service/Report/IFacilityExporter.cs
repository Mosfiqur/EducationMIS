using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public interface IFacilityExporter
    {
        Task<byte[]> ExportFacilities(List<FacilityViewModel> facilities);
        Task<byte[]> ExportVersionedFacilities(List<FacilityViewModel> facilities, List<IndicatorSelectViewModel> indicators);
        Task<byte[]> ExportAggregatedFacilities(List<FacilityViewModel> facilities, List<IndicatorSelectViewModel> indicators);
    }
}
