using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class UpazilaRepository : BaseRepository<Upazila,int> , IUpazilaRepository
    {
        public UpazilaRepository(UnicefEduDbContext context) : base(context)
        {

        }
    }
}
