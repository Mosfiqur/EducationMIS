using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.Repositories.Framework;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class MonitoringFrameworkService : IMonitoringFrameworkService
    {
        private readonly IMonitoringFrameworkRepository _monitoringFrameworkRepository;
        private readonly IMonitoringDynamicCellRepository _monitoringDynamicCellRepository;
        private readonly IObjectiveIndicatorRepository _indicatorRepository;
        private readonly IMapper _mapper;
        public MonitoringFrameworkService(
                IMonitoringFrameworkRepository monitoringFrameworkRepository,
                IMapper mapper,
                IMonitoringDynamicCellRepository monitoringDynamicCellRepository,
                IObjectiveIndicatorRepository indicatorRepository)
        {
            _monitoringFrameworkRepository = monitoringFrameworkRepository;
            _mapper = mapper;
            _monitoringDynamicCellRepository = monitoringDynamicCellRepository;
            _indicatorRepository = indicatorRepository;
        }
        public async Task<MonitoringFrameworkUpdateViewModel> Add(MonitoringFrameworkCreateViewModel model)
        {
            var monitoringFramework = _mapper.Map<MonitoringFramework>(model);

            await _monitoringFrameworkRepository.Insert(monitoringFramework);

            return _mapper.Map<MonitoringFrameworkUpdateViewModel>(monitoringFramework);
        }

        public async Task Update(MonitoringFrameworkUpdateViewModel model)
        {
            var existing = await _monitoringFrameworkRepository.GetById(model.Id);
            if(existing == null)
            {
                throw new RecordNotFound($"Monitoring framework not found with id {model.Id}");
            }

            existing.Objective = model.Objective;
            await _monitoringFrameworkRepository.Update(existing);
        }

        public async Task Delete(long id)
        {
            var monitoringFramework = _monitoringFrameworkRepository.GetAll()
                .Include(x => x.ObjectiveIndicators)
                .FirstOrDefault(x => x.Id == id);
            if (monitoringFramework == null)
                throw new RecordNotFound("Not Found");

            await _monitoringFrameworkRepository.Delete(monitoringFramework);
        }

        public async Task<PagedResponse<MonitoringFrameworkViewModel>> GetAll(BaseQueryModel model)
        {            
            var total = _monitoringFrameworkRepository.GetAll().Count();
            var data = await _monitoringFrameworkRepository.GetAll()
                .Include(x => x.ObjectiveIndicators)
                .ThenInclude(x => x.DynamicCells)
                .ThenInclude(x => x.EntityDynamicColumn)
                .Include(x => x.ObjectiveIndicators)
                .ThenInclude( x=> x.ReportingFrequency)
                

                .Skip(model.Skip()).Take(model.PageSize)
                .Select(x => new MonitoringFrameworkViewModel
                {
                    Id = x.Id,
                    Objective = x.Objective,
                    ObjectiveIndicators =  x.ObjectiveIndicators.Select(indicator => new ObjectiveIndicatorViewModel()
                        {
                            Id = indicator.Id,
                            Indicator = indicator.Indicator,
                            MonitoringFrameworkId = indicator.MonitoringFrameworkId,
                            BaseLine = indicator.BaseLine,
                            Target = indicator.Target,
                            StartDate = indicator.StartDate,
                            EndDate = indicator.EndDate,
                            Unit = indicator.Unit,
                            OrganizationId = indicator.OrganizationId,
                            OrganizationName = indicator.Organization.PartnerName,
                            ReportingFrequencyId = indicator.ReportingFrequencyId,
                            FrequencyName = indicator.ReportingFrequency.Name,
                            DynamicCells = indicator.DynamicCells.Select(col => new ObjectiveIndicatorDynamicCellViewModel
                                {
                                    Id = col.Id,
                                    ColumnName = col.EntityDynamicColumn.Name,
                                    EntityDynamicColumnId = col.EntityDynamicColumnId,
                                    ObjectiveIndicatorId = col.ObjectiveIndicatorId,
                                    Value = col.Value
                                }).ToList()
                    }).ToList()
                })
                .ToListAsync();

            var grouped = data
                .Select(x => new MonitoringFrameworkViewModel
                {
                    Id = x.Id,
                    Objective = x.Objective,
                    ObjectiveIndicators = x.ObjectiveIndicators.Select(indicator => new ObjectiveIndicatorViewModel()
                    {
                        Id = indicator.Id,
                        Indicator = indicator.Indicator,
                        MonitoringFrameworkId = indicator.MonitoringFrameworkId,
                        BaseLine = indicator.BaseLine,
                        Target = indicator.Target,
                        StartDate = indicator.StartDate,
                        EndDate = indicator.EndDate,
                        Unit = indicator.Unit,
                        OrganizationId = indicator.OrganizationId,
                        OrganizationName = indicator.OrganizationName,
                        ReportingFrequencyId = indicator.ReportingFrequencyId,
                        FrequencyName = indicator.FrequencyName,
                        DynamicCells = indicator.DynamicCells.GroupBy(cell => new
                            {
                                cell.EntityDynamicColumnId,
                                cell.ObjectiveIndicatorId,
                                cell.ColumnName,
                                cell.DataType
                            })
                            .ToList()
                            .Select(col => new ObjectiveIndicatorDynamicCellViewModel()
                            {
                                ColumnName = col.Key.ColumnName,
                                EntityDynamicColumnId = col.Key.EntityDynamicColumnId,
                                ObjectiveIndicatorId = col.Key.ObjectiveIndicatorId,
                                DataType = col.Key.DataType,
                                Values = col.Select(x => x.Value).ToList()
                            }).ToList()

                    }).ToList()
                }).ToList();

            return new PagedResponse<MonitoringFrameworkViewModel>(grouped, total, model.PageNo, model.PageSize);
        }

        public async Task InsertDynamicCell(ObjectiveIndicatorDynamicCellInsertViewModel model)
        {
            await DoInsertDynamicCell(model);
        }
        public async Task UpdateDynamicCell(ObjectiveIndicatorDynamicCellInsertViewModel model)
        {
            await DoInsertDynamicCell(model);
        }


        private async Task DoInsertDynamicCell(ObjectiveIndicatorDynamicCellInsertViewModel model)
        {
            var ids = model.DynamicCells.Select(x => x.EntityDynamicColumnId)
                .ToList();

            var existing = await _monitoringDynamicCellRepository.All(
                x => x.ObjectiveIndicatorId == model.ObjectiveIndicatorId &&
                     ids.Contains(x.EntityDynamicColumnId)
            );

            await _monitoringDynamicCellRepository.DeleteRange(existing);

            var cells = model.DynamicCells.SelectMany(cell => cell.Value.Select(value =>
                new MonitoringFrameworkDynamicCell()
                {
                    EntityDynamicColumnId = cell.EntityDynamicColumnId,
                    ObjectiveIndicatorId = model.ObjectiveIndicatorId,
                    Value = value,
                })).ToList();
            await _monitoringDynamicCellRepository.InsertRange(cells);
        }


        public async Task DeleteDynamicCell(ObjectiveIndicatorDynamicCellViewModel model)
        {
            var list = await _monitoringDynamicCellRepository.All(x =>
                x.ObjectiveIndicatorId == model.ObjectiveIndicatorId &&
                x.EntityDynamicColumnId == model.EntityDynamicColumnId);
            await _monitoringDynamicCellRepository.DeleteRange(list);
        }

        public async Task<PagedResponse<ObjectiveIndicatorViewModel>> GetObjectiveIndicator(BaseQueryModel baseQueryModel)
        {
            var total =await _indicatorRepository.GetAll().CountAsync();
            var data = await _indicatorRepository.GetAll()
                        .Skip(baseQueryModel.Skip()).Take(baseQueryModel.PageSize)
                        .Where(a => a.Indicator.Contains(baseQueryModel.SearchText??""))
                        .Select(indicator => new ObjectiveIndicatorViewModel {
                            Id = indicator.Id,
                            Indicator = indicator.Indicator,
                            MonitoringFrameworkId = indicator.MonitoringFrameworkId,
                            BaseLine = indicator.BaseLine,
                            Target = indicator.Target,
                            StartDate = indicator.StartDate,
                            EndDate = indicator.EndDate,
                            Unit = indicator.Unit,
                            OrganizationId = indicator.OrganizationId,
                            OrganizationName = indicator.Organization.PartnerName,
                            ReportingFrequencyId = indicator.ReportingFrequencyId,
                            FrequencyName = indicator.ReportingFrequency.Name
                        }).ToListAsync();
            return new PagedResponse<ObjectiveIndicatorViewModel>(data, total, baseQueryModel.PageNo, baseQueryModel.PageSize);
        }

    }
}
