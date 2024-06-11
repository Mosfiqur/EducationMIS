using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories.Framework;

namespace UnicefEducationMIS.Data.Repositories.Framework
{
    public class DonorRepository : BaseRepository<Donor, int>, IDonorRepository
    {
        public DonorRepository(UnicefEduDbContext context) : base(context)
        {
        }
    }
}