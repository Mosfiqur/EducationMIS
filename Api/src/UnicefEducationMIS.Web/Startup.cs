using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpOverrides;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Data;
using UnicefEducationMIS.Dependency;
using UnicefEducationMIS.Web.Configurations;
using UnicefEducationMIS.Web.Helpers;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DocumentFormat.OpenXml.Bibliography;
using System.ComponentModel;
using Google.Protobuf.WellKnownTypes;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Settings;
using UnicefEducationMIS.Web.Seeders;

namespace UnicefEducationMIS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static IServiceProvider ServiceContainer { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
                        
            services.AddDbContext<UnicefEduDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseMySQL(Configuration.GetConnectionString(ConfigurationConstants.ConnectionStringName));  
            });

            services.AddApplicationDependencies();
            services.AddDomainServices();

            services.Configure<SeedDataFilesConfigurations>(
                Configuration.GetSection(ConfigurationConstants.SeedDataFileConfigName));

            services.AddSingleton<IEnvironment, AppEnvironment>();
            var mailSettings = Configuration.GetSection(ConfigurationConstants.MailSettingsConfigName).Get<MailSettings>();
            services.AddSingleton(mailSettings);
            var appSettings = Configuration.GetSection(ConfigurationConstants.AppSettingsConfigName).Get<AppSettings>();
            services.AddSingleton(appSettings);
        
            var builder = 
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<UnicefEduDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<Role>>()
                .AddSignInManager<SignInManager<User>>();


            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequireDigit = true;
                option.Password.RequiredLength = 8;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;

                // Lockout settings
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                option.Lockout.MaxFailedAccessAttempts = 5;
                option.Lockout.AllowedForNewUsers = true;
                // User settings

                
            });

            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(x =>
              {
                  x.Events = new JwtBearerEvents
                  {
                      OnTokenValidated = context =>
                      {                          
                          var currentUser = context.HttpContext.RequestServices.GetRequiredService<ICurrentLoginUserService>();
                          currentUser.SetClaims(context.Principal.Claims);
                          return Task.CompletedTask;
                      }
                  };
                  x.RequireHttpsMetadata = false;
                  x.SaveToken = true;
                  x.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(key),
                      ValidateIssuer = false,
                      ValidateAudience = false
                  };
              });
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = context =>
                {
                    var errMsg = context.ModelState
                        .First(x => x.Value.Errors.Count > 0).Value.Errors.First().ErrorMessage;
                    return new BadRequestObjectResult(new { message = errMsg });
                };
            });
            services.AddAuthorizationPolicyEvaluator();
            services.AddAuthorization(options =>
            {
                var permissions = AppPermissions.All();
                permissions.AddRange(HiddenPermissions.All());
                permissions
                .ForEach(claim =>
                {
                    options.AddPolicy(claim, policy =>
                    {                        
                        policy.RequireClaim(UnicefClaimTypes.Permission, claim);
                        policy.AuthenticationSchemes = new List<string>() { "Bearer" };
                    });
                });
            });

            services.AddScoped<IAuthorizationHandler, CustomAuthorizationHandler>();

            services.AddTransient<DatabseSeeder>();
            services.AddTransient<FrameworkSeeder>();
            services.AddTransient<CampCoordinateSeeder>();
            services.AddTransient<FacilityIndicatorSeeder>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Unicef Educational MIS API", Version = "v1" });
                c.IncludeXmlComments(Path.Combine($"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            }, new List<string>()
                        }
                    });
            });

            services.AddAutoMapper(typeof(UnicefEduDbContext));
            services
               .AddCors(options =>
               {
                   options.AddDefaultPolicy(policyBuilder =>
                   {
                       policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
                   });
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /*
                public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabseSeeder seeder, 
                    ILoggerFactory loggerFactory, DbLoggerProvider dbLoggerProvider)
        */

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabseSeeder seeder, 
            ILogger<Startup> logger, CampCoordinateSeeder campCoordinateSeeder,FrameworkSeeder frameworkSeeder,FacilityIndicatorSeeder facilityIndicatorSeeder
            )
        {
            ServiceContainer = app.ApplicationServices.CreateScope().ServiceProvider;
            seeder.EnsureDatabaseExists(app);
            seeder.Seed();
            frameworkSeeder.Seed();
            facilityIndicatorSeeder.SeedFacilityIndicator();

            var updateCampCoordinatesCheck = bool.Parse(Configuration.GetSection("AppSettings:UpdateCampCoordinates").Value);
            campCoordinateSeeder.SeedCampCoordinate(updateCampCoordinatesCheck);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsProduction())
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UEM V1");                
            });

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });  

            logger.LogInformation("Startup configuration completed");
        }
    }
}

