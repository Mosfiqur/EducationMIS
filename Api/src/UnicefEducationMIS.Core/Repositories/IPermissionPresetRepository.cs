using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IPermissionPresetRepository : IBaseRepository<PermissionPreset, int>
    {
        IQueryable<PermissionPreset> GetAllWithChangeTracking();
    }
}
