using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Service.DomainServices;
using UnicefEducationMIS.Service.Import;

namespace UnicefEducationMIS.Dependency
{
    public static class DomainServices
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddRepositories();

            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IObjectiveIndicatorService, ObjectiveIndicatorService>();
            services.AddScoped<IModelToIndicatorConverter, ModelToIndicatorConverter>();
            services.AddScoped<IFacilityVersionImporter, FacilityVersionImporter>();
            return services;
        }
    }
}
