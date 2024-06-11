using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.Repositories.Framework;
using UnicefEducationMIS.Data.Repositories;
using UnicefEducationMIS.Data.Repositories.Framework;

namespace UnicefEducationMIS.Dependency
{
    public static class Repositories
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMonitoringDynamicCellRepository, MonitoringDynamicCellRepository>();
            services.AddScoped<IBudgetDynamicCellRepository, BudgetDynamicCellRepository>();
            services.AddScoped<ITargetDynamicCellRepository, TargetDynamicCellRepository>();

            services.AddScoped<IObjectiveIndicatorRepository, ObjectiveIndicatorRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IDonorRepository, DonorRepository>();
            services.AddScoped<IReportingFrequencyRepository, ReportingFrequencyRepository>();
            services.AddScoped<IAgeGroupRepository, AgeGroupRepository>();
            services.AddScoped<IUserDynamicCellRepository, UserDynamicCellRepository>();
            return services;
        }
    }
}
