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
        [HttpGet("tour")]
        public async Task<IActionResult> Tours()
        {
            var userIdString = GetUserIdFromToken();
            var tours = await _mediator.Send(new TourQuery { AgencyId = userIdString });
            if (tours == null || !tours.Tours.Any())
            {
                return Ok(ApiResponse<TourQueryResponse>.SuccessResponse(null, null));
            }
            return Ok(ApiResponse<TourQueryResponse>.SuccessResponse(tours, null));
        }

        [Authorize]
        [HttpPost("tour")]
        public async Task<IActionResult> CreateTour(UpsertTourCommand request)
        {
            var userIdString = GetUserIdFromToken();
            request.AgencyId = userIdString;
            await _mediator.Send(request);
            return Ok(ApiResponse<UpsertTourCommandResponse>.SuccessResponse(null, null));
        }

        [Authorize]
        [HttpGet("driver")]
        public async Task<IActionResult> Drivers()
        {
            var userIdString = GetUserIdFromToken();
            var drivers = await _mediator.Send(new DriverQuery { AgencyId = userIdString });

            if (drivers?.Drivers?.Any() != true)
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

        [Authorize]
        [HttpGet("vehicle")]
        public async Task<IActionResult> Vehicles()
        {
            var userIdString = GetUserIdFromToken();

            var vehicles = await _mediator.Send(new VehicleQuery { AgencyId = userIdString });
            if (vehicles == null || !vehicles.Vehicles.Any())
            {
                return Ok(ApiResponse<VehicleQueryResponse>.SuccessResponse(null, null));
            }
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
