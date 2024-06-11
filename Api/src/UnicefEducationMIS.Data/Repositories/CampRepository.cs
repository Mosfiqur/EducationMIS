using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
    public class CampRepository : BaseRepository<Camp,int> , ICampRepository
    {
        public CampRepository(UnicefEduDbContext context): base(context)
        {

        }

        public Task<List<CampCoordinate>> GetCoordinatesByCampId(int campId)
        {
            return _context.Camps
            .Include(x => x.CampCoordinates)
                .Where(x => x.Id == campId)
                .SelectMany(
                    camp => camp.CampCoordinates.Select(coordinate => coordinate))
                .OrderBy(x => x.SequenceNumber)
                .ToListAsync();
        }

        public Task<List<CampCoordinate>> GetAllCoordinates()
        {
            return _context.Camps
                .Include(x => x.CampCoordinates)
                .SelectMany(
                    camp => camp.CampCoordinates.Select(coordinate => coordinate))
                .OrderBy(x => x.SequenceNumber)
                .ToListAsync();
        }
    }
}
