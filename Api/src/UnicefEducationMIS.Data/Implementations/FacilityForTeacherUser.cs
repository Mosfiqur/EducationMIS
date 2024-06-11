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
    public class FacilityForTeacherUser : IFacilityUserCondition
    {
        private readonly UnicefEduDbContext _context;
        private readonly ICurrentLoginUserService _currentLoginUserService;
        public FacilityForTeacherUser(UnicefEduDbContext context, ICurrentLoginUserService currentLoginUserService)
        {
            _context = context;
            _currentLoginUserService = currentLoginUserService;
        }
        public IQueryable<FacilityRawViewModel> ApplyCondition(IQueryable<FacilityRawViewModel> source, long instanceId)
        {
            var facilityIds = _context.FacilityView.Where(
                 a => a.TeacherId == _currentLoginUserService.UserId && a.InstanceId==instanceId)
                .Select(a => a.Id).ToList();

            return source.Where(a => facilityIds.Contains(a.Id));
        }

        public IQueryable<Facility> ApplyCondition(IQueryable<Facility> source, long instanceId)
        {
            var facilityIds = _context.FacilityView.Where(
                 a => a.TeacherId == _currentLoginUserService.UserId && a.InstanceId == instanceId)
                .Select(a => a.Id).ToList();

            return source.Where(a => facilityIds.Contains(a.Id));
        }

        public IList<FacilityRawViewModel> ApplyCondition(IList<FacilityRawViewModel> source)
        {
            return source.Where(a => a.TeacherId == _currentLoginUserService.UserId).ToList();
        }
    }
}
