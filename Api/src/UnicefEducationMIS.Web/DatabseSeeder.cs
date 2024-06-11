using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Data;
using UnicefEducationMIS.Web.Configurations;
using UnicefEducationMIS.Web.Helpers;
using UnicefEducationMIS.Web.Seeders;

namespace UnicefEducationMIS.Web
{
    public class DatabseSeeder
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly SeedDataFilesConfigurations _fileConfig;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ICurrentLoginUserService _currentLoginUserService;        
        private readonly IUserLevelRepository _userLevelRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionPresetRepository _permissionPresetRepository;
        private readonly IEducationSectorPartnerRepository _educationSectorPartnerRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly ICampRepository _campRepository;
        private readonly IUpazilaRepository _upazilaRepository;
        private readonly IUnionRepository _unionRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly ISubBlockRepository _subBlockRepository;

        public DatabseSeeder(UserManager<User> userManager, RoleManager<Role> roleManager,
            IWebHostEnvironment hostEnv, IOptions<SeedDataFilesConfigurations> fileConfig,
            ICurrentLoginUserService currentUserService,            
            IUserLevelRepository userLevelRepository,
            IPermissionRepository permissionRepository,
            IPermissionPresetRepository permissionPresetRepository,
            IEducationSectorPartnerRepository educationSectorPartnerRepository,
            IDistrictRepository districtRepository,
            ICampRepository campRepository,
            IUpazilaRepository upazilaRepository,
            IUnionRepository unionRepository,
            IBlockRepository blockRepository,
            ISubBlockRepository subBlockRepository
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _hostEnvironment = hostEnv;
            _fileConfig = fileConfig.Value;

            _currentLoginUserService = currentUserService;
            _userLevelRepository = userLevelRepository;
            _permissionRepository = permissionRepository;
            _permissionPresetRepository = permissionPresetRepository;
            _educationSectorPartnerRepository = educationSectorPartnerRepository;

            _districtRepository = districtRepository;
            _campRepository = campRepository;
            _upazilaRepository = upazilaRepository;
            _unionRepository = unionRepository;
            _blockRepository = blockRepository;
            _subBlockRepository = subBlockRepository;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, 1.ToString())
            };

            _currentLoginUserService.SetClaims(claims);

        }

        public void Seed()
        {            
            SeedUserLevel();
            SeedPermissions();
            SeedPermissionPresets();
            SeedEducationSectorPartner();
            SeedRolesWithPermissions();
            SeedUsersWithRoles();
            SeedDistrict();
            SeedUpazila();
            SeedUnion();
            SeedCamp();
            SeedBlock();
            SeedSubBlock();
        }

        private List<T> ReadData<T>(string path)
        {
            List<T> result = new List<T>();
            using (var file = File.OpenText(GetFullPath(path)))
            {
                result.AddRange(JsonConvert.DeserializeObject<List<T>>(file.ReadToEnd()));
            }
            return result;
        }

        private string GetFullPath(string path)
        {
            return Path.Combine(_hostEnvironment.WebRootPath, _fileConfig.DataRoot, path);
        }

        private void SeedRolesWithPermissions()
        {
            var seedRoles = ReadData<SeedRole>(_fileConfig.RolesFilename);

            var existingRoles = _roleManager.Roles
                .Include(x => x.RolePermissions)
                .ToList();

            foreach (var seedRole in seedRoles)
            {

                var newPermissions = new List<RolePermission>();

                var existingRole = existingRoles.SingleOrDefault(x => x.Id == seedRole.Id);
                var newRelations = new List<int>();
                var existingRelations = new List<int>();
                if (existingRole != null)
                {
                    existingRelations.AddRange(existingRole.RolePermissions.Select(x => x.PermissionId).ToList());

                    newRelations.AddRange(
                        seedRole.Permissions.Except(existingRelations).ToList()
                         );
                    newRelations.ForEach(id =>
                    {
                        existingRole.RolePermissions.Add(new RolePermission { PermissionId = id, RoleId = existingRole.Id });
                    });
                    _roleManager.UpdateAsync(existingRole).Wait();
                }
                else
                {
                    newRelations.AddRange(
                        seedRole.Permissions.Except(existingRelations).ToList()
                         );

                    var role = new Role()
                    {
                        Name = seedRole.Name,
                        Id = seedRole.Id,
                        LevelId = seedRole.LevelId,
                        PermissionPresetId = seedRole.PermissionPresetId,
                        RolePermissions = newRelations.Select(x => new RolePermission { RoleId = seedRole.Id, PermissionId = x }).ToList()
                    };
                    _roleManager.CreateAsync(role).Wait();
                }
            }

        }
        private void SeedUsersWithRoles()
        {
            var seedUsers = ReadData<UserViewModel>(_fileConfig.UsersFilename);

            seedUsers.ToList().ForEach(seedUser =>
            {


                List<UserEduSectorPartner> userEduSectorPartnerList = null;
                if (seedUser.EduSectorPartners != null)
                {
                    userEduSectorPartnerList = seedUser.EduSectorPartners?.Select(x => new UserEduSectorPartner
                    {
                        PartnerId = x.Id,
                        PartnerType = x.PartnerType
                    }).ToList();
                }

                var user = new User
                {
                    UserName = seedUser.Email,
                    FullName = seedUser.FullName,
                    Email = seedUser.Email,
                    DesignationName = seedUser.DesignationName,
                    PhoneNumber = seedUser.PhoneNumber,
                    UserEduSectorPartners = userEduSectorPartnerList
                };

                var result = _userManager.CreateAsync(user, SystemDefaults.DefaultPassword).Result;
                if (result.Succeeded)
                {
                    var role = _roleManager.Roles.SingleOrDefault(x => x.Id == seedUser.RoleId);
                    _userManager.AddToRoleAsync(user, role.Name).Wait();
                }
            });
        }
      
        private void SeedUserLevel()
        {
            var seedUserLevels = ReadData<UserLevel>(_fileConfig.UserLevelsFilename);
            var all = _userLevelRepository.GetAll().ToList();
            var newIds = seedUserLevels.Select(x => x.Id)
                .Except(all.Select(x => x.Id)).ToList();

            _userLevelRepository.InsertRange(seedUserLevels.Where(x => newIds.Contains(x.Id))).Wait();
        }
        private void SeedPermissions()
        {
            var seedPermissions = ReadData<Permission>(_fileConfig.PermissionsFilename);

            var all = _permissionRepository.GetAll().ToList();
            var newIds = seedPermissions.Select(x => x.Id)
                .Except(all.Select(x => x.Id)).ToList();

            _permissionRepository.InsertRange(seedPermissions.Where(x => newIds.Contains(x.Id)));

            seedPermissions.ForEach(seed =>
            {
                var p = all.FirstOrDefault(x => x.Id == seed.Id);
                if (p != null)
                {
                    p.Description = seed.Description;
                    _permissionRepository.Update(p);
                }
            });

        }

        private void SeedPermissionPresets()
        {
            var seedPresets = ReadData<SeedPermissionPreset>(_fileConfig.PermissionPresetsFilename);


            var existingPresets = _permissionPresetRepository.GetAllWithChangeTracking()
                .Include(x => x.PermissionPresetPermissions)
                .ToList();


            var inserts = new List<PermissionPreset>();
            var updates = new List<PermissionPreset>();

            foreach (var item in seedPresets)
            {
                var presetPermissions = new List<PermissionPresetPermission>();

                List<int> existingRelations = new List<int>();

                var existingPreset = existingPresets.SingleOrDefault(x => item.Id == x.Id);
                if (existingPreset != null)
                {
                    existingRelations.AddRange(
                        existingPreset
                        .PermissionPresetPermissions
                        .Select(x => x.PermissionId).ToList()
                    );

                    var newRelations = item.Permissions.Except(existingRelations).ToList();

                    newRelations.ForEach(id =>
                    {
                        presetPermissions.Add(new PermissionPresetPermission
                        {
                            PermissionId = id,
                            PermissionPresetId = item.Id
                        });
                    });

                    newRelations.ForEach(id =>
                    {
                        existingPreset.PermissionPresetPermissions
                        .Add(new PermissionPresetPermission
                        {
                            PermissionId = id,
                            PermissionPresetId = existingPreset.Id
                        });
                    });
                    
                    _permissionPresetRepository.Update(existingPreset).Wait();
                }
                else
                {
                    var newRelations = item.Permissions.Except(existingRelations).ToList();

                    newRelations.ForEach(id =>
                    {
                        presetPermissions.Add(new PermissionPresetPermission { PermissionId = id, PermissionPresetId = item.Id });
                    });

                    //TODO: Add code to remove permissions

                    var p = new PermissionPreset
                    {
                        Id = item.Id,
                        PresetName = item.PresetName,
                        PermissionPresetPermissions = presetPermissions
                    };
                    inserts.Add(p);
                }
            }
            _permissionPresetRepository.InsertRange(inserts).Wait();
        }

        private void SeedEducationSectorPartner()
        {
            var seedEducationSectorPartner = ReadData<EducationSectorPartner>(_fileConfig.EducationSectorPartnerFilename);
            var all = _educationSectorPartnerRepository.GetAll().ToList();
            var newIds = seedEducationSectorPartner.Select(x => x.Id)
                .Except(all.Select(x => x.Id)).ToList();

            _educationSectorPartnerRepository.InsertRange(seedEducationSectorPartner.Where(x => newIds.Contains(x.Id))).Wait();
        }

        private void SeedDistrict()
        {
            var seedDistricts = ReadData<District>(_fileConfig.DistrictFileName);
            var all = _districtRepository.GetAll().ToList();
            var newIds = seedDistricts.Select(x => x.Id).Except(all.Select(x => x.Id))
                .ToList();
            _districtRepository.InsertRange(seedDistricts.Where(x => newIds.Contains(x.Id)).ToList()).Wait();
        }
        private void SeedCamp()
        {
            var seedCamps = ReadData<Camp>(_fileConfig.CampFileName);
            var all = _campRepository.GetAll().ToList();
            var newIds = seedCamps.Select(x => x.Id).Except(all.Select(x => x.Id))
                .ToList();
            _campRepository.InsertRange(seedCamps.Where(x => newIds.Contains(x.Id)).ToList()).Wait();
        }
        private void SeedUpazila()
        {
            var seedUpazila = ReadData<Upazila>(_fileConfig.UpazilaFileName);
            var all = _upazilaRepository.GetAll().ToList();
            var newIds = seedUpazila.Select(x => x.Id).Except(all.Select(x => x.Id))
                .ToList();
            _upazilaRepository.InsertRange(seedUpazila.Where(x => newIds.Contains(x.Id)).ToList()).Wait();
        }
        private void SeedBlock()
        {
            var seedBlock = ReadData<Block>(_fileConfig.BlockFileName);
            var all = _blockRepository.GetAll().ToList();
            var newIds = seedBlock.Select(x => x.Id).Except(all.Select(x => x.Id))
                .ToList();
            _blockRepository.InsertRange(seedBlock.Where(x => newIds.Contains(x.Id)).ToList()).Wait();
        }
        private void SeedSubBlock()
        {
            var seedSubBlocks = ReadData<SubBlock>(_fileConfig.SubBlockFileName);
            var all = _subBlockRepository.GetAll().ToList();
            var newIds = seedSubBlocks.Select(x => x.Id).Except(all.Select(x => x.Id))
                .ToList();
            _subBlockRepository.InsertRange(seedSubBlocks.Where(x => newIds.Contains(x.Id)).ToList()).Wait();
        }
        private void SeedUnion()
        {
            var seedUnions = ReadData<Union>(_fileConfig.UnionFileName);
            var all = _unionRepository.GetAll().ToList();
            var newIds = seedUnions.Select(x => x.Id).Except(all.Select(x => x.Id))
                .ToList();
            _unionRepository.InsertRange(seedUnions.Where(x => newIds.Contains(x.Id)).ToList()).Wait();
        }

        public void EnsureDatabaseExists(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            var context = serviceScope.ServiceProvider.GetService<UnicefEduDbContext>();
     
            context.Database.Migrate();
        }


    }
}
