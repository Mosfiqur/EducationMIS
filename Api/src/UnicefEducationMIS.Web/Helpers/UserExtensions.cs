using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UnicefEducationMIS.Web.Helpers
{
    public static class UserExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var id = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("User id cannot be null");
            return int.Parse(id);
        }

        public static string GetUserRole(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
        }

        public static List<string> GetUserRoles(this ClaimsPrincipal user)
        {
            var roles = user.Claims.Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();            
            return roles;
        }
    }
}
