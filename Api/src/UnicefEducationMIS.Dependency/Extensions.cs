using Microsoft.Extensions.DependencyInjection;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Factories;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Data.Factory;
using UnicefEducationMIS.Data.Logging;
using UnicefEducationMIS.Data.Repositories;
using UnicefEducationMIS.Service.ApplicationServices;
using UnicefEducationMIS.Service.DomainServices;
using UnicefEducationMIS.Service.Helpers;
using UnicefEducationMIS.Service.Import;
using UnicefEducationMIS.Service.Report;

namespace UnicefEducationMIS.Dependency
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddScoped<IBeneficiaryService, BeneficiaryService>();
            
            services.AddScoped<IBeneficiaryService, BeneficiaryService>();
            services.AddScoped<IGridViewService, GridViewService>();
            services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
            services.AddScoped<IGridViewRepository, GridViewRepository>();
            services.AddScoped<IBeneficiaryDynamicCellRepository, BeneficiaryDynamicCellRepository>();
            services.AddScoped<IBeneficiaryDynamicCellService, BeneficiaryDynamicCellService>();
            services.AddScoped<IFacilityDynamicCellRepository, FacilityDynamicCellRepository>();
            services.AddScoped<IFacilityDynamicCellService, FacilityDynamicCellService>();

            services.AddScoped<IDynamicColumnRepositories, DynamicPropertyRepository>();
            services.AddScoped<IDynamicPropertiesService, DynamicPropertiesService>();
            services.AddScoped<ICurrentLoginUserService, CurrentLoginUserService>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<BeneficiaryUserConditionFactory>();
            services.AddScoped<FacilityUserConditionFactory>();

            services.AddScoped<IListDataTypeService, ListDataTypeService>();
            services.AddScoped<IListDataTypeRepository, ListDataTypeRepository>();
            services.AddScoped<IIndicatorRepository, IndicatorRepository>();
            services.AddScoped<IIndicatorService, IndicatorService>();

            services.AddScoped<IMonitoringFrameworkService, MonitoringFrameworkService>();
            services.AddScoped<IBudgetFrameworkService, BudgetFrameworkService>();
            services.AddScoped<ITargetFrameworkService, TargetFrameworkService>();
            services.AddScoped<IMonitoringFrameworkRepository, MonitoringFrameworkRepository>();
            services.AddScoped<IBudgetFrameworkRepository, BudgetFrameworkRepository>();
            services.AddScoped<ITargetFrameworkRepository, TargetFrameworkRepository>();

            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<ICampRepository, CampRepository>();
            services.AddScoped<IUpazilaRepository, UpazilaRepository>();
            services.AddScoped<IUnionRepository, UnionRepository>();
            services.AddScoped<IBlockRepository, BlockRepository>();
            services.AddScoped<ISubBlockRepository, SubBlockRepository>();
                        
            services.AddScoped<IUserLevelRepository, UserLevelRepository>();
            services.AddScoped<IPermissionPresetRepository, PermissionPresetRepository>();
            services.AddScoped<IEducationSectorPartnerRepository, EducationSectorPartnerRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEducationSectorPartnerService, EducationSectorPartnerService>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IScheduleInstanceRepository, ScheduleInstanceRepository>();
            services.AddScoped<IScheduleService, ScheduleService>();

            services.AddScoped<ISchedulerFactory, SchedulerFactory>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IBeneficiaryDataCollectionStatusRepository, BeneficiaryDataCollectionStatusRepository>();
            services.AddScoped<IFacilityDataCollectionStatusRepository, FacilityDataCollectionStatusRepository>();            
            services.AddScoped<IFacilityService, FacilityService>();
            services.AddScoped<IFacilityRepository, FacilityRepository>();                        
            services.AddScoped<IDataApprovalService, DataApprovalService>();
            services.AddScoped<IBeneficiaryDataCollectionRepository, BeneficiaryDataCollectionRepository>();
            services.AddScoped<IFacilityDataCollectionRepository, FacilityDataCollectionRepository>();
            services.AddTransient<IDbLoggingRepository, DbLoggingRepository>();
            services.AddSingleton<IDbContextService, DbContextService>();
            services.AddScoped<IBeneficiaryImporter, BeneficiaryImporter>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IFiveWReportBuilder, FiveWReportBuilder>();
            services.AddScoped<ICampWiseReportBuilder, CampWiseReportBuilder>();
            services.AddScoped<IBeneficiaryVersionDataImportFileBuilder, BeneficiaryVersionDataImportFileBuilder>();
            services.AddScoped<IFacilityVersionDataImportFileBuilder, FacilityVersionDataImportFileBuilder>();
            services.AddScoped<IBeneficiaryVersionImporter, BeneficiaryVersionImporter>();
            services.AddScoped<IBeneficiaryExporter, BeneficiaryExporter>();
            services.AddScoped<IFacilityExporter, FacilityExporter>();
            services.AddTransient<IJrpPropertiesInfoRepository, JrpPropertiesInfoRepository>();
            services.AddTransient<IJrpPropertiesInfoService, JrpPropertiesInfoService>();
            services.AddScoped<IEmbeddedDashboardRepository, EmbeddedDashboardRepository>();
            services.AddScoped<IEmbeddedDashboardService, EmbeddedDashboardService>();
            services.AddScoped<MailHelper>();

            services.AddScoped<IDuplicationReportExporter, DuplicationReportExporter>();
            services.AddScoped<IGapAnalysisReportExporter, GapAnalysisReportExporter>();
            services.AddScoped<IDamageReportExporter, DamageReportExporter>();
            services.AddScoped<ISummaryReportExporter, SummaryReportExporter>();
            services.AddScoped<IUserExporter, UserExporter>();

            services.AddScoped<ICampCoordinateRepository, CampCoordinateRepository>();

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IInstanceMappingRepository, InstanceMappingRepository>();

            return services;
        }  
    }
}
