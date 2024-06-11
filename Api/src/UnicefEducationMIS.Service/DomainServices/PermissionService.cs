using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class PermissionService : IPermissionService
    {
        private readonly IMapper _mapper;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionPresetRepository _permissionPresetRepository;

        public PermissionService(IMapper mapper, 
            IPermissionRepository permissionRepository, 
            IPermissionPresetRepository permissionPresetRepository)
        {
            _mapper = mapper;
            _permissionRepository = permissionRepository;
            _permissionPresetRepository = permissionPresetRepository;
        }

        public async Task<PagedResponse<PermissionPresetViewModel>> GetPermissionPresets(BaseQueryModel model)
        {
            var total = _permissionPresetRepository.GetAll()
                .Count();

            var presets = _permissionPresetRepository.GetAll()
                .Skip(model.Skip())
                .Take(model.PageSize)
                .ToList();

            var data = _mapper.Map<IEnumerable<PermissionPresetViewModel>>(presets);
            return await Task.FromResult(new PagedResponse<PermissionPresetViewModel>(data, total, model.PageNo, model.PageSize));
        }

        public async Task<PagedResponse<PermissionViewModel>> GetPermissions(BaseQueryModel model)
        {
            var total = _permissionRepository
                .GetAll()
                .Count();

            var permissions = _permissionRepository
                .GetAll()
                .Skip(model.Skip())
                .Take(model.PageSize);

            var data = _mapper.Map<IEnumerable<PermissionViewModel>>(permissions);
            
            return await Task.FromResult(new PagedResponse<PermissionViewModel>(data.OrderBy(x => x.PermissionName), total, model.PageNo, model.PageSize));
        }

        public Task<PagedResponse<PermissionViewModel>> GetPermissionsByPresetId(PermissionQueryModel model)
        {
            throw new NotImplementedException();
        }
    }
}
