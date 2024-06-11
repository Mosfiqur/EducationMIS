using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class RoleService : IRoleService
    {
        private readonly ICurrentLoginUserService _currentUserService;
        private readonly IUserLevelRepository _userLevelRepository;
        private readonly RoleManager<Role> _roleManager;
        
        private readonly IMapper _mapper;


        public RoleService(ICurrentLoginUserService currentUserService, 
            RoleManager<Role> roleManager, 
            IMapper mapper,
            IUserLevelRepository userLevelRepository
            )
        {
            _currentUserService = currentUserService;
            _roleManager = roleManager;
            _mapper = mapper;
            _userLevelRepository = userLevelRepository;
        }

        public async Task CreateRole(RoleViewModel roleVM)
        {
            roleVM.Id = 0;
            var role = _mapper.Map<Role>(roleVM);
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new DomainException(result.Errors.First().Description);
            }
        }

        public async Task UpdateRole(RoleViewModel roleVM)
        {
            var role = _mapper.Map<Role>(roleVM);

            var userRole = _roleManager.Roles
              .Where(x => x.Id == roleVM.Id)
              .Include(x => x.RolePermissions)
              .Include(x => x.Level)
              .SingleOrDefault();

            if (userRole == null)
                throw new DomainException("Role not found.");
            if (userRole.LevelId != role.LevelId)
                throw new DomainException("Level can't be changed.");

            userRole.RolePermissions
                .ToList()
                .ForEach(rp =>
                    userRole.RolePermissions.Remove(rp)
                  );
            role.RolePermissions
                .ToList()
                .ForEach(rp =>
                    userRole.RolePermissions.Add(rp)
                    );
            userRole.Name = role.Name;
            await _roleManager.UpdateAsync(userRole);
        }
               
        public async Task<PagedResponse<RoleViewModel>> GetRoles(BaseQueryModel pagingQuery)
        {
            var total = _roleManager.Roles.Count();
            var data = await _roleManager.Roles
                .Where(x => _currentUserService.IsAdmin || _currentUserService.UserRank > x.Level.Rank)
                .Skip(pagingQuery.Skip())
                .Take(pagingQuery.PageSize)
                .Include(x => x.Level)
                .OrderBy(x => x.Id)
                .ToListAsync();

            var list = _mapper.Map<IEnumerable<RoleViewModel>>(data);
            return new PagedResponse<RoleViewModel>(list, total, pagingQuery.PageNo, pagingQuery.PageSize);
        }

        public async Task<RoleViewModel> GetRoleById(int roleId)
        {
            var data = await _roleManager.Roles
                .Include(x => x.Level)
                .Include(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)           
                .Include(x => x.PermissionPreset)
                .SingleOrDefaultAsync(x => x.Id == roleId);
            if(data == null)
            {
                throw new RecordNotFound("Role not found");
            }
            return _mapper.Map<RoleViewModel>(data);
        }

        public async Task DeleteRole(int roleId)
        {
            var role = await _roleManager.Roles.SingleOrDefaultAsync(role => role.Id == roleId);
            if(role == null)
            {
                throw new RecordNotFound($"Role with id {roleId} not found.");
            }
            await _roleManager.DeleteAsync(role);
        }

        public async Task<IEnumerable<UserLevelViewModel>> GetLevels()
        {
            var data = await _userLevelRepository.GetAll()
                .Where(x => _currentUserService.IsAdmin || x.Rank < _currentUserService.UserRank)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserLevelViewModel>>(data);
        }
    }
}
