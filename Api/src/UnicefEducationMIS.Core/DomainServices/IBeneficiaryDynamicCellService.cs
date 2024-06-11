using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
   public interface IBeneficiaryDynamicCellService
    {
        Task Save(BeneficiaryDynamicCellAddViewModel dynamicColumn);
        Task Delete(BeneficiaryDynamicCellDeleteViewModel dynamicCell);
    }
}
