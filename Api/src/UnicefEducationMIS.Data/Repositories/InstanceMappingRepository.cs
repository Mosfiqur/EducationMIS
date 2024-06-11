using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class InstanceMappingRepository : BaseRepository<InstanceMapping, int>, IInstanceMappingRepository
    {
        public InstanceMappingRepository(UnicefEduDbContext context):base(context)
        {

        }
    }
}
