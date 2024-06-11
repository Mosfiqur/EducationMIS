using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class CampCoordinateRepository : BaseRepository<CampCoordinate,long> , ICampCoordinateRepository
    {
        public CampCoordinateRepository(UnicefEduDbContext context) : base(context)
        {

        }
    }
}
