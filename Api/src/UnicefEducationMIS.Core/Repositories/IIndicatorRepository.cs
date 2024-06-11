using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IIndicatorRepository : IBaseRepository<InstanceIndicator, long>
    {
        Task Insert(List<InstanceIndicator> instanceIndicators);
        Task Update(List<InstanceIndicator> instanceIndicators);
        Task Delete(List<DeleteIndicatorViewModel> indicator);


        //Task<PagedResponse<IndicatorSelectViewModel>> GetIndicatorsByEntityType(IndicatorsByEntityTypeQueryModel model);


        Task<PagedResponse<IndicatorSelectViewModel>> GetIndicatorsByInstance(IndicatorsByInstanceQueryModel model);
        Task<PagedResponse<BeneficairyWiseIndicatorViewModel>> GetBeneficiaryIndicators(IndicatorByBeneficairyWiseQueryModel model);
        Task<PagedResponse<FacilityWiseIndicatorViewModel>> GetFacilityIndicators(IndicatorByFacilityWiseQueryModel model);

        Task AddInstanceIndicator(Instance instance);
        //Task AddInstanceIndicator(long instanceId);
        //Task UpdateInstanceIndicator(long instanceId);
        Task<List<IndicatorSelectViewModel>> GetAutoCalculatedIndicator(EntityType entityType);
        Task<List<IndicatorSelectViewModel>> GetIndicatorsByInstances(List<long> instanceIds);
    }
}
