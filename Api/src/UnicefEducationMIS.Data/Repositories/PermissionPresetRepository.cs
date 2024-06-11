using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class PermissionPresetRepository : BaseRepository<PermissionPreset, int>, IPermissionPresetRepository
    {
        public PermissionPresetRepository(UnicefEduDbContext context) : base(context)
        {
        }


        public IQueryable<PermissionPreset> GetAllWithChangeTracking()
        {
            return _dbSet;
        }
    }
}
