using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories.Framework;

namespace UnicefEducationMIS.Data.Repositories.Framework
{
    public class ObjectiveIndicatorRepository: BaseRepository<ObjectiveIndicator, long>, IObjectiveIndicatorRepository
    {
        public ObjectiveIndicatorRepository(UnicefEduDbContext context) : base(context)
        {
        }
    }
}