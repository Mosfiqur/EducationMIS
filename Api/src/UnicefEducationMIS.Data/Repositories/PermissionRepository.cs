using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class PermissionRepository : BaseRepository<Permission, int>, IPermissionRepository
    { 
        public PermissionRepository(UnicefEduDbContext context):base(context)
        {
            
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByPresetId(int id)
        {
            var data = await _context.PermissionPresets
                .Where(x => x.Id == id)
                .SelectMany(x => x.PermissionPresetPermissions)
                .Select(x => x.Permission)
                .OrderBy(x=> x.Description.Substring(0, 1))
                .ToListAsync();
            return data;
        }

        public Task<List<string>> GetPermissionsByUserId(int id)
        {
            var query = from ur in _context.UserRoles
                        join rp in _context.RolePermissions on ur.RoleId equals rp.RoleId
                        join p in _context.Permissions on rp.PermissionId equals p.Id
                        where ur.UserId == id
                        select p.PermissionName;
            return query.ToListAsync();
        }
    }
}
