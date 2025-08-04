using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features;

namespace TourBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgencyController : BaseController
    {
        private readonly IMediator _mediator;

        public AgencyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("driver")]
        public async Task<IActionResult> Drivers()
        {
            var userIdString = GetUserIdFromToken();
            var drivers = await _mediator.Send(new DriverQuery { AgencyId = userIdString });

            if (drivers == null || !drivers.Drivers.Any())
            {
                return Ok(ApiResponse<DriverQueryResponse>.SuccessResponse(null, null));
            }

            return Ok(ApiResponse<DriverQueryResponse>.SuccessResponse(drivers, null));
        }

        [Authorize]
        [HttpPost("driver")]
        public async Task<IActionResult> CreateDriver(UpsertDriverCommand request)
        {
            var userIdString = GetUserIdFromToken();

            request.AgencyId = userIdString;
            await _mediator.Send(request);
            return Ok(ApiResponse<UpsertDriverCommandResponse>.SuccessResponse(null, null));
        }

        [HttpGet("vehicle")]
        public async Task<IActionResult> Vehicles()
        {
            var vehicles = await _mediator.Send(new VehicleQuery());
            return Ok(ApiResponse<VehicleQueryResponse>.SuccessResponse(vehicles, null));
        }

        [Authorize]
        [HttpPost("vehicle")]
        public async Task<IActionResult> CreateVehicle(UpsertVehicleCommand request)
        {
            var userIdString = GetUserIdFromToken();

            request.AgencyId = userIdString;

            await _mediator.Send(request);
            return Ok(ApiResponse<UpsertVehicleCommandResponse>.SuccessResponse(null, null));
        }
    }
}
