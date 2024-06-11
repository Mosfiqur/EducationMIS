using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Data.Implementations;

namespace UnicefEducationMIS.Data.Factory
{
   public class FacilityUserConditionFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public FacilityUserConditionFactory(IServiceProvider serviceProvider)
        {

            _serviceProvider = serviceProvider;
        }

        public IFacilityUserCondition GetFacilityUserCondition()
        {
            ICurrentLoginUserService _currentLoginUserService
                = (ICurrentLoginUserService)_serviceProvider.GetService(typeof(ICurrentLoginUserService));

            if (_currentLoginUserService.IsAdmin)
                return new FacilityForAdminUser((UnicefEduDbContext)_serviceProvider.GetService(typeof(UnicefEduDbContext)), (ICurrentLoginUserService)_serviceProvider.GetService(typeof(ICurrentLoginUserService)));
            else if (_currentLoginUserService.IsTeacher)
                return new FacilityForTeacherUser((UnicefEduDbContext)_serviceProvider.GetService(typeof(UnicefEduDbContext)), (ICurrentLoginUserService)_serviceProvider.GetService(typeof(ICurrentLoginUserService)));
            else
                return new FacilityForPPandIPUser((UnicefEduDbContext)_serviceProvider.GetService(typeof(UnicefEduDbContext)), (ICurrentLoginUserService)_serviceProvider.GetService(typeof(ICurrentLoginUserService)));
        }
    }
}
