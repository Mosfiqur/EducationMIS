using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class UserLevelRepository : BaseRepository<UserLevel, byte> , IUserLevelRepository
    {
        public UserLevelRepository(UnicefEduDbContext context) : base(context)
        {
        }        
    }
}
