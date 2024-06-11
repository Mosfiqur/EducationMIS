using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Web.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(Policy = AppPermissions.AddRoles)]
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] RoleViewModel roleVM)
        {
            await _roleService.CreateRole(roleVM);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.EditRoles)]
        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleViewModel roleVM)
        {
            await _roleService.UpdateRole(roleVM);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.ViewRoles)]
        [HttpGet("GetRoles")]
        public async Task<PagedResponse<RoleViewModel>> GetRoles([FromQuery] BaseQueryModel pagingQuery)
        {
            return await _roleService.GetRoles(pagingQuery);
        }

        [Authorize(Policy = AppPermissions.ViewRoles)]
        [HttpGet("GetRoleById")]
        public async Task<RoleViewModel> GetRoleById(int roleId)
        {
            return await _roleService.GetRoleById(roleId);
        }

        [Authorize(Policy = AppPermissions.DeleteRole)]
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            await _roleService.DeleteRole(roleId);
            return Ok();
        }
        /// <summary>
        /// Retrieves list of levels lower than current user. Only admin user gets a full list.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = AppPermissions.ViewLevels)]
        [HttpGet("GetLevels")]
        public async Task<IEnumerable<UserLevelViewModel>> GetLevels()
        {
            return await _roleService.GetLevels();
        }

    }
}
