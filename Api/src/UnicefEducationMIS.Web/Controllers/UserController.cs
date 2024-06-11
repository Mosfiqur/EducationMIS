using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.User;
using UnicefEducationMIS.Web.ActionResults;

namespace UnicefEducationMIS.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Policy = AppPermissions.AddUsers)]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserViewModel userVM)
        {
            await _userService.CreateUser(userVM);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.UpdateOwnProfileInfo)]
        [HttpPut("UpdateProfileInfo")]
        public async Task<IActionResult> UpdateProfileInfo([FromBody] ProfileInfoViewModel profileInfoVM)
        {
            await _userService.UpdateProfileInfo(profileInfoVM);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.EditUsers)]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserViewModel userVM)
        {
            await _userService.UpdateUser(userVM);
            return Ok();
        }
             
        [Authorize(Policy = AppPermissions.ViewUsers)]
        [HttpPost("GetUsers")]
        public async Task<PagedResponse<UserViewModel>> GetUsers([FromBody] UserQueryModel pagingQuery)
        {
            return await _userService.GetUsers(pagingQuery);
        }

        [Authorize(Policy = AppPermissions.ViewUsers)]
        [HttpGet("GetTeachers")]
        public async Task<PagedResponse<UserViewModel>> GetTeachers([FromQuery] BaseQueryModel pagingQuery)
        {
            return await _userService.GetTeachers(pagingQuery);
        }

        [Authorize(Policy = AppPermissions.AddUsers)]
        [HttpGet("GetUserById")]
        public async Task<UserViewModel> GetUserById(int userId)
        {
            return await _userService.GetUserById(userId);
        }

        [Authorize(Policy = AppPermissions.DeleteUser)]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _userService.DeleteUser(userId);
            return Ok();
        }
        [Authorize(Policy = AppPermissions.DeleteUser)]
        [HttpPost("DeleteUsers")]
        public async Task<IActionResult> DeleteUsers([FromBody]List<int> userIds)
        {
            await _userService.DeleteUser(userIds);
            return Ok();
        }
        /// <summary>
        /// Users with the appropriate privilege can reset other user's password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Policy = AppPermissions.ResetUsersPassword)]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel model)
        {
            await _userService.ResetPassword(model);
            return Ok();
        }


        [Authorize(Policy = AppPermissions.AddUserDynamicCells)]
        [HttpPost("InsertDynamicCell")]
        public async Task<IActionResult> InsertDynamicCells([FromBody] UserDynamicCellInsertViewModel model)
        {
            await _userService.InsertDynamicCell(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.UpdateUserDynamicCells)]
        [HttpPut("UpdateDynamicCell")]
        public async Task<IActionResult> UpdateDynamicCells([FromBody] UserDynamicCellInsertViewModel model)
        {
            await _userService.UpdateDynamicCell(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteUserDynamicCells)]
        [HttpPost("DeleteDynamicCell")]
        public async Task<IActionResult> DeleteDynamicCells(UserDynamicCellViewModel model)
        {
            await _userService.DeleteDynamicCell(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.ExportFilteredUsers)]
        [HttpPost("ExportFilteredUsers")]
        public async Task<IActionResult> ExportFilteredUsers([FromBody] UserQueryModel model)
        {
            var bytes = await _userService.ExportFilteredUsers(model);
            return new DocumentResult(bytes, FileNames.FilteredUsersExportFilename);
        }

    }
}
