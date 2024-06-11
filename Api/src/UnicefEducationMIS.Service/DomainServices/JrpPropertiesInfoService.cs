using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Data;
using UnicefEducationMIS.Data.Repositories;

namespace UnicefEducationMIS.Service.DomainServices
{
    public class JrpPropertiesInfoService : IJrpPropertiesInfoService
    {
        private readonly IJrpPropertiesInfoRepository _jrpPropertiesInfoRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IScheduleInstanceRepository _scheduleInstanceRepository;

        public IMapper _mapper { get; }

        public JrpPropertiesInfoService(IJrpPropertiesInfoRepository jrpPropertiesInfoRepository, IMapper mapper
            , IServiceProvider serviceProvider, IScheduleInstanceRepository scheduleInstanceRepository)
        {
            _jrpPropertiesInfoRepository = jrpPropertiesInfoRepository;
            _serviceProvider = serviceProvider;
            _scheduleInstanceRepository = scheduleInstanceRepository;
            _mapper = mapper;
        }

        public async Task Add(JrpParameterInfoViewModel jrpParameterInfo)
        {
            var jrp = _mapper.Map<JrpParameterInfo>(jrpParameterInfo);
            await _jrpPropertiesInfoRepository.Insert(jrp);
        }
        public async Task Update(JrpParameterInfoViewModel jrpParameterInfo)
        {
            var jrp = await _jrpPropertiesInfoRepository.GetById((int)jrpParameterInfo.Id);

            jrp.Name = jrpParameterInfo.Name;
            jrp.IndicatorId = jrpParameterInfo.IndicatorId;
            jrp.TargetId = jrpParameterInfo.TargetId;
            await _jrpPropertiesInfoRepository.Update(jrp);
        }
        public async Task Delete(int entityId)
        {
            await _jrpPropertiesInfoRepository.Delete(entityId);
        }

        public async Task<JrpChartViewModel> GetJrpChartData(JrpPropertiesWithFilterViewModel jrpReportFilter)
        {
            JrpChartViewModel jrpChartViewModel = new JrpChartViewModel();
            var jrpProperties = await _jrpPropertiesInfoRepository.GetAll().ToListAsync();
            var taskList = new List<Task<JrpChartData>>();
            long beneficiaryInstanceId = 0;
            long facilityInstanceId = 0;
            if (jrpReportFilter.BeneficiaryInstanceId == null)
            {
                beneficiaryInstanceId = await _scheduleInstanceRepository.GetAll()
                    .Where(a => a.Status == Core.ValueObjects.InstanceStatus.Completed && a.Schedule.ScheduleFor == EntityType.Beneficiary)
                    .OrderByDescending(a => a.DataCollectionDate).Select(a => a.Id).FirstOrDefaultAsync();
            }
            else
            {
                beneficiaryInstanceId = (long)jrpReportFilter.BeneficiaryInstanceId;
            }

            if (jrpReportFilter.FacilityInstanceId == null)
            {
                facilityInstanceId = await _scheduleInstanceRepository.GetAll()
                    .Where(a => a.Status == Core.ValueObjects.InstanceStatus.Completed
                    && a.Schedule.ScheduleFor == EntityType.Facility)
                    .OrderByDescending(a => a.DataCollectionDate).Select(a => a.Id).FirstOrDefaultAsync();
            }
            else
            {
                facilityInstanceId = (long)jrpReportFilter.FacilityInstanceId;
            }

            foreach (var jrp in jrpProperties)
            {
                JrpPropertiesWithFilterViewModel jrpPropertiesWithFilter = new JrpPropertiesWithFilterViewModel();
                jrpPropertiesWithFilter = _mapper.Map<JrpPropertiesWithFilterViewModel, JrpPropertiesWithFilterViewModel>(jrpReportFilter, jrpPropertiesWithFilter);

                var repo = (IJrpPropertiesInfoRepository)_serviceProvider.GetService(
                    typeof(IJrpPropertiesInfoRepository));
                jrpChartViewModel.Categories.Add(jrp.Name);

                var jrpViewModel = _mapper.Map<JrpParameterInfoViewModel>(jrp);

                jrpPropertiesWithFilter.JrpParameterInfo = jrpViewModel;

                var task = new Task<JrpChartData>(() => repo.GetJrpChartData(jrpPropertiesWithFilter, beneficiaryInstanceId, facilityInstanceId));
                task.Start();
                taskList.Add(task);
            }


            if (taskList.Count > 0)
            {
                Task.WaitAll(taskList.ToArray());
                List<JrpChartData> datas = taskList.Select(t => t.Result).ToList();
                foreach (var data in datas)
                {
                    jrpChartViewModel.Achievement.Add(data.Achievement);
                    jrpChartViewModel.Target.Add(data.Target);
                }
            }

            return jrpChartViewModel;
        }

        public async Task<List<JrpParameterInfoViewModel>> Get(BaseQueryModel baseQueryModel)
        {
            var rawData = _jrpPropertiesInfoRepository.GetAll().Include(a => a.EntityDynamicColumn).Include(a => a.ObjectiveIndicator)
                .Select(a => new JrpParameterInfoViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    IndicatorId = a.IndicatorId,
                    IndicatorName = a.EntityDynamicColumn.Name,
                    TargetId=a.TargetId,
                    TargetName=a.ObjectiveIndicator.Indicator
                });
            if (!string.IsNullOrEmpty(baseQueryModel.SearchText))
                rawData = rawData.Where(a => a.Name.Contains(baseQueryModel.SearchText));
            var data = await rawData.Skip(baseQueryModel.Skip()).Take(baseQueryModel.PageSize).ToListAsync();
            return _mapper.Map<List<JrpParameterInfoViewModel>>(data);
        }
    }
}
