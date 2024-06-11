using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class EmbeddedDashboardRepository : BaseRepository<EmbeddedDashboard, int>, IEmbeddedDashboardRepository
    {
        public EmbeddedDashboardRepository(UnicefEduDbContext context):base(context)
        {

        }
    }
}
