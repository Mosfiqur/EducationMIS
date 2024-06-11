using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Interfaces
{
    public interface IFacilityVersionImporter
    {
        Task<ImportResult<FacilityDynamicCellAddViewModel>> Import(Stream stream, long instanceId);
    }
}