using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ViewModel.Dashboard;
using UnicefEducationMIS.Core.ViewModel;
using AutoMapper;

namespace UnicefEducationMIS.Web.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;
        private readonly IJrpPropertiesInfoService _jrpPropertiesInfoService;
        private readonly IMapper _mapper;
        private readonly IEmbeddedDashboardService _embeddedDashboardService;

        public DashboardController(IDashboardService dashboardService, IJrpPropertiesInfoService jrpPropertiesInfoService,
            IMapper mapper, IEmbeddedDashboardService embeddedDashboardService)
        {
            _dashboardService = dashboardService;
            _jrpPropertiesInfoService = jrpPropertiesInfoService;
            _mapper = mapper;
            _embeddedDashboardService = embeddedDashboardService;
        }


        [Authorize(Policy = AppPermissions.GetPredefinedDashboardTotals)]
        [HttpGet("GetTotalCounts")]
        public async Task<TotalCountsViewModel> GetTotalCounts()
        {
            return await _dashboardService.GetTotalCounts();
        }

        [Authorize(Policy = AppPermissions.GetGapMaps)]
        [HttpPost("GetGapMaps")]
        public async Task<List<GapMapViewModel>> GetGapMaps([FromBody] GapMapQueryModel query)
        {
            return await _dashboardService.GetGapMaps(query);
        }

        [Authorize(Policy = AppPermissions.GetLcMaps)]
        [HttpPost("GetLcMaps")]
        public async Task<List<LcMapViewModel>> GetLcCoordindates([FromBody] LcCoordinateQueryModel model)
        {
            return await _dashboardService.GetLcCoordinates(model);
        }

        [Authorize(Policy = AppPermissions.AddJrpParameter)]
        [HttpPost("AddJrpParameter")]
        public async Task<ActionResult> AddJrpParameter([FromBody] JrpParameterInfoViewModel model)
        {
             await _jrpPropertiesInfoService.Add(model);
            return Ok();
        }
        [Authorize(Policy = AppPermissions.UpdateJrpParameter)]
        [HttpPost("UpdateJrpParameter")]
        public async Task<ActionResult> UpdateJrpParameter([FromBody] JrpParameterInfoViewModel model)
        {
            await _jrpPropertiesInfoService.Update(model);
            return Ok();
        }
        [Authorize(Policy = AppPermissions.DeleteJrpParameter)]
        [HttpPost("DeleteJrpParameter/{id}")]
        public async Task<ActionResult> DeleteJrpParameter([FromRoute] int id)
        {
            await _jrpPropertiesInfoService.Delete(id);
            return Ok();
        }
        [Authorize(Policy = AppPermissions.GetJrpParameter)]
        [HttpGet("GetJrpParameter")]
        public async Task<ActionResult> GetJrpParameter([FromQuery] BaseQueryModel model)
        {
            var data=await _jrpPropertiesInfoService.Get(model);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.GetJrpChart)]
        [HttpPost("GetJrpChart")]
        public async Task<JrpChartViewModel> GetJrpChart([FromBody] JrpDashboardFilterViewModel model)
        {
            var filterModel = _mapper.Map<JrpPropertiesWithFilterViewModel>(model);
            return await _jrpPropertiesInfoService.GetJrpChartData(filterModel);
        }

        [Authorize(Policy = AppPermissions.AddEmbeddedDashboard)]
        [HttpPost("AddEmbeddedDashboard")]
        public async Task<ActionResult> AddEmbeddedDashboard([FromBody] EmbeddedDashboardViewModel model)
        {
            await _embeddedDashboardService.Save(model);
            return Ok();
        }
        [Authorize(Policy = AppPermissions.UpdateEmbeddedDashboard)]
        [HttpPost("UpdateEmbeddedDashboard")]
        public async Task<ActionResult> EmbeddedDashboard([FromBody] EmbeddedDashboardViewModel model)
        {
            await _embeddedDashboardService.Update(model);
            return Ok();
        }
        [Authorize(Policy = AppPermissions.DeleteEmbeddedDashboard)]
        [HttpPost("DeleteEmbeddedDashboard/{id}")]
        public async Task<ActionResult> DeleteEmbeddedDashboard([FromRoute] int id)
        {
            await _embeddedDashboardService.Delete(id);
            return Ok();
        }
        [Authorize(Policy = AppPermissions.GetEmbeddedDashboard)]
        [HttpGet("GetEmbeddedDashboard")]
        public async Task<ActionResult> GetEmbeddedDashboard()
        {
            var data = await _embeddedDashboardService.GetAll();
            return Ok(data);
        }

    }

}
