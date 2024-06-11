using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UnicefEducationMIS.Core;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.Models.Views;

namespace UnicefEducationMIS.Data
{
    public partial class UnicefEduDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private readonly ICurrentLoginUserService _currentUserService;

        public UnicefEduDbContext(DbContextOptions<UnicefEduDbContext> options, ICurrentLoginUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        public virtual DbSet<Beneficiary> Beneficiary { get; set; }
        public virtual DbSet<GridView> GridViews { get; set; }
        public virtual DbSet<GridViewDetails> GridViewDetails { get; set; }
        public virtual DbSet<BeneficiaryDynamicCell> BeneficiaryDynamicCells { get; set; }
        public virtual DbSet<BeneficiaryDataCollectionStatus> BeneciaryDataCollectionStatuses { get; set; }
        public virtual DbSet<FacilityDataCollectionStatus> FacilityDataCollectionStatuses { get; set; }
        public virtual DbSet<EntityDynamicColumn> EntityDynamicColumn { get; set; }
        public virtual DbSet<ListDataType> ListDataType { get; set; }
        public virtual DbSet<ListItem> ListItems { get; set; }

        public virtual DbSet<Facility> Facility { get; set; }
        public virtual DbSet<FacilityDynamicCell> FacilityDynamicCells { get; set; }
        // public virtual DbSet<Indicator> Indicators { get; set; }

        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<PermissionPreset> PermissionPresets { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<UserLevel> UserLevels { get; set; }
        public virtual DbSet<UserDynamicCell> UserDynamicCells { get; set; }

        public virtual DbSet<EducationSectorPartner> EducationSectorPartners { get; set; }

        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Upazila> Upazilas { get; set; }
        public virtual DbSet<Union> Unions { get; set; }
        public virtual DbSet<Camp> Camps { get; set; }
        public virtual DbSet<Block> Blocks { get; set; }
        public virtual DbSet<SubBlock> SubBlocks { get; set; }

        public virtual DbSet<InstanceMapping> InstanceMappings { get; set; }
        public virtual DbSet<InstanceIndicator> InstanceIndicators { get; set; }
        public virtual DbSet<Instance> ScheduleInstances { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<MonitoringFramework> Monitorings { get; set; }
        public virtual DbSet<BudgetFramework> Budgets { get; set; }

        public virtual DbSet<TargetFramework> Targets { get; set; }


        public virtual DbSet<MonitoringFrameworkDynamicCell> MonitoringDynamicCells { get; set; }
        public virtual DbSet<BudgetFrameworkDynamicCell> BudgetDynamicCells { get; set; }
        public virtual DbSet<TargetFrameworkDynamicCell> TargetDynamicCells { get; set; }
        public virtual DbSet<LogEntry> LogEntries { get; set; }
        public virtual DbSet<CampCoordinate> CampCoordinates { get; set; }
        public virtual DbSet<AgeGroup> AgeGroups { get; set; }
        public virtual DbSet<ReportingFrequency> ReportingFrequencies { get; set; }

        public virtual DbSet<JrpParameterInfo> JrpParameterInfos { get; set; }
        public virtual DbSet<ObjectiveIndicator> ObjectiveIndicators { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }

        public DbSet<BeneficiaryView> BeneficiaryView { get; set; }
        public DbSet<FacilityView> FacilityView { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnicefEduDbContext).Assembly);
            // Don't call base.OnModelCreating(modelBuilder);
            // It's not required: https://stackoverflow.com/questions/39576176/is-base-onmodelcreatingmodelbuilder-necessary
            // and in this particular case creates problems in migrations.

            // TODO: Transfer these to Configurations folder in seperate classes


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var item in ChangeTracker.Entries<IAuditable>())
            {
                switch (item.State)
                {
                    case EntityState.Added:
                        item.Entity.DateCreated = DateTime.Now;
                        item.Entity.CreatedBy = _currentUserService.UserId;
                        break;
                    case EntityState.Modified:
                        item.Entity.LastUpdated = DateTime.Now;
                        item.Entity.UpdatedBy = _currentUserService.UserId;
                        break;
                    default:
                        break;
                }
            }

            return base.SaveChangesAsync();
        }
    }
}
