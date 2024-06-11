using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Web.Controllers
{
    public class EducationSectorPartnerController : BaseController
    {
        private readonly IEducationSectorPartnerService _eduSectorPartnerService;

        public EducationSectorPartnerController(IEducationSectorPartnerService eduSectorPartnerService)
        {
            _eduSectorPartnerService = eduSectorPartnerService;
        }


        [Authorize(Policy = AppPermissions.ViewEducationSectorPartners)]
        [HttpGet("GetAllEsp")]
        public async Task<IEnumerable<EduSectorPartnerViewModel>> GetAllEsp()
        {
            return await _eduSectorPartnerService.GetAll();
        }
    }
}
