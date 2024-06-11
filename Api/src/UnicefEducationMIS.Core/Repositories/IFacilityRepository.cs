using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Views;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Reporting.Models;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IFacilityRepository : ICountable<Facility> , IBaseRepository<Facility, long>
    {
        Task<PagedResponse<FacilityViewModel>> GetAllWithValue(FacilityQueryModel facilityQueryModel);
        Task<PagedResponse<FacilityViewModel>> GetFacilityByIndicator(FacilityQueryModel facilityQueryModel, List<long> indicators);
        Task<PagedResponse<FacilityViewModel>> GetAllByViewId(FacilityByViewIdQueryModel viewId);
        Task<PagedResponse<FacilityViewModel>> GetAllByInstanceId(FacilityByViewIdQueryModel viewId);
        Task<PagedResponse<FacilityViewModel>> GetAllForDevice(FacilityQueryModel beneficiaryQuery);
        Task<PagedResponse<FacilityObjectViewModel>> GetAll(BaseQueryModel facilityQuery,long instanceId);
        Task<PagedResponse<FacilityObjectViewModel>> GetAll(FacilityQueryModel facilityQuery);
        Task<PagedResponse<FacilityObjectViewModel>> GetAllFilteredData(FacilityGetAllQueryModel facilityQuery); 

        IQueryable<FacilityView> GetFacilityView();
        Task<FacilityEditViewModel> GetById(long id, long instanceId);
        Task<bool> IsFacilityCodeExist(string facilityCode);
        new Task Add(Facility facility);
        Task<string> GetMaxFacilityCode();
        new Task Update(Facility facility);
        Task Delete(List<long> facilityIds);
        Task StartCollectionForAllFacility(long instanceId);
        Task StartCollectionForAllFacility(long instanceId, List<long> facilityIds);
        Task<List<FacilityViewModel>> GetAllFacilitites(Expression<Func<FacilityView, bool>> filter);

        IQueryable<FacilityRawViewModel> GetFacilityByInstance(long instanceId);
        IQueryable<FacilityRawViewModel> GetFacilityByInstanceForImportTemplate(long instanceId);
        List<FacilityRawViewModel> GetFacilityByInstances(List<long> instanceIds);
        List<FacilityViewModel> SetFacilityViewModel(List<FacilityRawViewModel> facilityRawData);

        Task<PagedResponse<FacilityObjectViewModel>> ExportAll(BaseQueryModel facilityQuery);

        Task Import(Facility facility);

        Task<PagedResponse<FacilityObjectViewModel>> GetPaginatedFacilityObject(FacilityQueryModel queryModel);
        Task<PagedResponse<FacilityIndicatorWithValue>> GetFacilityIndicatorWithValue(FacilityQueryModel facilityQueryModel, List<long> indicators);
        List<FacilityRawViewModel> FacilityRawAsync(long instanceId, out List<FacilityDynamicCell> baseDynamicCellData, out List<FacilityDataCollectionStatus> dataCollectionStatus);
    }
}
