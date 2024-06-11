using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace UnicefEducationMIS.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Beneficiary",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Education_Sector_Partners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    PartnerName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education_Sector_Partners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Embedded_Dashboard",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    IsEmbedded = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Embedded_Dashboard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FR_Age_Group",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Age_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FR_Donor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Donor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FR_Monitorings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    Objective = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Monitorings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FR_Project",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FR_Reporting_Frequency",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Reporting_Frequency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grid_View",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(maxLength: 2048, nullable: true),
                    EntityTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grid_View", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instance_Mapping",
                columns: table => new
                {
                    BeneficiaryInstanceId = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    FacilityInstanceId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instance_Mapping", x => x.BeneficiaryInstanceId);
                });

            migrationBuilder.CreateTable(
                name: "List_Object",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_List_Object", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    TimeStampUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    Category = table.Column<string>(maxLength: 128, nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    StateText = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission_Presets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    PresetName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission_Presets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    PermissionName = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    ScheduleName = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ScheduleType = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ScheduleFor = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Claims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Claims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Levels",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    LevelName = table.Column<string>(maxLength: 128, nullable: true),
                    Rank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Logins",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: true),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: true),
                    ProviderDisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Logins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "User_Tokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Tokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 128, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 128, nullable: true),
                    Email = table.Column<string>(maxLength: 128, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 128, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    DesignationName = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Upazila",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 2048, nullable: true),
                    DistrictId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upazila", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Upazila__DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FR_Budgets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    DonorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Budgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FR_Budgets_FR_Donor_DonorId",
                        column: x => x.DonorId,
                        principalTable: "FR_Donor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FR_Budgets_FR_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "FR_Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FR_Objective_Indicators",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    Indicator = table.Column<string>(maxLength: 512, nullable: true),
                    Unit = table.Column<string>(maxLength: 512, nullable: true),
                    BaseLine = table.Column<int>(nullable: false),
                    Target = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    ReportingFrequencyId = table.Column<int>(nullable: false),
                    MonitoringFrameworkId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Objective_Indicators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FR_Objective_Indicators_FR_Monitorings_MonitoringFrameworkId",
                        column: x => x.MonitoringFrameworkId,
                        principalTable: "FR_Monitorings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FR_Objective_Indicators_Education_Sector_Partners_Organizati~",
                        column: x => x.OrganizationId,
                        principalTable: "Education_Sector_Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FR_Objective_Indicators_FR_Reporting_Frequency_ReportingFreq~",
                        column: x => x.ReportingFrequencyId,
                        principalTable: "FR_Reporting_Frequency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entity_Dynamic_Column",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(maxLength: 2048, nullable: true),
                    NameInBangla = table.Column<string>(nullable: true),
                    EntityTypeId = table.Column<int>(nullable: false),
                    ColumnType = table.Column<int>(nullable: false),
                    Unit = table.Column<string>(maxLength: 1024, nullable: true),
                    IsFixed = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    IsMultiValued = table.Column<bool>(type: "BOOLEAN", nullable: true),
                    ColumnListId = table.Column<long>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TargetedPopulation = table.Column<int>(nullable: true),
                    IsAutoCalculated = table.Column<bool>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity_Dynamic_Column", x => x.Id);
                    table.ForeignKey(
                        name: "FK__EntityDynamicColumns__ColumnListId",
                        column: x => x.ColumnListId,
                        principalTable: "List_Object",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "List_Item",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(maxLength: 2048, nullable: true),
                    Value = table.Column<int>(type: "int", nullable: false),
                    ColumnListId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_List_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ListItems__ColumnListId",
                        column: x => x.ColumnListId,
                        principalTable: "List_Object",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    NotificationTypeId = table.Column<int>(nullable: false),
                    Actor = table.Column<int>(nullable: false),
                    User = table.Column<int>(nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: true),
                    Data = table.Column<string>(type: "text", nullable: true),
                    Details = table.Column<string>(type: "text", nullable: true),
                    IsActed = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Notification__NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permission_Preset_Permission",
                columns: table => new
                {
                    PermissionId = table.Column<int>(nullable: false),
                    PermissionPresetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission_Preset_Permission", x => new { x.PermissionId, x.PermissionPresetId });
                    table.ForeignKey(
                        name: "FK_Permission_Preset_Permission_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permission_Preset_Permission_Permission_Presets_PermissionPr~",
                        column: x => x.PermissionPresetId,
                        principalTable: "Permission_Presets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedule_Frequencies",
                columns: table => new
                {
                    ScheduleId = table.Column<long>(nullable: false),
                    Interval = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: true),
                    Month = table.Column<int>(nullable: true),
                    DaysOfWeek = table.Column<string>(maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule_Frequencies", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_Schedule_Frequencies_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedule_Instances",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    ScheduleId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(maxLength: 256, nullable: true),
                    DataCollectionDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule_Instances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedule_Instances_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 128, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    LevelId = table.Column<byte>(nullable: false),
                    PermissionPresetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_User_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "User_Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Roles_Permission_Presets_PermissionPresetId",
                        column: x => x.PermissionPresetId,
                        principalTable: "Permission_Presets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_EduSector_Partners",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    PartnerId = table.Column<int>(nullable: false),
                    PartnerType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_EduSector_Partners", x => new { x.UserId, x.PartnerId, x.PartnerType });
                    table.ForeignKey(
                        name: "FK_User_EduSector_Partners_Education_Sector_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Education_Sector_Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_EduSector_Partners_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Union",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(maxLength: 2048, nullable: true),
                    UpazilaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Union", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Unions__UpazilaId",
                        column: x => x.UpazilaId,
                        principalTable: "Upazila",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FR_Budget_Dynamic_Cells",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    BudgetFrameworkId = table.Column<long>(nullable: false),
                    EntityDynamicColumnId = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Budget_Dynamic_Cells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FR_Budget_Dynamic_Cells_FR_Budgets_BudgetFrameworkId",
                        column: x => x.BudgetFrameworkId,
                        principalTable: "FR_Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FR_Budget_Dynamic_Cells_Entity_Dynamic_Column_EntityDynamicC~",
                        column: x => x.EntityDynamicColumnId,
                        principalTable: "Entity_Dynamic_Column",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FR_Monitoring_Dynamic_Cells",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    ObjectiveIndicatorId = table.Column<long>(nullable: false),
                    EntityDynamicColumnId = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Monitoring_Dynamic_Cells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FR_Monitoring_Dynamic_Cells_Entity_Dynamic_Column_EntityDyna~",
                        column: x => x.EntityDynamicColumnId,
                        principalTable: "Entity_Dynamic_Column",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FR_Monitoring_Dynamic_Cells_FR_Objective_Indicators_Objectiv~",
                        column: x => x.ObjectiveIndicatorId,
                        principalTable: "FR_Objective_Indicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grid_View_Details",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    EntityDynamicColumnId = table.Column<long>(type: "bigint", nullable: false),
                    ColumnOrder = table.Column<int>(type: "int", nullable: false),
                    GridViewId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grid_View_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BeneficiaryViewDetails__EntityColumnId",
                        column: x => x.EntityDynamicColumnId,
                        principalTable: "Entity_Dynamic_Column",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__BeneficiaryViewDetails__BeneficiaryGridViewId",
                        column: x => x.GridViewId,
                        principalTable: "Grid_View",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jrp_Parameter_Info",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: true),
                    TargetId = table.Column<long>(nullable: false),
                    IndicatorId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jrp_Parameter_Info", x => x.Id);
                    table.ForeignKey(
                        name: "FK__JrpParameterInfo__IndicatorId",
                        column: x => x.IndicatorId,
                        principalTable: "Entity_Dynamic_Column",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__JrpParameterInfo__TargetId",
                        column: x => x.TargetId,
                        principalTable: "FR_Objective_Indicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Dynamic_Cells",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    EntityDynamicColumnId = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Dynamic_Cells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Dynamic_Cells_Entity_Dynamic_Column_EntityDynamicColumn~",
                        column: x => x.EntityDynamicColumnId,
                        principalTable: "Entity_Dynamic_Column",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Dynamic_Cells_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Beneciary_Data_Collection_Status",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    BeneficiaryId = table.Column<long>(type: "bigint", nullable: false),
                    InstanceId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: true),
                    ApprovalDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneciary_Data_Collection_Status", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BeneciaryDataCollectionStatuses__BeneficiaryId",
                        column: x => x.BeneficiaryId,
                        principalTable: "Beneficiary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__BeneciaryDataCollectionStatuses__InstanceId",
                        column: x => x.InstanceId,
                        principalTable: "Schedule_Instances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Beneficiary_Dynamic_Cell",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    EntityDynamicColumnId = table.Column<long>(nullable: false),
                    BeneficiaryId = table.Column<long>(type: "bigint", nullable: false),
                    InstanceId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiary_Dynamic_Cell", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BeenficiaryDynamicCell__BeneficairyId",
                        column: x => x.BeneficiaryId,
                        principalTable: "Beneficiary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__DynamicCe__EntityColumnId",
                        column: x => x.EntityDynamicColumnId,
                        principalTable: "Entity_Dynamic_Column",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Beneficiary_Dynamic_Cell_Schedule_Instances_InstanceId",
                        column: x => x.InstanceId,
                        principalTable: "Schedule_Instances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facilities_DataCollection_Status",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    FacilityId = table.Column<long>(nullable: false),
                    InstanceId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: true),
                    ApprovalDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities_DataCollection_Status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facilities_DataCollection_Status_Facility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facilities_DataCollection_Status_Schedule_Instances_Instance~",
                        column: x => x.InstanceId,
                        principalTable: "Schedule_Instances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facility_Dynamic_Cell",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(nullable: true),
                    EntityDynamicColumnId = table.Column<long>(nullable: false),
                    FacilityId = table.Column<long>(nullable: false),
                    InstanceId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility_Dynamic_Cell", x => x.Id);
                    table.ForeignKey(
                        name: "FK__FacilityDynamicCells__EntityDynamicColumnId",
                        column: x => x.EntityDynamicColumnId,
                        principalTable: "Entity_Dynamic_Column",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__FacilityDynamicCells__FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__FacilityDynamicCells__InstanceId",
                        column: x => x.InstanceId,
                        principalTable: "Schedule_Instances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instance_Indicator",
                columns: table => new
                {
                    EntityDynamicColumnId = table.Column<long>(nullable: false),
                    InstanceId = table.Column<long>(nullable: false),
                    Id = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    ColumnOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instance_Indicator", x => new { x.EntityDynamicColumnId, x.InstanceId });
                    table.ForeignKey(
                        name: "FK_InstanceIndicators_EntityDynamicColumnId",
                        column: x => x.EntityDynamicColumnId,
                        principalTable: "Entity_Dynamic_Column",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstanceIndicators_InstanceId",
                        column: x => x.InstanceId,
                        principalTable: "Schedule_Instances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role_Claims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Claims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role_Permissions",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_Permissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_Role_Permissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Role_Permissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Roles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_User_Roles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Roles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Camp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    SSID = table.Column<string>(maxLength: 1024, nullable: true),
                    Name = table.Column<string>(maxLength: 2048, nullable: true),
                    NameAlias = table.Column<string>(nullable: true),
                    UnionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camp", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Camps__UnionId",
                        column: x => x.UnionId,
                        principalTable: "Union",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Block",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 2048, nullable: true),
                    CampId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Block", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Blocks__CampId",
                        column: x => x.CampId,
                        principalTable: "Camp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Camp_Coordinate",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CampId = table.Column<int>(nullable: false),
                    SequenceNumber = table.Column<int>(nullable: false),
                    Latitude = table.Column<double>(type: "double", nullable: false),
                    Longitude = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camp_Coordinate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Camp_Coordinate_Camp_CampId",
                        column: x => x.CampId,
                        principalTable: "Camp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FR_Targets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    CampId = table.Column<int>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    AgeGroupId = table.Column<int>(nullable: false),
                    PeopleInNeed = table.Column<int>(nullable: false),
                    Target = table.Column<int>(nullable: false),
                    StartYear = table.Column<int>(nullable: false),
                    StartMonth = table.Column<int>(nullable: false),
                    EndYear = table.Column<int>(nullable: false),
                    EndMonth = table.Column<int>(nullable: false),
                    UpazilaId = table.Column<int>(nullable: false),
                    UnionId = table.Column<int>(nullable: false),
                    TargetedPopulation = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Targets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FR_Targets_FR_Age_Group_AgeGroupId",
                        column: x => x.AgeGroupId,
                        principalTable: "FR_Age_Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FR_Targets_Camp_CampId",
                        column: x => x.CampId,
                        principalTable: "Camp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubBlock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(maxLength: 2048, nullable: true),
                    BlockId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubBlock", x => x.Id);
                    table.ForeignKey(
                        name: "FK__SubBlocks__BlockId",
                        column: x => x.BlockId,
                        principalTable: "Block",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FR_Target_Dynamic_Cells",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    TargetFrameworkId = table.Column<long>(nullable: false),
                    EntityDynamicColumnId = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FR_Target_Dynamic_Cells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FR_Target_Dynamic_Cells_Entity_Dynamic_Column_EntityDynamicC~",
                        column: x => x.EntityDynamicColumnId,
                        principalTable: "Entity_Dynamic_Column",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FR_Target_Dynamic_Cells_FR_Targets_TargetFrameworkId",
                        column: x => x.TargetFrameworkId,
                        principalTable: "FR_Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "List_Object",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "LastUpdated", "Name", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, 1, new DateTime(2021, 5, 11, 11, 37, 15, 732, DateTimeKind.Local).AddTicks(7355), null, "Education Level", null },
                    { 2L, 1, new DateTime(2021, 5, 11, 11, 37, 15, 737, DateTimeKind.Local).AddTicks(2957), null, "Damage Status", null },
                    { 3L, 1, new DateTime(2021, 5, 11, 11, 37, 15, 737, DateTimeKind.Local).AddTicks(3253), null, "Damage Caused By", null },
                    { 4L, 1, new DateTime(2021, 5, 11, 11, 37, 15, 737, DateTimeKind.Local).AddTicks(3271), null, "Action Required", null },
                    { 5L, 1, new DateTime(2021, 5, 11, 11, 37, 15, 737, DateTimeKind.Local).AddTicks(3281), null, "Taken Action", null },
                    { 6L, 1, new DateTime(2021, 5, 11, 11, 37, 15, 737, DateTimeKind.Local).AddTicks(3300), null, "How many time the facility damage", null },
                    { 7L, 1, new DateTime(2021, 5, 11, 11, 37, 15, 737, DateTimeKind.Local).AddTicks(3311), null, "Targeted Population", null },
                    { 8L, 1, new DateTime(2021, 5, 11, 11, 37, 15, 737, DateTimeKind.Local).AddTicks(3323), null, "Facility Type", null },
                    { 9L, 1, new DateTime(2021, 5, 11, 11, 37, 15, 737, DateTimeKind.Local).AddTicks(3333), null, "Facility Status", null },
                    { 10L, 1, new DateTime(2021, 5, 11, 11, 37, 15, 737, DateTimeKind.Local).AddTicks(3347), null, "Sex", null }
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "LastUpdated", "Name", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2021, 5, 11, 11, 37, 15, 759, DateTimeKind.Local).AddTicks(9605), null, "Beneficiary Instance Start", null },
                    { 2, 1, new DateTime(2021, 5, 11, 11, 37, 15, 760, DateTimeKind.Local).AddTicks(1055), null, "Facility Instance Start", null },
                    { 3, 1, new DateTime(2021, 5, 11, 11, 37, 15, 760, DateTimeKind.Local).AddTicks(1084), null, "Recollect Facility", null },
                    { 4, 1, new DateTime(2021, 5, 11, 11, 37, 15, 760, DateTimeKind.Local).AddTicks(1088), null, "Recollect Beneficiary", null },
                    { 5, 1, new DateTime(2021, 5, 11, 11, 37, 15, 760, DateTimeKind.Local).AddTicks(1091), null, "Assign Teacher", null }
                });

            migrationBuilder.InsertData(
                table: "List_Item",
                columns: new[] { "Id", "ColumnListId", "CreatedBy", "DateCreated", "LastUpdated", "Title", "UpdatedBy", "Value" },
                values: new object[,]
                {
                    { 1L, 1L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Level 1", null, 1 },
                    { 31L, 5L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Planned for action", null, 4 },
                    { 32L, 5L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Decommisionned", null, 5 },
                    { 33L, 5L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "No action taken", null, 6 },
                    { 34L, 5L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Others(Mentioned on Remarks)", null, 7 },
                    { 35L, 6L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "1st time", null, 1 },
                    { 36L, 6L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "2nd time", null, 2 },
                    { 37L, 6L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "3rd time", null, 3 },
                    { 38L, 6L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "4th time", null, 4 },
                    { 39L, 6L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "5th time", null, 5 },
                    { 40L, 6L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "6th time", null, 6 },
                    { 41L, 6L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "7th time", null, 7 },
                    { 42L, 6L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "8th time", null, 8 },
                    { 43L, 6L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "9th time", null, 9 },
                    { 44L, 6L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "10th time", null, 10 },
                    { 45L, 7L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Host Communities", null, 1 },
                    { 46L, 7L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Refugee Communities", null, 2 },
                    { 47L, 7L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Both Communities", null, 3 },
                    { 48L, 8L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Learning Center(LC)", null, 1 },
                    { 49L, 8L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Community Based Learning Facility", null, 2 },
                    { 50L, 8L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Cross Sectoral Shared Learning Facility (CSSLF)", null, 3 },
                    { 51L, 9L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ongoing", null, 1 },
                    { 52L, 9L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Completed", null, 2 },
                    { 53L, 9L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Planned", null, 3 },
                    { 54L, 9L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Decommission", null, 4 },
                    { 55L, 10L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Male", null, 1 },
                    { 30L, 5L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Reallocation", null, 3 },
                    { 56L, 10L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Female", null, 2 },
                    { 29L, 5L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Repair work ongoing", null, 2 },
                    { 27L, 4L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Others (Mentioned on Remarks)", null, 3 },
                    { 2L, 1L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Level 2", null, 2 },
                    { 3L, 1L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Level 3", null, 3 },
                    { 4L, 1L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Level 4", null, 4 },
                    { 5L, 2L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Slightly Damaged", null, 1 },
                    { 6L, 2L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Medium Damaged", null, 2 },
                    { 7L, 2L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Heavily Damaged", null, 3 },
                    { 8L, 2L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Repaired", null, 4 },
                    { 9L, 2L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Decommissioned", null, 5 },
                    { 10L, 2L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Others (Mentioned on Remarks)", null, 6 },
                    { 11L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Cyclone", null, 1 },
                    { 12L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Flood", null, 2 },
                    { 13L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Landslide", null, 3 },
                    { 14L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Landslide & Flood", null, 4 },
                    { 15L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Landslide, Flood & Storm surge", null, 5 },
                    { 16L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Lighthening", null, 6 },
                    { 17L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Rain", null, 7 },
                    { 18L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Rain and Flood", null, 8 },
                    { 19L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Rain and Storm surge", null, 9 },
                    { 20L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Rain, Flood and storm surge", null, 10 },
                    { 21L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Road construction", null, 11 },
                    { 22L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Storm surge", null, 12 },
                    { 23L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Lands taken by authority", null, 13 },
                    { 24L, 3L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Others (Mentioned on Remarks)", null, 14 },
                    { 25L, 4L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tied-up/renovate LC that is in potential damage threat", null, 1 },
                    { 26L, 4L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Repair damaged LC", null, 2 },
                    { 28L, 5L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Repaired", null, 1 },
                    { 57L, 10L, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Others", null, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beneciary_Data_Collection_Status_BeneficiaryId",
                table: "Beneciary_Data_Collection_Status",
                column: "BeneficiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Beneciary_Data_Collection_Status_InstanceId",
                table: "Beneciary_Data_Collection_Status",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiary_Dynamic_Cell_BeneficiaryId",
                table: "Beneficiary_Dynamic_Cell",
                column: "BeneficiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiary_Dynamic_Cell_EntityDynamicColumnId",
                table: "Beneficiary_Dynamic_Cell",
                column: "EntityDynamicColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiary_Dynamic_Cell_InstanceId",
                table: "Beneficiary_Dynamic_Cell",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Block_CampId",
                table: "Block",
                column: "CampId");

            migrationBuilder.CreateIndex(
                name: "IX_Camp_UnionId",
                table: "Camp",
                column: "UnionId");

            migrationBuilder.CreateIndex(
                name: "IX_Camp_Coordinate_CampId",
                table: "Camp_Coordinate",
                column: "CampId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity_Dynamic_Column_ColumnListId",
                table: "Entity_Dynamic_Column",
                column: "ColumnListId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_DataCollection_Status_FacilityId",
                table: "Facilities_DataCollection_Status",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_DataCollection_Status_InstanceId",
                table: "Facilities_DataCollection_Status",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_Dynamic_Cell_EntityDynamicColumnId",
                table: "Facility_Dynamic_Cell",
                column: "EntityDynamicColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_Dynamic_Cell_FacilityId",
                table: "Facility_Dynamic_Cell",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_Dynamic_Cell_InstanceId",
                table: "Facility_Dynamic_Cell",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Budget_Dynamic_Cells_BudgetFrameworkId",
                table: "FR_Budget_Dynamic_Cells",
                column: "BudgetFrameworkId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Budget_Dynamic_Cells_EntityDynamicColumnId",
                table: "FR_Budget_Dynamic_Cells",
                column: "EntityDynamicColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Budgets_DonorId",
                table: "FR_Budgets",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Budgets_ProjectId",
                table: "FR_Budgets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Monitoring_Dynamic_Cells_EntityDynamicColumnId",
                table: "FR_Monitoring_Dynamic_Cells",
                column: "EntityDynamicColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Monitoring_Dynamic_Cells_ObjectiveIndicatorId",
                table: "FR_Monitoring_Dynamic_Cells",
                column: "ObjectiveIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Objective_Indicators_MonitoringFrameworkId",
                table: "FR_Objective_Indicators",
                column: "MonitoringFrameworkId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Objective_Indicators_OrganizationId",
                table: "FR_Objective_Indicators",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Objective_Indicators_ReportingFrequencyId",
                table: "FR_Objective_Indicators",
                column: "ReportingFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Target_Dynamic_Cells_EntityDynamicColumnId",
                table: "FR_Target_Dynamic_Cells",
                column: "EntityDynamicColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Target_Dynamic_Cells_TargetFrameworkId",
                table: "FR_Target_Dynamic_Cells",
                column: "TargetFrameworkId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Targets_AgeGroupId",
                table: "FR_Targets",
                column: "AgeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FR_Targets_CampId",
                table: "FR_Targets",
                column: "CampId");

            migrationBuilder.CreateIndex(
                name: "IX_Grid_View_Details_EntityDynamicColumnId",
                table: "Grid_View_Details",
                column: "EntityDynamicColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_Grid_View_Details_GridViewId",
                table: "Grid_View_Details",
                column: "GridViewId");

            migrationBuilder.CreateIndex(
                name: "IX_Instance_Indicator_InstanceId",
                table: "Instance_Indicator",
                column: "InstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Jrp_Parameter_Info_IndicatorId",
                table: "Jrp_Parameter_Info",
                column: "IndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Jrp_Parameter_Info_TargetId",
                table: "Jrp_Parameter_Info",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_List_Item_ColumnListId",
                table: "List_Item",
                column: "ColumnListId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Preset_Permission_PermissionPresetId",
                table: "Permission_Preset_Permission",
                column: "PermissionPresetId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Claims_RoleId",
                table: "Role_Claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Permissions_PermissionId",
                table: "Role_Permissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_LevelId",
                table: "Roles",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_PermissionPresetId",
                table: "Roles",
                column: "PermissionPresetId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Instances_ScheduleId",
                table: "Schedule_Instances",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SubBlock_BlockId",
                table: "SubBlock",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Union_UpazilaId",
                table: "Union",
                column: "UpazilaId");

            migrationBuilder.CreateIndex(
                name: "IX_Upazila_DistrictId",
                table: "Upazila",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Dynamic_Cells_EntityDynamicColumnId",
                table: "User_Dynamic_Cells",
                column: "EntityDynamicColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Dynamic_Cells_UserId",
                table: "User_Dynamic_Cells",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_EduSector_Partners_PartnerId",
                table: "User_EduSector_Partners",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Roles_RoleId",
                table: "User_Roles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beneciary_Data_Collection_Status");

            migrationBuilder.DropTable(
                name: "Beneficiary_Dynamic_Cell");

            migrationBuilder.DropTable(
                name: "Camp_Coordinate");

            migrationBuilder.DropTable(
                name: "Embedded_Dashboard");

            migrationBuilder.DropTable(
                name: "Facilities_DataCollection_Status");

            migrationBuilder.DropTable(
                name: "Facility_Dynamic_Cell");

            migrationBuilder.DropTable(
                name: "FR_Budget_Dynamic_Cells");

            migrationBuilder.DropTable(
                name: "FR_Monitoring_Dynamic_Cells");

            migrationBuilder.DropTable(
                name: "FR_Target_Dynamic_Cells");

            migrationBuilder.DropTable(
                name: "Grid_View_Details");

            migrationBuilder.DropTable(
                name: "Instance_Indicator");

            migrationBuilder.DropTable(
                name: "Instance_Mapping");

            migrationBuilder.DropTable(
                name: "Jrp_Parameter_Info");

            migrationBuilder.DropTable(
                name: "List_Item");

            migrationBuilder.DropTable(
                name: "LogEntry");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Permission_Preset_Permission");

            migrationBuilder.DropTable(
                name: "Role_Claims");

            migrationBuilder.DropTable(
                name: "Role_Permissions");

            migrationBuilder.DropTable(
                name: "Schedule_Frequencies");

            migrationBuilder.DropTable(
                name: "SubBlock");

            migrationBuilder.DropTable(
                name: "User_Claims");

            migrationBuilder.DropTable(
                name: "User_Dynamic_Cells");

            migrationBuilder.DropTable(
                name: "User_EduSector_Partners");

            migrationBuilder.DropTable(
                name: "User_Logins");

            migrationBuilder.DropTable(
                name: "User_Roles");

            migrationBuilder.DropTable(
                name: "User_Tokens");

            migrationBuilder.DropTable(
                name: "Beneficiary");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "FR_Budgets");

            migrationBuilder.DropTable(
                name: "FR_Targets");

            migrationBuilder.DropTable(
                name: "Grid_View");

            migrationBuilder.DropTable(
                name: "Schedule_Instances");

            migrationBuilder.DropTable(
                name: "FR_Objective_Indicators");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Block");

            migrationBuilder.DropTable(
                name: "Entity_Dynamic_Column");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "FR_Donor");

            migrationBuilder.DropTable(
                name: "FR_Project");

            migrationBuilder.DropTable(
                name: "FR_Age_Group");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "FR_Monitorings");

            migrationBuilder.DropTable(
                name: "Education_Sector_Partners");

            migrationBuilder.DropTable(
                name: "FR_Reporting_Frequency");

            migrationBuilder.DropTable(
                name: "Camp");

            migrationBuilder.DropTable(
                name: "List_Object");

            migrationBuilder.DropTable(
                name: "User_Levels");

            migrationBuilder.DropTable(
                name: "Permission_Presets");

            migrationBuilder.DropTable(
                name: "Union");

            migrationBuilder.DropTable(
                name: "Upazila");

            migrationBuilder.DropTable(
                name: "District");
        }
    }
}
