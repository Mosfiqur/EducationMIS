using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IDynamicPropertiesService
    {
        Task<EntityDynamicColumn> SaveDynamicColumn(DynamicColumnViewModel dynamicColumnViewModel);
        Task<DynamicColumnViewModel> GetById(long id);
        Task<PagedResponse<DynamicPropertiesViewModel>> GetPaginatedColumn(DynamicPropertiesBySearchParamQueryModel dynamicPropertiesBySearchModel);
        Task<PagedResponse<DynamicPropertiesViewModel>> GetPaginatedNumericColumn(DynamicPropertiesBySearchParamQueryModel dynamicPropertiesBySearchModel);
        Task<PagedResponse<DynamicPropertiesViewModel>> GetPaginatedColumnForIndicator(DynamicPropertiesByInstanceIdQueryParam model);
        Task DeleteDynamicColumn(long entityDynamicColumnId);
    }
}
