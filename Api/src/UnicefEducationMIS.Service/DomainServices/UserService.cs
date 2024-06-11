using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.Settings;
using UnicefEducationMIS.Core.Specifications;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.User;
using UnicefEducationMIS.Data;
using UnicefEducationMIS.Service.Helpers;
using UnicefEducationMIS.Service.Report;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class UserService : IUserService
    {
        private readonly ICurrentLoginUserService _currentUserService;
        private readonly IEnvironment _env;
        private readonly IDynamicPropertiesService _dynamicPropertiesService;
        private readonly IUserExporter _userExporter;

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        private readonly IMapper _mapper;
        private readonly UnicefEduDbContext _dbContext;        
        private readonly MailHelper _mailHelper;
        private readonly AppSettings _appSettings;

        private Expression<Func<User, bool>> _getTeachersFilter;
        private readonly IUserDynamicCellRepository _userDynamicCellRepository;

        public UserService(
            ICurrentLoginUserService currentUserService,
            IEnvironment env,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IMapper mapper,
            UnicefEduDbContext dbContext,
            
            MailHelper mailHelper,
            AppSettings appSettings, IUserDynamicCellRepository userDynamicCellRepository, IDynamicPropertiesService dynamicPropertiesService, IUserExporter userExporter)
        {
            _currentUserService = currentUserService;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _dbContext = dbContext;            
            _mailHelper = mailHelper;
            _appSettings = appSettings;
            _userDynamicCellRepository = userDynamicCellRepository;
            _dynamicPropertiesService = dynamicPropertiesService;
            _userExporter = userExporter;

            SetFilters();
        }


        public async Task CreateUser(UserViewModel userVM)
        {
            userVM.Id = 0;
            var user = _mapper.Map<User>(userVM);
            user.UserName = Guid.NewGuid().ToString();
            var role = _roleManager.Roles
                .Include(x => x.Level)
                .FirstOrDefault(x => x.Id == userVM.RoleId);

            if (role == null)
                throw new DomainException("Role not found.");

            if (await PassesAllRules(user, role))
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    var password = PasswordGenerator(8);

                    var existings = 
                    _userManager.Users.Where(x => x.Email == userVM.Email && !x.IsDeleted).ToList();
                    if(existings.Count > 0)
                    {
                        throw new EmailAlreadyTakenException(userVM.Email);                        
                    }
                    var result = await _userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else
                    {
                        throw new DomainException(result.Errors.First().Description);
                    }

                    var mailBody = GetNewUserEmailBody(user.Email, password);
                    _mailHelper.SendEmail(user.Email, "New User", mailBody);

                    transaction.Commit();
                }
            }
        }
        public string PasswordGenerator(int length)
        {

            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!#$%&*";
            const string specialCharacter = "!#$%&*";
            const string upperCase= "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digit = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            res.Append(specialCharacter[rnd.Next(specialCharacter.Length)]);
            res.Append(upperCase[rnd.Next(upperCase.Length)]);
            res.Append(digit[rnd.Next(digit.Length)]);
            length = length - 3;

            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();

        }
        private string GetNewUserEmailBody(string email, string password)
        {
            var templateFileFullPath = Path.Combine(_env.GetRootPath(), FileNames.EMAIL_TEMPLATE_FOLDER,
                FileNames.NEW_USER_EMAIL_TEMPLATE);
            var emailBody = File.ReadAllText(templateFileFullPath);
            emailBody = string.Format(emailBody, email, password, _appSettings.HostUrl);
            return emailBody;
        }

        public async Task UpdateProfileInfo(ProfileInfoViewModel userVM)
        {
            var existing = _userManager.Users.SingleOrDefault(x => x.Email == userVM.Email && !x.IsDeleted);

            var user = await _userManager
                .Users
                .Include(x => x.UserRoles)
                .Include(x => x.UserEduSectorPartners)
                .SingleOrDefaultAsync(x => x.Id == userVM.Id && !x.IsDeleted);
            if (user == null)
            {
                throw new RecordNotFound($"User {userVM.FullName} not found.");
            }

            if (existing != null && existing.Id != user.Id)
            {
                throw new EmailAlreadyTakenException(userVM.Email);
            }

            user.FullName = userVM.FullName;
            user.PhoneNumber = userVM.PhoneNumber;

            await _userManager.UpdateAsync(user);
        }

        public async Task UpdateUser(UserViewModel userVM)
        {
            var existing = _userManager.Users.SingleOrDefault(x => x.Email == userVM.Email && !x.IsDeleted);

            var user = await _userManager
                .Users
                .Include(x => x.UserRoles)
                .Include(x=> x.UserEduSectorPartners)
                .SingleOrDefaultAsync(x => x.Id == userVM.Id && !x.IsDeleted);
            if (user == null)
            {
                throw new RecordNotFound($"User {userVM.FullName} not found.");
            }

            if(existing != null && existing.Id != user.Id)
            {
                throw new EmailAlreadyTakenException(userVM.Email);
            }

            user.FullName = userVM.FullName;
            user.DesignationName = userVM.DesignationName;
            user.UserRoles.Remove(user.UserRoles.First());
            user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = userVM.RoleId });
            user.PhoneNumber = userVM.PhoneNumber;
            user.Email = userVM.Email;

            
            var partners = user.UserEduSectorPartners.ToList();
            partners.ForEach(x => user.UserEduSectorPartners.Remove(x));
            userVM.EduSectorPartners.ForEach(x =>
            {
                user.UserEduSectorPartners.Add(new Core.Models.UserEduSectorPartner
                {
                    UserId = user.Id,
                    PartnerId = x.Id,
                    PartnerType = x.PartnerType
                });
            });

            await _userManager.UpdateAsync(user);
        }

        //Todo: Refactor to use specification pattern
        private async Task<bool> PassesAllRules(User user, Role role)
        {
            var partners = user.UserEduSectorPartners.ToList();

            if (_currentUserService.UserRank != LevelRank.Admin && _currentUserService.UserRank <= role.Level.Rank)
            {
                throw new AuthException("Can't create user");
            }

            if (role.Level.Rank == LevelRank.Admin)
            {
                user.UserEduSectorPartners.Clear();
                return await Task.FromResult(true);
            }

            if (_currentUserService.IsAdmin)
            {
                return await Task.FromResult(true);
            }

            if (role.Level.Rank == LevelRank.ProgramPartner)
            {
                var esp = partners.SingleOrDefault(x => x.PartnerType == PartnerType.EducationSectorPartner);
                if (esp == null)
                    throw new DomainException("Program Partner has to have an ESP.");
                return await Task.FromResult(true);
            }

            if (role.Level.Rank == LevelRank.ImplementationPartner)
            {
                var esp = partners.SingleOrDefault(x => x.PartnerType == PartnerType.EducationSectorPartner);
                if (esp == null)
                    throw new DomainException("Implementation Partner has to have an ESP");

                if (esp.PartnerId != _currentUserService.EducationSectorPartner)
                    throw new DomainException("Implementation Partner's ESP has to be logged in user's ESP.");

                return await Task.FromResult(true);
            }

            if (role.Level.Rank == LevelRank.Teacher)
            {
                if (partners.SingleOrDefault(x => x.PartnerType == PartnerType.ProgramPartner) == null)
                    throw new DomainException("Teacher has to have a Program Partner.");

                if (partners.SingleOrDefault(x => x.PartnerType == PartnerType.ImplementationPartner) == null)
                    throw new DomainException("Teacher has to have an Implementation Partner.");

                if (!partners.Select(x => x.PartnerId).Contains((int)_currentUserService.EducationSectorPartner))
                    throw new DomainException("Teacher's PP or IP has to be from logged in user's ESP.");

                return await Task.FromResult(true);
            }
            throw new DomainException("Can't create user.");
        }

        public async Task<PagedResponse<UserViewModel>> GetUsers(UserQueryModel model)
        {
            var esps = _currentUserService.Esps.Select(x => x.Id).ToList();


            Expression<Func<User, bool>> getAllUsersFilter = x => 
                  (_currentUserService.UserRank == LevelRank.Admin ||
                  x.UserRoles.First().Role.Level.Rank < _currentUserService.UserRank)
                  &&
                  ((_currentUserService.UserRank == LevelRank.Admin || esps.Count == 0) ||
                   x.UserEduSectorPartners.Select(partner => partner.PartnerId).Any(id => esps.Contains(id)))
                  && !x.IsDeleted
                  && (string.IsNullOrEmpty(model.SearchText) || 
                     (x.FullName.Contains(model.SearchText) || x.DesignationName.ToLower().Contains(model.SearchText.ToLower()))
                    )
                  && (model.RoleIds.Count == 0 || model.RoleIds.Contains(x.UserRoles.First().RoleId))
                  && (model.EspIds.Count == 0 || x.UserEduSectorPartners.Any(x => model.EspIds.Contains(x.PartnerId)))
                  && model.UserId == 0 || x.Id == model.UserId
                  ;

            return await GetFilteredUsersWithDynamicCells(getAllUsersFilter, model);
        }

        public async Task<PagedResponse<UserViewModel>> GetTeachers(BaseQueryModel model)
        {
            return await GetFilteredUsers(_getTeachersFilter, model);
        }
        
        private async Task<PagedResponse<UserViewModel>> GetFilteredUsersWithDynamicCells(Expression<Func<User, bool>> filter, BaseQueryModel model)
        {
            // TODO: combine filter and where condition to count

            var total = await Count(filter);

            var data = await
                _userManager.Users
                    .Where(filter)
                    .Where(x => string.IsNullOrEmpty(model.SearchText) || (x.FullName.Contains(model.SearchText) || x.DesignationName.Contains(model.SearchText)))
                    .Include(x => x.UserEduSectorPartners)
                    .ThenInclude(x => x.Partner)
                    .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                    .ThenInclude(x => x.Level)
                    .Include(x=> x.DynamicCells)
                    .ThenInclude(x=> x.EntityDynamicColumn)
                    .ThenInclude(x=> x.ColumnList)
                    .ThenInclude(x=> x.ListItems)
                    .Skip(model.Skip())
                    .Take(model.PageSize)
                    .ToListAsync();


            var projected = 
            data.Select(x => new UserViewModel
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    DesignationName = x.DesignationName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    LevelId = x.UserRoles.Select(userRole => userRole.Role).First().Level.Id,
                    LevelName = x.UserRoles.Select(userRole => userRole.Role).First().Level.LevelName,
                    RoleId = x.UserRoles.Single().Role.Id,
                    RoleName = x.UserRoles.Single().Role.Name,
                    EduSectorPartners = x.UserEduSectorPartners.Select(p => new EduSectorPartnerViewModel
                    {
                        Id = p.Partner.Id,
                        PartnerName = p.Partner.PartnerName,
                        PartnerType = p.PartnerType
                    }).ToList(),
                    DynamicCells = x.DynamicCells.Select(col => new UserDynamicCellViewModel
                    {
                        Id = col.Id,
                        ColumnName = col.EntityDynamicColumn.Name,
                        EntityDynamicColumnId = col.EntityDynamicColumnId,
                        UserId = col.UserId,
                        Value = col.Value,
                        DataType = col.EntityDynamicColumn.ColumnType,
                        ListType = new ListDataTypeViewModel()
                        {
                            Name = col.EntityDynamicColumn.ColumnList?.Name,
                            ListItems = col.EntityDynamicColumn
                                .ColumnList?
                                .ListItems
                                .Select(item => new ListItemViewModel()
                                {
                                    Title = item.Title,
                                    Value = item.Value
                                }).ToList()
                        }
                    }).ToList()
                })
                .ToList();
            
            var grouped = projected.Select(UserViewModel.GroupByEntityDynamicColumn);

            return new PagedResponse<UserViewModel>(grouped, total, model.PageNo, model.PageSize);
        }


        private async Task<PagedResponse<UserViewModel>> GetFilteredUsers(Expression<Func<User, bool>> filter, BaseQueryModel model)
        {
            var data = await
            _userManager.Users
            .Where(filter)
            .Where(x=> string.IsNullOrEmpty(model.SearchText) || (x.FullName.Contains(model.SearchText) || x.DesignationName.Contains(model.SearchText)))
            .Include(x => x.UserEduSectorPartners)
            .ThenInclude(x => x.Partner)
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .ThenInclude(x => x.Level)
            
            .Skip(model.Skip())
            .Take(model.PageSize)
            .ToListAsync();

            var total = await Count(filter);


            var mappedData = _mapper.Map<IEnumerable<UserViewModel>>(data)
            .OrderBy(x => x.Id);

            return new PagedResponse<UserViewModel>(mappedData, total, model.PageNo, model.PageSize);
        }

        public async Task<UserViewModel> GetUserById(int userId)
        {
            var user = await _userManager
                .Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.Level)                
                .Include(x => x.UserEduSectorPartners)
                .ThenInclude(x => x.Partner)
                .Where(x => !x.IsDeleted && x.Id == userId)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    FullName = x.FullName,                    
                    DesignationName = x.DesignationName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    RoleId = x.UserRoles.FirstOrDefault().RoleId,
                    RoleName = x.UserRoles.FirstOrDefault().Role.Name,
                    EduSectorPartners = x.UserEduSectorPartners.Select(p => new EduSectorPartnerViewModel
                    {
                        Id = p.Partner.Id,
                        PartnerName = p.Partner.PartnerName,
                        PartnerType = p.PartnerType
                    }).ToList()
                }).SingleOrDefaultAsync();

            if (user == null)
            {
                throw new RecordNotFound($"User not found.");
            }

            return user;
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);
            if (user == null)
                throw new RecordNotFound($"User not found.");
            user.IsDeleted = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new DomainException($"User {user.FullName} can't be deleted.");
            }
        }
        public async Task DeleteUser(List<int> userIds)
        {
            var users = await _userManager.Users.Where(x => userIds.Contains(x.Id) && !x.IsDeleted).ToListAsync();
            if (users.Count == 0)
                throw new RecordNotFound($"User not found.");
            users.ForEach(user => { user.IsDeleted = true; });
            
            _dbContext.Users.UpdateRange(users);

            await _dbContext.SaveChangesAsync();
        }

        public async Task ResetPassword(PasswordResetViewModel model)
        {
            var user = await _userManager.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.Level)
                .SingleOrDefaultAsync(x => x.Id == model.UserId && !x.IsDeleted)
                ;
            if (user == null)
            {
                throw new RecordNotFound($"User not found.");
            }

            if (user.UserRoles.Single().Role.Level.Rank >= _currentUserService.UserRank && _currentUserService.UserRank != LevelRank.Admin)
            {
                throw new AuthException("Failed to reset password.");
            }

            var result = await _userManager.RemovePasswordAsync(user);
            if (result.Succeeded)
            {
                result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!result.Succeeded)
                {
                    throw new DomainException(result.Errors.First().Description);
                }
            }
            else
            {
                throw new DomainException(result.Errors.First().Description);
            }
        }

        public async Task ChangePassword(PasswordChangeViewModel model)
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.Id == _currentUserService.UserId);
            if (user == null)
            {
                throw new RecordNotFound($"User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                throw new DomainException("Failed to change password.");
            }
        }

        public async Task InsertDynamicCell(UserDynamicCellInsertViewModel model)
        {
            await UpsertDynamicCell(model);
        }

        private async Task UpsertDynamicCell(UserDynamicCellInsertViewModel model)
        {
            var ids = model.DynamicCells.Select(x => x.EntityDynamicColumnId)
                .ToList();

            var existing = await _userDynamicCellRepository.All(
                x => x.UserId == model.UserId &&
                     ids.Contains(x.EntityDynamicColumnId)
            );

            await _userDynamicCellRepository.DeleteRange(existing);

            var cells = model.DynamicCells.SelectMany(cell => cell.Value.Select(value =>
                new UserDynamicCell()
                {
                    EntityDynamicColumnId = cell.EntityDynamicColumnId,
                    UserId = model.UserId,
                    Value = value,
                })).ToList();
            await _userDynamicCellRepository.InsertRange(cells);
        }

        public async Task UpdateDynamicCell(UserDynamicCellInsertViewModel model)
        {
            await UpsertDynamicCell(model);
        }

        public async Task DeleteDynamicCell(UserDynamicCellViewModel model)
        {
            var list = await _userDynamicCellRepository.All(x =>
                x.UserId == model.UserId &&
                x.EntityDynamicColumnId == model.EntityDynamicColumnId);

            await _userDynamicCellRepository.DeleteRange(list);
        }

        public async Task<byte[]> ExportFilteredUsers(UserQueryModel model)
        {
            model.PageNo = 1;
            model.PageSize = SystemDefaults.MaxPageSize;

            var users = (await GetUsers(model)).Data;

            var dynamicColumns = (await _dynamicPropertiesService.GetPaginatedColumn(
                new DynamicPropertiesBySearchParamQueryModel()
                {
                    PageNo = 1,
                    PageSize = SystemDefaults.MaxPageSize,
                    EntityTypeId = EntityType.User
                })).Data;


            return await _userExporter.ExportUsers(users.ToList(), dynamicColumns.ToList());
        }


        private void SetFilters()
        {
            var esps = _currentUserService.Esps.Select(x => x.Id).ToList();

            _getTeachersFilter =
                   x => (_currentUserService.UserRank == LevelRank.Admin ||
                   x.UserRoles.First().Role.Level.Rank < _currentUserService.UserRank) &&
                        ((_currentUserService.UserRank == LevelRank.Admin || esps.Count == 0) ||
                         x.UserEduSectorPartners.Select(partner => partner.PartnerId).Any(id => esps.Contains(id))) &&
                        x.UserRoles.First().Role.Level.Rank == LevelRank.Teacher 
                   && !x.IsDeleted;
        }

        private async Task<int> Count(Expression<Func<User, bool>> expression)
        {
            return await _userManager.Users.Where(expression).CountAsync();
        }

        public async Task<int> Count(Specification<User> specification)
        {
            return await _userManager.Users.Where(specification.ToExpression()).CountAsync();
        }
    }
}
