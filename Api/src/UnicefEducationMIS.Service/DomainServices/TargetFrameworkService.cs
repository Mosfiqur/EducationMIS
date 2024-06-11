using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.Repositories.Framework;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class TargetFrameworkService : ITargetFrameworkService
    {
        private readonly ITargetFrameworkRepository _targetFrameworkRepository;
        private readonly IMapper _mapper;
        private readonly ITargetDynamicCellRepository _targetDynamicCellRepository;
        public TargetFrameworkService(
            ITargetFrameworkRepository targetFrameworkRepository,
            IMapper mapper,
            ITargetDynamicCellRepository targetDynamicCellRepository)
        {
            _targetFrameworkRepository = targetFrameworkRepository;
            _mapper = mapper;
            _targetDynamicCellRepository = targetDynamicCellRepository;
        }

        public async Task AddTargetFramework(TargetFrameworkCreateViewModel model)
        {
            var targetFramework = _mapper.Map<TargetFramework>(model);

            await _targetFrameworkRepository.Insert(targetFramework);
        }

        public async Task UpdateTargetFramework(TargetFrameworkUpdateViewModel model)
        {
            var existing = await _targetFrameworkRepository.GetById(model.Id);
            if (existing == null)
            {
                throw new RecordNotFound();
            }

            existing = _mapper.Map<TargetFramework>(model);

            await _targetFrameworkRepository.Update(existing);
        }

        public async Task DeleteTargetFramework(long targetFrameworkId)
        {
            var targetFramework = await _targetFrameworkRepository.GetById(targetFrameworkId);
            if (targetFramework == null)
                throw new RecordNotFound("Target Framework Not Found");

            await _targetFrameworkRepository.Delete(targetFramework);
        }
        public async Task DeleteMultipleTargetFramework(List<long> targetFrameworkIds)
        {
            var targetFrameworks = await _targetFrameworkRepository.GetAll().Where(a=>targetFrameworkIds.Contains(a.Id)).ToListAsync();
            if (targetFrameworks.Count ==0)
                throw new RecordNotFound("Target Framework Not Found");

            await _targetFrameworkRepository.DeleteRange(targetFrameworks);
        }
        public async Task<PagedResponse<TargetFrameworkViewModel>> GetTargetFrameworks(BaseQueryModel model)
        {

            var total = _targetFrameworkRepository.GetAll().Count();
            var data = await _targetFrameworkRepository.GetAll()
                .Include(x => x.DynamicCells)                
                .ThenInclude(x => x.EntityDynamicColumn)
                .Include(x => x.Camp)
                .Skip(model.Skip()).Take(model.PageSize)
                .Select(TargetFrameworkViewModel.FromModel)
                .ToListAsync();

            var grouped = data.Select(TargetFrameworkViewModel.GroupByEntityDynamicColumn);
            return new PagedResponse<TargetFrameworkViewModel>(grouped, total, model.PageNo, model.PageSize);
        }

        public async Task<TargetFrameworkViewModel> GetById(long id)
        {
            return _mapper.Map<TargetFrameworkViewModel>(await _targetFrameworkRepository.GetById(id));
        }

        public async Task InsertDynamicCell(TargetDynamicCellInsertViewModel model)
        {
            await UpsertDynamicCell(model);
        }

        public async Task UpdateDynamicCell(TargetDynamicCellInsertViewModel model)
        {
            await UpsertDynamicCell(model);
        }

        private async Task UpsertDynamicCell(TargetDynamicCellInsertViewModel model)
        {
            var ids = model.DynamicCells.Select(x => x.EntityDynamicColumnId)
                .ToList();

            var existing = await _targetDynamicCellRepository.All(
                x => x.TargetFrameworkId == model.TargetFrameworkId &&
                     ids.Contains(x.EntityDynamicColumnId)
            );

            await _targetDynamicCellRepository.DeleteRange(existing);

            var cells = model.DynamicCells.SelectMany(cell => cell.Value.Select(value =>
                new TargetFrameworkDynamicCell()
                {
                    EntityDynamicColumnId = cell.EntityDynamicColumnId,
                    TargetFrameworkId = model.TargetFrameworkId,
                    Value = value,
                })).ToList();
            await _targetDynamicCellRepository.InsertRange(cells);
        }

        public async Task DeleteDynamicCell(TargetDynamicCellViewModel model)
        {

            var list = await _targetDynamicCellRepository.All(x =>
                x.TargetFrameworkId == model.TargetFrameworkId &&
                x.EntityDynamicColumnId == model.EntityDynamicColumnId);

            await _targetDynamicCellRepository.DeleteRange(list);
        }
    }
}
