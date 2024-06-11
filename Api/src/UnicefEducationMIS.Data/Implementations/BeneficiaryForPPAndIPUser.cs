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
    public class BeneficiaryForPPAndIPUser : IBeneficiaryUserCondition
    {
        private readonly UnicefEduDbContext _context;
        private readonly ICurrentLoginUserService _currentLoginUserService;
        public BeneficiaryForPPAndIPUser(UnicefEduDbContext context, ICurrentLoginUserService currentLoginUserService)
        {
            _context = context;
            _currentLoginUserService = currentLoginUserService;
        }

        public IQueryable<BeneficiaryRawViewModel> ApplyCondition(IQueryable<BeneficiaryRawViewModel> source, long instanceId)
        {
            var facilityIds = _context.FacilityView.Where(
                         a => a.InstanceId == instanceId &&
                         (
                         a.ProgramPartnerId == _currentLoginUserService.EducationSectorPartner ||
                         a.ImplementationPartnerId == _currentLoginUserService.EducationSectorPartner
                         )
                     ).Select(a => a.Id).ToList();
            return source.Where(a => facilityIds.Contains(a.FacilityId));

        }

        public IQueryable<BeneficairyObjectViewModel> ApplyCondition(IQueryable<BeneficairyObjectViewModel> source, long instanceId)
        {
            var facilityIds = _context.FacilityView.Where(
                a => a.InstanceId == instanceId &&
                 (
                 a.ProgramPartnerId == _currentLoginUserService.EducationSectorPartner ||
                 a.ImplementationPartnerId == _currentLoginUserService.EducationSectorPartner
                 )

                ).Select(a => a.Id).ToList();
            return source.Where(a => facilityIds.Contains(a.FacilityId));

        }

        public IQueryable<BeneficiaryView> ApplyCondition(IQueryable<BeneficiaryView> source, long instanceId)
        {
            var facilityIds = _context.FacilityView.Where(
         a => a.InstanceId == instanceId &&
             (
             a.ProgramPartnerId == _currentLoginUserService.EducationSectorPartner ||
             a.ImplementationPartnerId == _currentLoginUserService.EducationSectorPartner
             )
                                             ).Select(a => a.Id).ToList();
            return source.Where(a => facilityIds.Contains(a.FacilityId));

        }

        public async Task<List<BeneficiaryRawViewModel>> ApplyCondition(List<BeneficiaryRawViewModel> source, long instanceId)
        {
            var facilityData = await _context.FacilityDynamicCells.Where(a => a.InstanceId == instanceId &&
            (a.EntityDynamicColumnId == FacilityFixedIndicators.ProgramPartner || a.EntityDynamicColumnId == FacilityFixedIndicators.ImplementationPartner))
                .Select(a =>new { a.FacilityId, a.EntityDynamicColumnId, a.Value }).Distinct()
                .ToListAsync();
            var groupData = facilityData.GroupBy(a => a.FacilityId).Select(a => new {
                FacilityId=a.Key,
                ProgrammingPartnerId = a.Where(a => a.EntityDynamicColumnId == FacilityFixedIndicators.ProgramPartner).Select(b => Convert.ToInt32(b.Value)).FirstOrDefault(),
                ImplementationPartnerId = a.Where(a => a.EntityDynamicColumnId == FacilityFixedIndicators.ImplementationPartner).Select(b => Convert.ToInt32(b.Value)).FirstOrDefault()
            }).ToList();
            var facilityIds = groupData.Where(a => a.ProgrammingPartnerId == _currentLoginUserService.EducationSectorPartner ||
             a.ImplementationPartnerId == _currentLoginUserService.EducationSectorPartner).Select(a => a.FacilityId).ToList();

            return source.Where(a => facilityIds.Contains(a.FacilityId)).ToList();
        }
    }
}
