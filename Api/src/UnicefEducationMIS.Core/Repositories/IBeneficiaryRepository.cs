using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Views;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IBeneficiaryRepository : ICountable<Beneficiary>, IBaseRepository<Beneficiary, long>
    {
       
        Task<PagedResponse<BeneficiaryViewModel>> GetByFacilityId(BeneficiaryByFacilityIdQueryModel beneficiaryByFacilityId, long maxFacilityInstanceId);
        Task<PagedResponse<BeneficiaryViewModel>> GetAllByViewId(BeneficiaryByViewIdQueryModel viewId, long maxFacilityInstanceId);
        Task<List<BeneficiaryRawViewModel>> GetAggregateBeneficiary(List<long> instanceIds);
        Task<PagedResponse<BeneficiaryViewModel>> GetAllByInstanceId(BeneficiaryByViewIdQueryModel model, long maxFacilityInstanceId);
        new Task<BeneficiaryEditViewModel> GetById(long id, long instanceId);
        
        IQueryable<BeneficiaryView> GetBeneficiaryView();
  
        Task<Beneficiary> GetBeneficiaryByProgressId(string progressId);
        Task<Beneficiary> Add(Beneficiary beneficiary);
       // new Task Update(Beneficiary beneficiary);
      
        Task Revert(long id);
        
        Task<List<BeneficiaryRawViewModel>> GetAllBeneficiariesForVersionDataTemplate(long instanceId,
            long maxFacilityInstanceId);
        void AutoUpdateFacilityIndicators(FacilityApprovalViewModel model);
        IQueryable<BeneficiaryRawViewModel> GetBeneficiaryByInstance(long instanceId, long maxFacilityInstanceId);
        IQueryable<BeneficiaryRawViewModel> GetBeneficiaryByInstances(List<long> instanceIds, long maxFacilityInstanceId);
        List<BeneficiaryViewModel> SetBeneficiaryViewModel(List<BeneficiaryRawViewModel> beneficiaryRawData);
      
        Task<List<BeneficiaryView>> GetBeneficiaryBasicData(List<long> beneficiaryIds, long instanceId);
        Task StartCollectionForSelectedBeneficiaries(long instanceId, List<long> beneficiaryIds);
    }
}
#region commentedCode
// Task<string> StartCollectionForBeneficiaryById(long beneficairyId,long instanceId);
// Task<List<BeneficiaryPreCollectionViewModel>> GetForDataCollectionByFacilityId(long facilityId, long instanceId);
//   Task<PagedResponse<BeneficiaryViewModel>> GetAllByColumn(BeneficiaryByColumnIdQueryModel beneficiaryByColumn);

#endregion
