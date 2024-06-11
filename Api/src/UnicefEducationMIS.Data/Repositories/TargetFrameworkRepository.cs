using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class TargetFrameworkRepository : BaseRepository<TargetFramework, long> , ITargetFrameworkRepository
    {
        public TargetFrameworkRepository(UnicefEduDbContext context) : base(context)
        {
        }
    }
}
