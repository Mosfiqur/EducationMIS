using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.Implementations
{
    public class FacilityForAdminUser : IFacilityUserCondition
    {
        private readonly UnicefEduDbContext _context;
        private readonly ICurrentLoginUserService _currentLoginUserService;
        public FacilityForAdminUser(UnicefEduDbContext context, ICurrentLoginUserService currentLoginUserService)
        {
            _context = context;
            _currentLoginUserService = currentLoginUserService;
        }
        public IQueryable<FacilityRawViewModel> ApplyCondition(IQueryable<FacilityRawViewModel> source, long instanceId)
        {
            return source;
        }

        public IQueryable<Facility> ApplyCondition(IQueryable<Facility> source, long instanceId)
        {
            return source;
        }

        public IList<FacilityRawViewModel> ApplyCondition(IList<FacilityRawViewModel> source)
        {
            return source;
        }
    }
}
