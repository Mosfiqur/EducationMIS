using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IPermissionRepository : IBaseRepository<Permission, int>
    {
        Task<List<string>> GetPermissionsByUserId(int id);
        Task<IEnumerable<Permission>> GetPermissionsByPresetId(int id);
    }
}
