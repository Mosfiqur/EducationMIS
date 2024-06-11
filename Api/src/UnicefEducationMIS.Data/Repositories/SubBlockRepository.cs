using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class SubBlockRepository : BaseRepository<SubBlock,int> , ISubBlockRepository
    {
        public SubBlockRepository(UnicefEduDbContext context): base(context)
        {

        }
    }
}
