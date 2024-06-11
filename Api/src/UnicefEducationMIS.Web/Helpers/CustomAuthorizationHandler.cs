using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Web.Helpers
{
    public class CustomAuthorizationHandler : AuthorizationHandler<ClaimsAuthorizationRequirement>, IAuthorizationHandler
    {
        private readonly IPermissionRepository _permissionRepo;
        private readonly ICurrentLoginUserService _userService;
        private readonly IEducationSectorPartnerRepository _educationSectorPartnerRepository;
        public CustomAuthorizationHandler(IPermissionRepository permissionRepo,
            ICurrentLoginUserService userService, IEducationSectorPartnerRepository educationSectorPartnerRepository)
        {
            _permissionRepo = permissionRepo;
            _userService = userService;
            _educationSectorPartnerRepository = educationSectorPartnerRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            ClaimsAuthorizationRequirement requirement)
       {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var id = context.User.GetUserId();
            var claims = await _permissionRepo.GetPermissionsByUserId(id);

            this._userService.SetClaims(claims.Select(x => new System.Security.Claims.Claim(UnicefClaimTypes.Permission, x)));
            _userService.Esps = await _educationSectorPartnerRepository.GetEduSectorPartnersByUserId(id);
            foreach (var item in requirement.AllowedValues)
            {
                if (item == HiddenPermissions.HaveToBeLoggedIn)
                {
                    context.Succeed(requirement);
                    return;
                }
                if (!claims.Contains(item))
                {
                    context.Fail();
                    return;
                }
            }
            context.Succeed(requirement);
        }
    }
}
