using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public interface IBeneficiaryVersionExporter
    {
        Task<byte[]> ExportAll(List<BeneficairyObjectViewModel> beneficiaries);
    }
}
