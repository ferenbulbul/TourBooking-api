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
    [Route("api/calendar")]
    public class CalendarController : BaseController
    {
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // aralık bazlı getir (ay görünümü için from-to zorunlu tut)
        [HttpGet("guide-blocks")]
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

        [HttpPost("guide-blocks")]
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

        [HttpDelete("guide-blocks/{blockId:guid}")]
        public async Task<IActionResult> DeleteBlock(Guid blockId)
        {
            var userIdString = GetUserIdFromToken();
            await _mediator.Send(
                new RemoveBlockCommand { BlockId = blockId, GuideId = userIdString }
            );
            return Ok(ApiResponse<RemoveBlockCommandResponse>.SuccessResponse(null, null));
        }

        [HttpGet("vehicle-calendar")]
        public async Task<ActionResult<IEnumerable<CalendarEventDto2>>> GetVehicleCalendar(
            [FromQuery] DateOnly from,
            [FromQuery] DateOnly to,
            [FromQuery] Guid vehicleId
        )
        {


            var events = await _mediator.Send(
                new FetchVehicleEventsQuery
                {
                    From = from,
                    VehicleId = vehicleId,
                    To = to
                }
            );
            return Ok(ApiResponse<FetchVehicleEventsQueryResponse>.SuccessResponse(events, null));
        }

        [HttpPost("vehicle-blocks")]
        public async Task<IActionResult> CreateVehicleBlock(CreateVehicleBlockRequest req)
        {

            await _mediator.Send(
                new CreateVehicleBlockCommand
                {
                    VehicleId = req.vehicleId,
                    End = req.end,
                    Note = req.note,
                    Start = req.start
                }
            );
            return Ok(ApiResponse<CreateVehicleBlockCommandResponse>.SuccessResponse(null, null));
        }

        [HttpDelete("vehicle-blocks")]
        public async Task<IActionResult> DeleteVehicleBlock([FromQuery] Guid blockId,[FromQuery] Guid vehicleId)
        {
            await _mediator.Send(
                new RemoveVehicleBlockCommand { BlockId = blockId, VehicleId = vehicleId }
            );
            return Ok(ApiResponse<RemoveVehicleBlockCommandResponse>.SuccessResponse(null, null));
        }
    }
}
