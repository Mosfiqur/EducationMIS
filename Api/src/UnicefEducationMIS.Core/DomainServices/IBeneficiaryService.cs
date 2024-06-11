using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Models.Views;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IBeneficiaryService
    {
        Task<PagedResponse<BeneficiaryViewModel>> GetByFacilityId(BeneficiaryByFacilityIdQueryModel queryModel);
        Task<PagedResponse<BeneficiaryViewModel>> GetAllByViewId(BeneficiaryByViewIdQueryModel viewId);
        Task<PagedResponse<BeneficiaryViewModel>> GetAllByInstanceId(BeneficiaryByViewIdQueryModel model);
        Task<BeneficiaryEditViewModel> GetById(long id, long instanceId);
        
        Task<BeneficiaryAddViewModel> Add(BeneficiaryAddViewModel beneficiary);
        Task Update(BeneficiaryAddViewModel beneficiary);
        Task ActivateBeneficiary(BeneficiaryStatusChangeViewModel model);
        Task InactivateBeneficiary(BeneficiaryStatusChangeViewModel model);
      //  Task BeneficiaryStatusChange(BeneficiaryStatusChangeViewModel beneficiaryDataChangeStatus);
        Task Delete(BeneficiaryStatusChangeViewModel model);
        
        Task<ImportResult<BeneficiaryVersionDataViewModel>> ImportBeneficiariesVersionData(Stream ms,long instanceId);
        string GetImportTemplatePath();
        Task<byte[]> GetVersionDataImportTemplate(long instanceId);
       
        Task<byte[]> ExportBeneficiaries(long instanceId);
        Task<byte[]> ExportAggBeneficiaries(BeneficiaryAggExportQueryModel model);
    }
}

#region CommentedCode
// Task<string> StartCollectionForBeneficiaryById(long beneficairyId, long instanceId);
//  Task<PagedResponse<BeneficiaryViewModel>> GetAllByColumn(BeneficiaryByColumnIdQueryModel beneficiaryByColumn);
//Task<List<BeneficiaryPreCollectionViewModel>> GetForDataCollectionByFacilityId(long facilityId, long instanceId);

#endregion