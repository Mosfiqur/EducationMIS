using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories.Framework;

namespace UnicefEducationMIS.Data.Repositories.Framework
{
    public class AgeGroupRepository : BaseRepository<AgeGroup, int>, IAgeGroupRepository
    {
        public AgeGroupRepository(UnicefEduDbContext context) : base(context)
        {
        }
    }
}
