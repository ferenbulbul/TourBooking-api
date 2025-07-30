using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
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

        [HttpGet("driver")]
        public async Task<IActionResult> Drivers()
        {
            var drivers = await _mediator.Send(new DriverQuery());
            return Ok(ApiResponse<DriverQueryResponse>.SuccessResponse(drivers, null));
        }

        [HttpPost("driver")]
        public async Task<IActionResult> CreateDriver(UpsertDriverCommand request)
        {
            await _mediator.Send(request);
            return Ok(ApiResponse<UpsertDriverCommandResponse>.SuccessResponse(null, null));
        }        
    }
}