using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class UserDynamicCellRepository : BaseRepository<UserDynamicCell, long>, IUserDynamicCellRepository
    {
        public UserDynamicCellRepository(UnicefEduDbContext context) : base(context)
        {
        }
    }
}
