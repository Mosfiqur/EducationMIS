using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Data.Repositories
{
  public  class EducationSectorPartnerRepository : BaseRepository<EducationSectorPartner, int>, IEducationSectorPartnerRepository
    {
        public EducationSectorPartnerRepository(UnicefEduDbContext context) : base(context)
        {
        }

        public Task<List<EducationSectorPartner>> GetEduSectorPartnersByUserId(int userId)
        {
            var data = 
            _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.UserEduSectorPartners)
                .ThenInclude(p => p.Partner)                
                .SelectMany(u => u.UserEduSectorPartners.Select(x => new EducationSectorPartner
                {
                    Id = x.PartnerId,
                    PartnerName = x.Partner.PartnerName,
                    PartnerType = x.PartnerType
                }))
                .ToListAsync();
            return data;
        }
    }
}
