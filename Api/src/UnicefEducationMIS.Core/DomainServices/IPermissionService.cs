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
    public interface IPermissionService
    {
        Task<PagedResponse<PermissionViewModel>> GetPermissions(BaseQueryModel model);
        Task<PagedResponse<PermissionViewModel>> GetPermissionsByPresetId(PermissionQueryModel model);
        Task<PagedResponse<PermissionPresetViewModel>> GetPermissionPresets(BaseQueryModel model);

    }
}
