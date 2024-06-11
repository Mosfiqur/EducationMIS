using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Interfaces
{
    public interface IBeneficiaryVersionImporter
    {
        Task<ImportResult<BeneficiaryVersionDataViewModel>> Import(Stream stream , long instanceId);

        List<long> NewInserts();
    }
}
