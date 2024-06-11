using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IRoleService
    {
        Task CreateRole(RoleViewModel roleVM);
        Task UpdateRole(RoleViewModel roleVM);        
        Task<PagedResponse<RoleViewModel>> GetRoles(BaseQueryModel pagingQuery);
        Task<RoleViewModel> GetRoleById(int roleId);
        Task DeleteRole(int roleId);
        Task<IEnumerable<UserLevelViewModel>> GetLevels();
    }
}
