using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.Framework;
using UnicefEducationMIS.Core.ViewModel.User;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IUserService : ICountable<User>
    {
        Task CreateUser(UserViewModel userVM);
        Task UpdateUser(UserViewModel userVM);        
        Task<PagedResponse<UserViewModel>> GetUsers(UserQueryModel pagingQuery);
        Task<PagedResponse<UserViewModel>> GetTeachers(BaseQueryModel pagingQuery);        
        Task<UserViewModel> GetUserById(int userId);
        Task DeleteUser(int userId);
        Task DeleteUser(List<int> userIds);
        Task ResetPassword(PasswordResetViewModel model);
        Task ChangePassword(PasswordChangeViewModel model);
        Task UpdateProfileInfo(ProfileInfoViewModel userVM);

        Task InsertDynamicCell(UserDynamicCellInsertViewModel model);
        Task UpdateDynamicCell(UserDynamicCellInsertViewModel model);
        Task DeleteDynamicCell(UserDynamicCellViewModel model);

        Task<byte[]> ExportFilteredUsers(UserQueryModel model);
    }
}
