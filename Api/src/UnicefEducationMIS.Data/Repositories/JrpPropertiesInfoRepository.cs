using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.Repositories
{
    public class JrpPropertiesInfoRepository : BaseRepository<JrpParameterInfo, int>, IJrpPropertiesInfoRepository
    {
        //private UnicefEduDbContext _ucontext;
        private readonly IServiceProvider _serviceProvider;

        public JrpPropertiesInfoRepository(UnicefEduDbContext context, IServiceProvider serviceProvider) : base(context)
        {
            //_ucontext = context;
            _serviceProvider = serviceProvider;
        }

        public IQueryable<JrpDashboardBeneficairyRawData> GetBeneficairyRawDatas(UnicefEduDbContext unicefEduDbContext, long instanceId,long entityDynamicColumnId)
        {
            return null;
            //TODO: beneficiary new schema
            //var query = (from edc in unicefEduDbContext.EntityDynamicColumn
            //             join bdc in unicefEduDbContext.BeneficiaryDynamicCells on edc.Id equals bdc.EntityDynamicColumnId
            //             join bi in unicefEduDbContext.Beneficiary on bdc.BeneficiaryId equals bi.Id
            //             where bdc.InstanceId == instanceId
            //             && bdc.EntityDynamicColumnId == entityDynamicColumnId
            //             select new JrpDashboardBeneficairyRawData()
            //             {
            //                 BeneficiaryCampId = bi.BeneficiaryCampId,
            //                 DateOfBirth = bi.DateOfBirth,
            //                 Disabled = bi.Disabled,
            //                 LevelOfStudy = bi.LevelOfStudy,
            //                 Sex = bi.Sex,
            //                 Value = bdc.Value
            //             });
            //return query;
        }

        private (int, int) GetAgeGroupValue(AgeGroup ageGroup)
        {
            if (ageGroup == AgeGroup.Three)
                return (0, 3);
            else if (ageGroup == AgeGroup.Four_Five)
                return (4, 5);
            else if (ageGroup == AgeGroup.Six_Fourteen)
                return (6, 14);
            else if (ageGroup == AgeGroup.Fifteen_Eighteen)
                return (15, 18);
            else if (ageGroup == AgeGroup.Nineteen_TwentyFour)
                return (19, 24);
            return (0, 0);
        }

        public IQueryable<JrpDashboardBeneficairyRawData> ApplyFilterToBeneficiaryRawData(IQueryable<JrpDashboardBeneficairyRawData> query
            , JrpPropertiesWithFilterViewModel jrpDashboardFilter)
        {
            if (jrpDashboardFilter.CampId != null)
            {
                query = query.Where(a => a.BeneficiaryCampId == jrpDashboardFilter.CampId);
            }
            if (jrpDashboardFilter.Age != null)
            {
                var ageGroup = GetAgeGroupValue((AgeGroup)jrpDashboardFilter.Age);

                query = query.Where(a => a.Age >= ageGroup.Item1 && a.Age <= ageGroup.Item2);
            }
            if (jrpDashboardFilter.Disability != null)
            {
                query = query.Where(a => a.Disabled == jrpDashboardFilter.Disability);
            }
            if (jrpDashboardFilter.Level != null)
            {
                query = query.Where(a => a.LevelOfStudy == jrpDashboardFilter.Level);
            }
            if (jrpDashboardFilter.Gender != null)
            {
                query = query.Where(a => a.Sex == jrpDashboardFilter.Gender);
            }
            return query;
        }


        public IQueryable<JrpDashboardFacilityRawData> GetFacilityRawDatas(UnicefEduDbContext unicefEduDbContext, long instanceId,long entityDynamicColumnId)
        {
            var query = (from edc in unicefEduDbContext.EntityDynamicColumn
                         join fdc in unicefEduDbContext.FacilityDynamicCells on edc.Id equals fdc.EntityDynamicColumnId
                         join fa in unicefEduDbContext.FacilityView on new { fdc.InstanceId, fdc.FacilityId } equals new { fa.InstanceId,FacilityId = fa.Id}
                         where fdc.InstanceId == instanceId
                         && fdc.EntityDynamicColumnId == entityDynamicColumnId
                         select new JrpDashboardFacilityRawData()
                         {
                             CampId = fa.CampId ?? 0,
                             ImplementationPartnerId = fa.ImplementationPartnerId,
                             ProgramPartnerId = fa.ProgramPartnerId,
                             Value = fdc.Value,
                             TargetedPopulation = fa.TargetedPopulation
                         });
            var data = query.ToList();
            return query;

        }

        public IQueryable<JrpDashboardFacilityRawData> ApplyFilterToFacilityRawData(IQueryable<JrpDashboardFacilityRawData> query
           , JrpPropertiesWithFilterViewModel jrpDashboardFilter)
        {
            if (jrpDashboardFilter.CampId != null)
            {
                query = query.Where(a => a.CampId == jrpDashboardFilter.CampId);
            }
            if (jrpDashboardFilter.TargetedPopulationId != null)
            {
                query = query.Where(a => a.TargetedPopulation == jrpDashboardFilter.TargetedPopulationId);
            }

            if (jrpDashboardFilter.ProgramPartnerId != null)
            {
                query = query.Where(a => a.ProgramPartnerId == jrpDashboardFilter.ProgramPartnerId);
            }
            if (jrpDashboardFilter.ImplementationPartnerId != null)
            {
                query = query.Where(a => a.ImplementationPartnerId == jrpDashboardFilter.ImplementationPartnerId);
            }
            return query;
        }


        public JrpChartData GetJrpChartData(JrpPropertiesWithFilterViewModel jrpReportFilter, long benficiaryInstanceId, long facilityInstanceId)
        {
            var chartData = new JrpChartData();

            using (var scope = _serviceProvider.CreateScope())
            {
                var _ucontext =
                scope.ServiceProvider
                    .GetRequiredService<UnicefEduDbContext>();

                var objectiveIndicator = _ucontext.ObjectiveIndicators.FirstOrDefault(a => a.Id == jrpReportFilter.JrpParameterInfo.TargetId);
                var entityColumnData = _ucontext.EntityDynamicColumn.FirstOrDefault(a => a.Id == jrpReportFilter.JrpParameterInfo.IndicatorId);
                chartData.Target = objectiveIndicator.Target;
                decimal achievement = 0;

                if (entityColumnData.EntityTypeId == EntityType.Beneficiary)
                {
                   
                    var rawData = ApplyFilterToBeneficiaryRawData(GetBeneficairyRawDatas(_ucontext, benficiaryInstanceId, entityColumnData.Id), jrpReportFilter).ToList();
                    achievement = rawData.Where(x => decimal.TryParse(x.Value, out var val) && val > 0)
                        .Select(a => Convert.ToDecimal(a.Value)).Sum();

                }
                else if (entityColumnData.EntityTypeId == EntityType.Facility)
                {
                    var rawData = ApplyFilterToFacilityRawData(GetFacilityRawDatas(_ucontext, facilityInstanceId, entityColumnData.Id), jrpReportFilter).ToList();
                    achievement = rawData.Where(x => decimal.TryParse(x.Value, out var val) && val > 0)
                        .Select(a => Convert.ToDecimal(a.Value)).Sum();
                }

                chartData.Achievement = achievement;
            }

            return chartData;
        }
    }
}
