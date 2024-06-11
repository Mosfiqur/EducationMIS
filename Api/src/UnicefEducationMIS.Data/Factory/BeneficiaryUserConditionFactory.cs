using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Data.Implementations;

namespace UnicefEducationMIS.Data.Factory
{
    public class BeneficiaryUserConditionFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public BeneficiaryUserConditionFactory(IServiceProvider serviceProvider)
        {

            _serviceProvider = serviceProvider;
        }

        public IBeneficiaryUserCondition GetBeneficiaryUserCondition()
        {
            ICurrentLoginUserService _currentLoginUserService
                = (ICurrentLoginUserService)_serviceProvider.GetService(typeof(ICurrentLoginUserService));

            if (_currentLoginUserService.IsAdmin)
                return new BeneficiaryForAdminUser((UnicefEduDbContext)_serviceProvider.GetService(typeof(UnicefEduDbContext)), (ICurrentLoginUserService)_serviceProvider.GetService(typeof(ICurrentLoginUserService)));
            else if (_currentLoginUserService.IsTeacher)
                return new BeneficiaryForTeacherUser((UnicefEduDbContext)_serviceProvider.GetService(typeof(UnicefEduDbContext)), (ICurrentLoginUserService)_serviceProvider.GetService(typeof(ICurrentLoginUserService)));
            else
                return new BeneficiaryForPPAndIPUser((UnicefEduDbContext)_serviceProvider.GetService(typeof(UnicefEduDbContext)), (ICurrentLoginUserService)_serviceProvider.GetService(typeof(ICurrentLoginUserService)));
        }
    }
}
