using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ViewModel;
using System.Linq;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Views;
using System.Threading.Tasks;

namespace UnicefEducationMIS.Data.Implementations
{
    public class BeneficiaryForAdminUser : IBeneficiaryUserCondition
    {
        private readonly UnicefEduDbContext _context;
        private readonly ICurrentLoginUserService _currentLoginUserService;
        public BeneficiaryForAdminUser(UnicefEduDbContext context, ICurrentLoginUserService currentLoginUserService)
        {
            _context = context;
            _currentLoginUserService = currentLoginUserService;
        }
        public IQueryable<BeneficiaryRawViewModel> ApplyCondition(IQueryable<BeneficiaryRawViewModel> source, long instanceId)
        {
            return source;
        }

        public IQueryable<BeneficairyObjectViewModel> ApplyCondition(IQueryable<BeneficairyObjectViewModel> source, long instanceId)
        {
            return source;
        }

        public IQueryable<BeneficiaryView> ApplyCondition(IQueryable<BeneficiaryView> source, long instanceId)
        {
            return source;
        }

        public Task<List<BeneficiaryRawViewModel>> ApplyCondition(List<BeneficiaryRawViewModel> source, long instanceId)
        {
              return Task.FromResult<List<BeneficiaryRawViewModel>>(
                source
            );
        }
    }
}
