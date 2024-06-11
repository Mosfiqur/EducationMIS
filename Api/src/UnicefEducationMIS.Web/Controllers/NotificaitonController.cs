using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;

namespace UnicefEducationMIS.Web.Controllers
{
    public class NotificaitonController : BaseController
    {
        public INotificationService _notificationService { get; }
        public NotificaitonController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll([FromQuery] BaseQueryModel model)
        {
            var data = await _notificationService.Get(model);
            return Ok(data);
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpPost("ReadNotification/{notificationId}")]
        public async Task<ActionResult> ReadNotification(long notificationId)
        {
            await _notificationService.ReadNotification(notificationId);
            return Ok();
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpPost("ClearNotification")]
        public async Task<ActionResult> ClearNotification()
        {
             await _notificationService.ClearNotification();
            return Ok();
        }
    }
}
