using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IBeneficiaryDataCollectionStatusRepository: IBaseRepository<BeneficiaryDataCollectionStatus, long>
    {
        Task ApproveBeneficiary(BeneficiaryApprovalViewModel model);
        Task RemoveDeactivateBeneficiary(BeneficiaryApprovalViewModel model);
        Task<List<int>> BeneficiaryDeletedBy(BeneficiaryApprovalViewModel model);
        Task ApproveDeactiveBeneficiary(BeneficiaryApprovalViewModel model);

    }
}
