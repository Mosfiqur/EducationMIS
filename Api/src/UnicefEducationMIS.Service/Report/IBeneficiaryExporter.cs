using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public interface IBeneficiaryExporter
    {
        Task<byte[]> ExportAll(List<BeneficiaryViewModel> beneficiaries);
        Task<byte[]> ExportAll(List<BeneficiaryViewModel> beneficiaries, List<IndicatorSelectViewModel> indicators);
        Task<byte[]> ExportAggBeneficiaries(List<BeneficiaryViewModel> beneficiaries, List<IndicatorSelectViewModel> indicators);

        
    }
}
