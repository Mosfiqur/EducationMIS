using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Web.Controllers
{
    public class PermissionController : BaseController
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionPresetRepository _permissionPresetRepository;
        private readonly IPermissionService _permissionService;

        public PermissionController(
            IPermissionRepository permissionRepository, 
            IPermissionPresetRepository permissionPresetRepository,
            IPermissionService permissionService
            )
        {
            _permissionRepository = permissionRepository;
            _permissionPresetRepository = permissionPresetRepository;
            _permissionService = permissionService;
        }

        [Authorize(Policy = AppPermissions.ViewPermissions)]
        [HttpGet("GetPermissions")]
        public async Task<PagedResponse<PermissionViewModel>> GetPermissions([FromQuery] BaseQueryModel model)
        {            
            return await _permissionService.GetPermissions(model);
        }

        [Authorize(Policy = AppPermissions.ViewPermissionsPresets)]
        [HttpGet("GetPermissionPresets")]
        public async Task<PagedResponse<PermissionPresetViewModel>> GetPermissionPresets([FromQuery] BaseQueryModel model)
        {           
            return await _permissionService.GetPermissionPresets(model);
        }

        [Authorize(Policy = AppPermissions.ViewPermissionsPresets)]
        [HttpGet("GetPermissionsByPresetId")]
        public async Task<IEnumerable<Permission>> GetPermissionsByPresetId(int id)
        {
            return await _permissionRepository.GetPermissionsByPresetId(id);
        }


        

    }
}
