using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IDataApprovalService
    {
        Task ApproveBeneficiary(BeneficiaryApprovalViewModel model);
        Task RecollectBeneficiary(BeneficiaryApprovalViewModel model);

        Task ApproveFacility(FacilityApprovalViewModel model);
        Task ApproveInactiveBeneficiary(BeneficiaryApprovalViewModel model);
        Task RecollectFacility(FacilityApprovalViewModel model);

        Task<PagedResponse<BeneficiaryDataForApproval>> GetSubmittedBeneficiaries(SubmittedBeneficiaryQueryModel model);
        Task<PagedResponse<FacilityViewModel>> GetSubmittedFacilities(SubmittedFacilityQueryModel model);
    }
}
