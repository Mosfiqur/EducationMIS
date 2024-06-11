using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Views;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.Implementations
{
    public class BeneficiaryForTeacherUser : IBeneficiaryUserCondition
    {
        private readonly UnicefEduDbContext _context;
        private readonly ICurrentLoginUserService _currentLoginUserService;
        public BeneficiaryForTeacherUser(UnicefEduDbContext context, ICurrentLoginUserService currentLoginUserService)
        {
            _context = context;
            _currentLoginUserService = currentLoginUserService;
        }
        public IQueryable<BeneficiaryRawViewModel> ApplyCondition(IQueryable<BeneficiaryRawViewModel> source, long instanceId)
        {
            var facilityIds = _context.FacilityView.Where(
                a => a.TeacherId == _currentLoginUserService.UserId && a.InstanceId == instanceId)
                .Select(a => a.Id).ToList();

            return source.Where(a => facilityIds.Contains(a.FacilityId));
        }

        public IQueryable<BeneficairyObjectViewModel> ApplyCondition(IQueryable<BeneficairyObjectViewModel> source, long instanceId)
        {
            var facilityIds = _context.FacilityView.Where(
              a => a.TeacherId == _currentLoginUserService.UserId && a.InstanceId == instanceId)
                .Select(a => a.Id).ToList();

            return source.Where(a => facilityIds.Contains(a.FacilityId));

        }

        public IQueryable<BeneficiaryView> ApplyCondition(IQueryable<BeneficiaryView> source,long instanceId)
        {
            var facilityIds = _context.FacilityView.Where(
             a => a.TeacherId == _currentLoginUserService.UserId && a.InstanceId== instanceId)
                .Select(a => a.Id).ToList();

            return source.Where(a => facilityIds.Contains(a.FacilityId));

        }

        public async Task<List<BeneficiaryRawViewModel>> ApplyCondition(List<BeneficiaryRawViewModel> source, long instanceId)
        {
            var facilityData = await _context.FacilityDynamicCells.Where(a => a.InstanceId == instanceId &&
           (a.EntityDynamicColumnId == FacilityFixedIndicators.Teacher))
               .Select(a => new { a.FacilityId, a.EntityDynamicColumnId, a.Value }).Distinct()
               .ToListAsync();
            var groupData = facilityData.GroupBy(a => a.FacilityId).Select(a => new {
                FacilityId = a.Key,
                TeacherId = a.Where(a => a.EntityDynamicColumnId == FacilityFixedIndicators.Teacher).Select(b => Convert.ToInt32(b.Value)).FirstOrDefault()
                
            }).ToList();
            var facilityIds = groupData.Where(a => a.TeacherId == _currentLoginUserService.UserId).Select(a => a.FacilityId).ToList();

            return source.Where(a => facilityIds.Contains(a.FacilityId)).ToList();
        }
    }
}
