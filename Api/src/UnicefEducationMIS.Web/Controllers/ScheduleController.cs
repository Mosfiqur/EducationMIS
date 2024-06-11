using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Web.Controllers
{
    public class ScheduleController : BaseController
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }
              

        [Authorize(Policy = AppPermissions.CreateBeneficiarySchedule)]
        [HttpPost("CreateBeneficiarySchedule")]
        public async Task<IActionResult> CreateBeneficiarySchedule([FromBody] ScheduleViewModel model)
        {
            await _scheduleService.CreateSchedule(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.CreateFacilitySchedule)]
        [HttpPost("CreateFacilitySchedule")]
        public async Task<IActionResult> CreateFacilitySchedule([FromBody] ScheduleViewModel model)
        {
            await _scheduleService.CreateSchedule(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.UpdateBeneficiarySchedule)]
        [HttpPut("UpdateBeneficiarySchedule")]
        public async Task<IActionResult> UpdateBeneficiarySchedule([FromBody] ScheduleUpdateViewModel model)
        {
            await _scheduleService.UpdateSchedule(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.UpdateFacilitySchedule)]
        [HttpPut("UpdateFacilitySchedule")]
        public async Task<IActionResult> UpdateFacilitySchedule([FromBody] ScheduleUpdateViewModel model)
        {
            await _scheduleService.UpdateSchedule(model);
            return Ok();
        }


        [Authorize(Policy = AppPermissions.UpdateBeneficiaryScheduleInstance)]
        [HttpPut("UpdateBeneficiaryScheduleInstance")]
        public async Task<IActionResult> UpdateBeneficiaryScheduleInstance([FromBody] InstanceUpdateViewModel model)
        {
            await _scheduleService.UpdateScheduleInstance(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.UpdateFacilityScheduleInstance)]
        [HttpPut("UpdateFacilityScheduleInstance")]
        public async Task<IActionResult> UpdateFacilityScheduleInstance([FromBody] InstanceUpdateViewModel model)
        {
            await _scheduleService.UpdateScheduleInstance(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.ViewBeneficiarySchedule)]
        [HttpGet("GetBeneficiaryCurrentSchedule")]
        public async Task<ScheduleViewModel> GetBeneficiaryCurrentSchedule(EntityType scheduleFor)
        {
            return await _scheduleService.GetCurrentSchedule(scheduleFor);            
        }

        [Authorize(Policy = AppPermissions.ViewFacilitySchedule)]
        [HttpGet("GetFacilityCurrentSchedule")]
        public async Task<ScheduleViewModel> GetFacilityCurrentSchedule(EntityType scheduleFor)
        {
            return await _scheduleService.GetCurrentSchedule(scheduleFor);
        }


        [Authorize(Policy = AppPermissions.ViewBeneficiarySchedule)]
        [HttpGet("GetBeneficiarySchedules")]
        public async Task<PagedResponse<ScheduleViewModel>> GetBeneficiarySchedules([FromQuery] ScheduleQueryModel model)
        {
            return await _scheduleService.GetSchedules(model);
        }

        [Authorize(Policy = AppPermissions.ViewFacilitySchedule)]
        [HttpGet("GetFacilitySchedules")]
        public async Task<PagedResponse<ScheduleViewModel>> GetFacilitySchedules([FromQuery] ScheduleQueryModel model)
        {
            return await _scheduleService.GetSchedules(model);
        }





        [Authorize(Policy = AppPermissions.DeleteBeneficiarySchedule)]
        [HttpDelete("DeleteBeneficiarySchedule")]
        public async Task<IActionResult> DeleteBeneficiarySchedule(long scheduleId)
        {
            await _scheduleService.DeleteSchedule(scheduleId);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.DeleteFacilitySchedule)]
        [HttpDelete("DeleteFacilitySchedule")]
        public async Task<IActionResult> DeleteFacilitySchedule(long scheduleId)
        {
            await _scheduleService.DeleteSchedule(scheduleId);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.StartBeneficiaryCollection)]
        [HttpPut("StartBeneficiaryCollection")]
        public async Task<IActionResult> StartBeneficiaryCollection([FromBody] StartCollectionViewModel model)
        {
            await _scheduleService.StartCollection(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.StartFacilityCollection)]
        [HttpPut("StartFacilityCollection")]
        public async Task<IActionResult> StartFacilityCollection([FromBody] StartCollectionViewModel model)
        {
            await _scheduleService.StartCollection(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.ViewBeneficiaryInstances)]
        [HttpGet("GetBeneficiaryCompletedInstances")]
        public async Task<PagedResponse<InstanceViewModel>> GetBeneficiaryCompletedInstances([FromQuery] BaseQueryModel model)
        {
            return await _scheduleService.GetCompletedInstances(model);            
        }

        [Authorize(Policy = AppPermissions.ViewFacilityInstances)]
        [HttpGet("GetFacilityCompletedInstances")]
        public async Task<PagedResponse<InstanceViewModel>> GetFacilityCompletedInstances([FromQuery] BaseQueryModel model)
        {
            return await _scheduleService.GetCompletedInstances(model);
        }

        [Authorize(Policy = HiddenPermissions.HaveToBeLoggedIn)]
        [HttpGet("GetRunningInstances")]
        public async Task<PagedResponse<InstanceViewModel>> GetRunningInstances([FromQuery] ScheduleInstanceQueryModel model)
        {
            return await _scheduleService.GetRunningInstances(model);
        }

        [Authorize(Policy = AppPermissions.ViewBeneficiaryInstances)]
        [HttpGet("GetBeneficiaryCurrentScheduleInstances")]
        public async Task<PagedResponse<InstanceViewModel>> GetBeneficiaryCurrentScheduleInstances([FromQuery] ScheduleInstanceQueryModel model)
        {
            return await _scheduleService.GetCurrentScheduleInstances(model);
        }

        [Authorize(Policy = AppPermissions.ViewFacilityInstances)]
        [HttpGet("GetFacilityCurrentScheduleInstances")]
        public async Task<PagedResponse<InstanceViewModel>> GetFacilityCurrentScheduleInstances([FromQuery] ScheduleInstanceQueryModel model)
        {
            return await _scheduleService.GetCurrentScheduleInstances(model);
        }

        [Authorize(Policy = AppPermissions.ViewBeneficiaryInstances)]
        [HttpGet("GetBeneficiaryNotPendingInstances")]
        public async Task<PagedResponse<InstanceViewModel>> GetBeneficiaryNotPendingInstances([FromQuery] ScheduleInstanceQueryModel model)
        {
            return await _scheduleService.GetNotPendingInstances(model);
        }

        [Authorize(Policy = AppPermissions.ViewFacilityInstances)]
        [HttpGet("GetFacilityNotPendingInstances")]
        public async Task<PagedResponse<InstanceViewModel>> GetFacilityNotPendingInstances([FromQuery] ScheduleInstanceQueryModel model)
        {
            return await _scheduleService.GetNotPendingInstances(model);
        }

        [Authorize(Policy = AppPermissions.ViewBeneficiaryInstances)]
        [HttpGet("GetBeneficiaryInstancesStatusWise")]
        public async Task<PagedResponse<InstanceViewModel>> GetBeneficiaryInstancesStatusWise([FromQuery] ScheduleInstanceStatusWiseQueryModel model)
        {
            return await _scheduleService.GetInstancesStatusWise(model);
        }

        [Authorize(Policy = AppPermissions.ViewFacilityInstances)]
        [HttpGet("GetFacilityInstancesStatusWise")]
        public async Task<PagedResponse<InstanceViewModel>> GetFacilityInstancesStatusWise([FromQuery] ScheduleInstanceStatusWiseQueryModel model)
        {
            return await _scheduleService.GetInstancesStatusWise(model);
        }

        [Authorize(Policy = AppPermissions.CompleteBeneficiaryInstance)]
        [HttpPost("CompleteBeneficiaryInstance/{instanceId}")]
        public async Task<IActionResult> CompleteBeneficiaryInstance([FromRoute] long instanceId)
        {
            await _scheduleService.CompleteInstance(instanceId);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.CompleteFacilityInstance)]
        [HttpPost("CompleteFacilityInstance/{instanceId}")]
        public async Task<IActionResult> CompleteFacilityInstance([FromRoute] long instanceId)
        {
            await _scheduleService.CompleteInstance(instanceId);
            return Ok();
        }
    }
}
