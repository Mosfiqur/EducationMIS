using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class DistrictRepository : BaseRepository<District,int>,IDistrictRepository
    {
        public DistrictRepository(UnicefEduDbContext context):base(context)
        {

        }
    }
}
