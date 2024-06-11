using System.IO;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Interfaces
{
    public interface IBeneficiaryImporter
    {
        Task<ImportResult<BeneficiaryAddViewModel>> Import(Stream stream);
    }
}