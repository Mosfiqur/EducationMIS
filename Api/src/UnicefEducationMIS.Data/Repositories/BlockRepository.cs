using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class BlockRepository : BaseRepository<Block,int> , IBlockRepository
    {
        public BlockRepository(UnicefEduDbContext context) : base(context)
        {

        }
    }
}
