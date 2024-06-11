using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
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
    public class BudgetFrameworkService : IBudgetFrameworkService
    {
        private readonly IBudgetFrameworkRepository _budgetFrameworkRepository;
        private readonly IMapper _mapper;
        private readonly IBudgetDynamicCellRepository _budgetDynamicCellRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IDonorRepository _donorRepository;
        public BudgetFrameworkService(
            IBudgetFrameworkRepository budgetFrameworkRepository,
            IMapper mapper,
            IBudgetDynamicCellRepository budgetDynamicCellRepository,
            IProjectRepository projectRepository, 
            IDonorRepository donorRepository)
        {
            _budgetFrameworkRepository = budgetFrameworkRepository;
            _mapper = mapper;
            _budgetDynamicCellRepository = budgetDynamicCellRepository;
            _projectRepository = projectRepository;
            _donorRepository = donorRepository;
        }

        public async Task Add(BudgetFrameworkCreateViewModel model)
        {
            var budgetFramework = _mapper.Map<BudgetFramework>(model);
            await _budgetFrameworkRepository.Insert(budgetFramework);
        }

        public async Task Update(BudgetFrameworkUpdateViewModel model)
        {
            await _budgetFrameworkRepository.ThrowIfNotFound(model.Id);
            var entity = _mapper.Map<BudgetFramework>(model);
            await _budgetFrameworkRepository.Update(entity);
        }
        

        public async Task Delete(long budgetId)
        {
            var budgetFramework = await _budgetFrameworkRepository.ThrowIfNotFound(budgetId);
            await _budgetFrameworkRepository.Delete(budgetFramework);
        }
        public async Task DeleteMultiple(List<long> budgetIds)
        {
            var budgetFrameworks = await _budgetFrameworkRepository.GetAll().Where(a => budgetIds.Contains(a.Id)).ToListAsync();
            await _budgetFrameworkRepository.DeleteRange(budgetFrameworks);
        }
        public async Task<PagedResponse<BudgetFrameworkViewModel>> GetAllBudgets(BaseQueryModel model)
        {
            var total = _budgetFrameworkRepository.GetAll().Count();
            var data = await _budgetFrameworkRepository.GetAll()
                .Include(x => x.Donor)
                .Include(x => x.Project)
                .Include(x => x.DynamicCells)
                .ThenInclude(x => x.EntityDynamicColumn)
                .Skip(model.Skip()).Take(model.PageSize)
                .Select(BudgetFrameworkViewModel.FromModel)
                .ToListAsync();

            var grouped = data.Select(BudgetFrameworkViewModel.GroupeByEntityDynamicColumn);
            
            return new PagedResponse<BudgetFrameworkViewModel>(grouped, total, model.PageNo, model.PageSize);
        }

        public async Task InsertDynamicCell(BudgetDynamicCellInsertViewModel model)
        {
            var ids = model.DynamicCells.Select(x => x.EntityDynamicColumnId)
                .ToList();

            var existing = await _budgetDynamicCellRepository.All(
                x => x.BudgetFrameworkId == model.BudgetFrameworkId && 
                     ids.Contains(x.EntityDynamicColumnId)
                     );

            await _budgetDynamicCellRepository.DeleteRange(existing);

            var cells = model.DynamicCells.SelectMany(cell => cell.Value.Select(value =>
                new BudgetFrameworkDynamicCell()
                {
                    EntityDynamicColumnId = cell.EntityDynamicColumnId,
                    BudgetFrameworkId = model.BudgetFrameworkId,
                    Value = value,
                })).ToList();
            await _budgetDynamicCellRepository.InsertRange(cells);
        }

        public async Task DeleteDynamicCell(BudgetDynamicCellViewModel model)
        {
            var list = await _budgetDynamicCellRepository.All(x =>
                x.BudgetFrameworkId == model.BudgetFrameworkId &&
                x.EntityDynamicColumnId == model.EntityDynamicColumnId);
            await _budgetDynamicCellRepository.DeleteRange(list);
        }

        public async Task<ProjectViewModel> AddProject(ProjectViewModel model)
        {
            var project = _mapper.Map<Project>(model);
            await _projectRepository.Insert(project);
            return _mapper.Map<ProjectViewModel>(project);
        }

        public async Task<DonorViewModel> AddDonor(DonorViewModel model)
        {
            var donor = _mapper.Map<Donor>(model);
            await _donorRepository.Insert(donor);
            return _mapper.Map<DonorViewModel>(donor);
        }

        public async  Task<PagedResponse<ProjectViewModel>> GetAllProjects(BaseQueryModel model)
        {
            Expression<Func<Project, bool>> filter = x => x.Name.Contains(model.SearchText);
            var total = await _projectRepository.Count(filter);
            var list = await _projectRepository.All(filter);
            var mapped = _mapper.Map<List<ProjectViewModel>>(list);
            return new PagedResponse<ProjectViewModel>(mapped, total, model.PageNo, model.PageSize);
        }

        public async Task<PagedResponse<DonorViewModel>> GetAllDonors(BaseQueryModel model)
        {
            Expression<Func<Donor, bool>> filter = x => x.Name.Contains(model.SearchText);
            var total = await _donorRepository.Count(filter);
            var list = await _donorRepository.All(filter);
            var mapped = _mapper.Map<List<DonorViewModel>>(list);
            return new PagedResponse<DonorViewModel>(mapped, total, model.PageNo, model.PageSize);
        }

        public async Task<BudgetFrameworkViewModel> GetById(long id)
        {
            var entity = await _budgetFrameworkRepository
                .GetAll()
                .Include(x => x.Project)
                .Include(x => x.Donor)
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<BudgetFrameworkViewModel>(entity);
        }
    }
}
