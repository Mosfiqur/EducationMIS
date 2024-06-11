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
   public interface IDynamicColumnRepositories : IBaseRepository<EntityDynamicColumn, long>
    {
        Task<EntityDynamicColumn> SaveDynamicColumn(EntityDynamicColumn entityDynamicColumn);
        Task<PagedResponse<DynamicPropertiesViewModel>> GetPaginatedColumn(DynamicPropertiesBySearchParamQueryModel model);
        Task<PagedResponse<DynamicPropertiesViewModel>> GetPaginatedNumericColumn(DynamicPropertiesBySearchParamQueryModel model); 
        Task<PagedResponse<DynamicPropertiesViewModel>> GetPaginatedColumnForIndicator(DynamicPropertiesByInstanceIdQueryParam model);
    }
}
