using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Web.Controllers
{
    public class ListDataTypeController : BaseController
    {
        private readonly IListDataTypeService _columnListService;

        public ListDataTypeController(IListDataTypeService columnListService)
        {
            _columnListService = columnListService;
        }
        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] BaseQueryModel baseQueryModel)
        {
            var data = await _columnListService.GetAll(baseQueryModel);
            return Ok(data);
        }
        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetById")]
        public async Task<ActionResult<ListDataTypeViewModel>> GetById(long id)
        {            
            return Ok(await _columnListService.GetById(id));
        }
        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpPost("Add")]
        public async Task<ActionResult<ListDataType>> Add([FromBody] ListDataTypeCreateViewModel aListDataType)
        {            
            return Ok(await _columnListService.Add(aListDataType));
        }
        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] ListDataTypeUpdateViewModel aListDataType)
        {
            await _columnListService.Update(aListDataType);
            return Ok();
        }

    }
}