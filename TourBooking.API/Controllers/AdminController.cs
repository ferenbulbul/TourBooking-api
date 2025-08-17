using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features;
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

        [HttpGet("call-center-agents")]
        public async Task<IActionResult> CallCenterAgents()
        {
            var agencies = await _mediator.Send(new AgenciesToConfirmQuery());
            if (agencies == null || !agencies.Agencies.Any())
            {
                return Ok(ApiResponse<AgenciesToConfirmQueryResponse>.SuccessResponse(null, null));
            }
            return Ok(ApiResponse<AgenciesToConfirmQueryResponse>.SuccessResponse(agencies, null));
        }

        [HttpPost("upsert-call-center-agent")]
        public async Task<IActionResult> UpsertCallCenterAgent()
        {
            await _mediator.Send(new UpsertCallCenterAgentCommand());
            return Ok(
                ApiResponse<UpsertCallCenterAgentCommandResponse>.SuccessResponse(null, null)
            );
        }
    }
}
