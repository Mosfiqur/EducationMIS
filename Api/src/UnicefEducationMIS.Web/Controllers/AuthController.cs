using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Exceptions;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.Settings;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Service.Helpers;

namespace UnicefEducationMIS.Web.Controllers
{
    public class AuthController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IEducationSectorPartnerRepository _eduSectorPartnerRepository;
        private readonly ILogger<AuthController> _logger;
        private readonly MailHelper _mailHelper;
        private readonly IEnvironment _env;
        private readonly AppSettings _appSettings;

        private readonly IPermissionRepository _permissionRepository;
        private readonly ICurrentLoginUserService _currentLoginUser;
        public AuthController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IUserService userService,
            IConfiguration configuration,
            IEducationSectorPartnerRepository educationSectorPartnerRepository,
            ILogger<AuthController> logger,
            MailHelper mailHelper,
            IEnvironment env,
            AppSettings appSettings,
            IDataProtectionProvider dataProtectionProvider,
            IOptions<DataProtectionTokenProviderOptions> options, IPermissionRepository permissionRepository, ICurrentLoginUserService currentLoginUser)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _configuration = configuration;
            _eduSectorPartnerRepository = educationSectorPartnerRepository;
            _logger = logger;
            _mailHelper = mailHelper;
            _env = env;
            _appSettings = appSettings;
            _permissionRepository = permissionRepository;
            _currentLoginUser = currentLoginUser;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<LoginResponse> Login(LoginViewModel model)
        {
            _logger.LogInformation("User {Email} is login", model.Email);
            //  var user = _userManager.Users.SingleOrDefault(x => x.Email == model.Email.Trim() && !x.IsDeleted);

            var user = await _userManager.FindByEmailAsync(model.Email.Trim());

            if (user == null)
            {
                throw new AuthenticationFailedException("Username/Password do not match.");
            }


            //_userManager.SupportsUserLockout = true;

            var lockoutTime = user.LockoutEnd;
            if (lockoutTime!=null && 
                Convert.ToDateTime(lockoutTime.Value.ToString("yyyy/MM/dd HH:mm:ss tt")) 
                >= Convert.ToDateTime(DateTimeOffset.UtcNow.ToString("yyyy/MM/dd HH:mm:ss tt")))
            {
                var lockout= lockoutTime.Value.ToUniversalTime().ToString("dd-MMM-yyyy hh:mm:ss") + " "+
                             lockoutTime.Value.AddHours(lockoutTime.Value.Offset.Hours).ToString("tt");
                throw new DomainException($"User is locked till {lockout}");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);


            if (result.Succeeded)
            {
                return new LoginResponse
                {
                    Token = await GenerateToken(user),
                    StatusCode = AppStatusCode.SuccessStatusCode.ToString(),
                    Message = string.Empty,
                    UserProfile = await _userService.GetUserById(user.Id)
                };
            }

            if (result.IsLockedOut)
            {
                var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                throw new AuthenticationFailedException($"User is locked till {lockoutDate.Value.ToLocalTime()}");
            }
            throw new AuthenticationFailedException("Username/Password do not match.");
        }
        /// <summary>
        /// User can change his own password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Policy = AppPermissions.ChangeOwnPassword)]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(PasswordChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();
                throw new Core.Exceptions.DomainException(string.Join(',', errors));
            }
            await _userService.ChangePassword(model);
            return Ok();
        }


        private async Task<string> GenerateToken(User user)
        {
            //var userLevel = await _userLevelRepository.GetById(user.LevelId);
            var userRole = _roleManager.Roles
                .Where(x => x.UserRoles.Select(x => x.UserId).Contains(user.Id))
                .Include(x => x.Level)
                .SingleOrDefault();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, userRole.Name),
                new Claim(UnicefClaimTypes.UserRank, userRole.Level.Rank.ToString())
            };

            await AddEducationSectorPartnerClaims(user, claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task AddEducationSectorPartnerClaims(User user, List<Claim> claims)
        {
            var eduPartners = await _eduSectorPartnerRepository.GetEduSectorPartnersByUserId(user.Id);

            var esp = eduPartners
                    .SingleOrDefault(x => x.PartnerType == PartnerType.EducationSectorPartner);

            var pp = eduPartners
                    .SingleOrDefault(x => x.PartnerType == PartnerType.ProgramPartner);

            var ipList = eduPartners
                    .Where(x => x.PartnerType == PartnerType.ImplementationPartner)
                    .ToList();

            if (esp != null)
            {
                claims.Add(new Claim(UnicefClaimTypes.EducationSectorPartner, esp.Id.ToString()));
            }

            if (pp != null)
            {
                claims.Add(new Claim(UnicefClaimTypes.ProgramPartner, pp.Id.ToString()));
            }

            ipList.ForEach(x => claims.Add(new Claim(UnicefClaimTypes.ImplementationPartner, x.Id.ToString())));
        }


        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            var bytes = Convert.FromBase64String(model.Token);
            var decryped = Encoding.UTF8.GetString(bytes);

            var arr = decryped.Split(',');

            DateTime creationTime;
            DateTime.TryParse(arr[1], out creationTime);

            var user = await _userManager.FindByEmailAsync(arr[0]);
            if (user == null)
            {
                return BadRequest(Messages.TokenExpired);
            }

            var result = await IsValidToken(model.Token);

            if (!result.IsValid)
            {
                return BadRequest(Messages.TokenExpired);
            }

            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, model.NewPassword);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok();
            }

            var token = await GeneratePasswordResetToke(user);

            _mailHelper.SendEmail(model.Email, "Forgot your password? We can help.", GetResetPasswordEmailBody(user, token));
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("ValidatePasswordResetToken")]
        public async Task<ActionResult<Helpers.TokenValidationResult>> ValidatePasswordResetToken(string token)
        {
            return Ok(await IsValidToken(token));
        }

        [Authorize(Policy = AppPermissions.GetOwnPermissions)]
        [HttpGet("GetOwnPermissions")]
        public async Task<IEnumerable<string>> GetOwnPermissions()
        {
            return await _permissionRepository.GetPermissionsByUserId(_currentLoginUser.UserId);
        }


        private string GetResetPasswordEmailBody(User user, string token)
        {
            var path = Path.Combine(_env.GetRootPath(), FileNames.EMAIL_TEMPLATE_FOLDER, FileNames.PasswordResetEmailTemplate);

            var emailBody = System.IO.File.ReadAllText(path);
            var redirectionLink = $"{_appSettings.HostUrl}{_appSettings.PasswordResetUrl}{token}";
            return string.Format(emailBody, user.FullName, redirectionLink);
        }

        private async Task<Helpers.TokenValidationResult> IsValidToken(string token)
        {
            try
            {
                var bytes = Convert.FromBase64String(token);
                var decryped = Encoding.UTF8.GetString(bytes);

                var arr = decryped.Split(',');

                DateTime creationTime;
                DateTime.TryParse(arr[1], out creationTime);

                var expirationTime = creationTime.AddHours(_appSettings.PasswordResetTokenLifespan);
                if (DateTime.Now > expirationTime)
                {
                    return await Task.FromResult(Helpers.TokenValidationResult.InValidResult());
                }
                var user = await _userManager.FindByEmailAsync(arr[0]);

                if (user == null)
                {
                    return await Task.FromResult(Helpers.TokenValidationResult.InValidResult());
                }
                return await Task.FromResult(Helpers.TokenValidationResult.ValidResult());
            }
            catch (Exception)
            {
                return await Task.FromResult(Helpers.TokenValidationResult.InValidResult());
            }
        }

        private async Task<string> GeneratePasswordResetToke(User user)
        {
            var token = $"{user.Email},{DateTime.Now}";
            return await Task.FromResult(Convert.ToBase64String(Encoding.UTF8.GetBytes(token)));
        }
    }
}
