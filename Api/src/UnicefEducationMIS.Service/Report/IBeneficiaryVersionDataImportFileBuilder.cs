using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public interface IBeneficiaryVersionDataImportFileBuilder
    {
        Task<byte[]> BuildFile(List<IndicatorSelectViewModel> indicators,
            List<BeneficiaryRawViewModel> allBeneficiaries);
    }
}