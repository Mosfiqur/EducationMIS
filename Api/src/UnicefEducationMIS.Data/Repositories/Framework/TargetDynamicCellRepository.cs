using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories.Framework;

namespace UnicefEducationMIS.Data.Repositories.Framework
{
    public class TargetDynamicCellRepository : BaseRepository<TargetFrameworkDynamicCell, long>, ITargetDynamicCellRepository
    {
        public TargetDynamicCellRepository(UnicefEduDbContext context) : base(context)
        {
        }
    }

    
}
