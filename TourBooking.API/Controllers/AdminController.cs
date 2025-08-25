using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features;
using TourBooking.Application.Features.Admin;
using TourBooking.Application.Features.Admin.Command.ConfirmAgency;
using TourBooking.Application.Features.Admin.Command.ConfirmGuide;
using TourBooking.Application.Features.Admin.Query.AgenciesToConfirm;
using TourBooking.Application.Features.Settings.Commands;

namespace TourBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : BaseController
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("agencies-to-confirm")]
        public async Task<IActionResult> AgenciesToConfirm()
        {
            var agencies = await _mediator.Send(new AgenciesToConfirmQuery());
            if (agencies == null || !agencies.Agencies.Any())
            {
                return Ok(ApiResponse<AgenciesToConfirmQueryResponse>.SuccessResponse(null, null));
            }
            return Ok(ApiResponse<AgenciesToConfirmQueryResponse>.SuccessResponse(agencies, null));
        }

        [HttpGet("guides-to-confirm")]
        public async Task<IActionResult> GuidesToConfirm()
        {
            var guides = await _mediator.Send(new GuidesToConfirmQuery());
            if (guides == null || !guides.Guides.Any())
            {
                return Ok(ApiResponse<GuidesToConfirmQueryResponse>.SuccessResponse(null, null));
            }
            return Ok(ApiResponse<GuidesToConfirmQueryResponse>.SuccessResponse(guides, null));
        }

        [HttpPut("confirm-guide")]
        public async Task<IActionResult> ConfirmGuide([FromQuery] Guid id)
        {
            await _mediator.Send(new ConfirmGuideCommand { Id = id });
            return Ok(ApiResponse<ConfirmGuideCommandResponse>.SuccessResponse(null, null));
        }

        [HttpPut("confirm-agency")]
        public async Task<IActionResult> ConfirmAgency([FromQuery] Guid id)
        {
            await _mediator.Send(new ConfirmAgencyCommand { Id = id });
            return Ok(ApiResponse<ConfirmAgencyCommandResponse>.SuccessResponse(null, null));
        }

        [HttpGet("admin-management-user")]
        public async Task<IActionResult> AdminManagementUser()
        {
            var response = await _mediator.Send(new AdminManagementUserQuery());
            if (response == null || !response.Users.Any())
            {
                return Ok(
                    ApiResponse<AdminManagementUserQueryResponse>.SuccessResponse(null, null)
                );
            }
            return Ok(
                ApiResponse<AdminManagementUserQueryResponse>.SuccessResponse(response, null)
            );
        }

        [HttpPost("admin-management-user")]
        public async Task<IActionResult> UpsertAdminManagementUser(
            AdminManagementUserCommand request
        )
        {
            await _mediator.Send(request);
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
        }

        [HttpDelete("admin-management-user")]
        public async Task<IActionResult> DeleteAdminManagementUser([FromQuery] string userId)
        {
            await _mediator.Send(new DeleteAdminUserCommand { UserId = userId });
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
        }

        [Authorize]
        [HttpGet("driver-locations")]
        public async Task<IActionResult> DriverLocations()
        {
            var locations = await _mediator.Send(new DriverLocationsQuery());
            return Ok(ApiResponse<DriverLocationsQueryResponse>.SuccessResponse(locations, null));
        }
    }
}
