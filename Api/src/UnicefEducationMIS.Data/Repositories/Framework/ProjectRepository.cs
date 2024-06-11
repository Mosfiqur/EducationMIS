using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.Repositories.Framework;

namespace UnicefEducationMIS.Data.Repositories.Framework
{
    public class ProjectRepository : BaseRepository<Project, int>, IProjectRepository
    {
        public ProjectRepository(UnicefEduDbContext context) : base(context)
        {
        }
    }
}