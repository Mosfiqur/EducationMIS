using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Repositories
{
   public interface IBeneficiaryDynamicCellRepository
    {
        Task Save(BeneficiaryDynamicCellAddViewModel model);
        Task Revert(BeneficiaryDynamicCellAddViewModel model);
        Task Delete(BeneficiaryDynamicCellDeleteViewModel dynamicCell);        
        Task ImportVersionData(List<BeneficiaryDynamicCellAddViewModel> items, CollectionStatus collectionStatus, long instanceId);
    }
}
