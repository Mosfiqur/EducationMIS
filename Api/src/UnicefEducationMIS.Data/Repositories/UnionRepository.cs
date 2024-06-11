using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class UnionRepository : BaseRepository<Union,int> , IUnionRepository
    {
        public UnionRepository(UnicefEduDbContext context) : base(context)
        {

        }
    }
}
