using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IIndicatorService
    {
        Task Insert(List<AddInstanceIndicatorViewModel> instanceIndicators);
        Task Update(List<AddInstanceIndicatorViewModel> instanceIndicators);
        Task Delete(List<DeleteIndicatorViewModel> indicator);


        Task<PagedResponse<IndicatorSelectViewModel>> GetIndicatorsByInstance(IndicatorsByInstanceQueryModel model);
        Task<PagedResponse<BeneficairyWiseIndicatorViewModel>> GetBeneficiaryIndicators(IndicatorByBeneficairyWiseQueryModel model);
        Task<PagedResponse<FacilityWiseIndicatorViewModel>> GetFacilityIndicators(IndicatorByFacilityWiseQueryModel model);
      

    }
}
