using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.Import;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IFacilityService
    {
        Task<PagedResponse<FacilityViewModel>> GetAllWithValue(FacilityQueryModel facilityQueryModel);
        Task<PagedResponse<FacilityViewModel>> GetAllByViewId(FacilityByViewIdQueryModel viewId);
        Task<PagedResponse<FacilityViewModel>> GetAllByInstanceId(FacilityByViewIdQueryModel viewId);
        Task<PagedResponse<FacilityViewModel>> GetAllForDevice(FacilityQueryModel facilityQuery);
        Task<PagedResponse<FacilityObjectViewModel>> GetAll(FacilityQueryModel facilityQuery);
        Task<PagedResponse<FacilityObjectViewModel>> GetAllByBeneficiaryInstance(FacilityQueryModel facilityQuery);
        Task<PagedResponse<FacilityObjectViewModel>> GetAllFilteredData(FacilityGetAllQueryModel facilityQuery); 
        Task<FacilityEditViewModel> GetById(long id,long instanceId);
        Task<bool> IsFacilityCodeExist(string facilityCode);
        Task Add(CreateFacilityViewModel model);
        Task Update(CreateFacilityViewModel model);
        Task Delete(DeleteFacilityViewModel model);
       // Task StartCollectionForAllFacility(long instanceId);
        Task AssignTeacher(AssignTeacherViewModel model);
        Task<ImportResult<FacilityImportViewModel>> ImportFacilities(MemoryStream stream);
        Task<ImportResult<FacilityDynamicCellAddViewModel>> ImportVersionedFacilities(MemoryStream stream, long instanceId);
        Task<byte[]> GetVersionedDataImportTemplate(long instanceId);
        Task<byte[]> ExportFacilities();
        Task<byte[]> ExportVersionedFacilities(long instanceid);
        Task<byte[]> ExportAggFacilities(FacilityAggExportQueryModel model);
        Task<PagedResponse<FacilityObjectViewModel>> GetAllLatest(BaseQueryModel baseQueryModel);
    }
}
