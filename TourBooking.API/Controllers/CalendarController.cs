using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.DTOs.GuideCalendar;
using TourBooking.Application.Features.Settings;
using TourBooking.Application.Features.Settings.Queries;
using TourBooking.Domain.Enums;
using TourBooking.Infrastructure.Context;
using TourBooking.Infrastructure.Services;

namespace TourBooking.API.Controllers
{
    [ApiController]
    [Route("api/guides/calendar")]
    public class CalendarController : BaseController
    {
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // aralık bazlı getir (ay görünümü için from-to zorunlu tut)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarEventDto2>>> Get(
            [FromQuery] DateOnly from,
            [FromQuery] DateOnly to
        )
        {
            var userIdString = GetUserIdFromToken();

            var events = await _mediator.Send(
                new FetchEventsQuery
                {
                    From = from,
                    GuidId = userIdString,
                    To = to
                }
            );
            return Ok(ApiResponse<FetchEventsQueryResponse>.SuccessResponse(events, null));
        }

        [HttpPost("blocks")]
        public async Task<IActionResult> CreateBlock(CreateBlockRequest req)
        {
            var userIdString = GetUserIdFromToken();

            await _mediator.Send(
                new CreateBlockCommand
                {
                    GuideId = userIdString,
                    End = req.end,
                    Note = req.note,
                    Start = req.start
                }
            );
            return Ok(ApiResponse<CreateBlockCommandResponse>.SuccessResponse(null, null));
        }

        [HttpDelete("blocks/{blockId:guid}")]
        public async Task<IActionResult> DeleteBlock(Guid blockId)
        {
            var userIdString = GetUserIdFromToken();
            await _mediator.Send(
                new RemoveBlockCommand { BlockId = blockId, GuideId = userIdString }
            );
            return Ok(ApiResponse<RemoveBlockCommandResponse>.SuccessResponse(null, null));
        }
    }
}
