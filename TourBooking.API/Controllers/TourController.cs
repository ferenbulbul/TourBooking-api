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
    public class TourController : BaseController
    {
        private readonly IMediator _mediator;

        public TourController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("tour")]
        public async Task<IActionResult> Tours()
        {
            var tours = await _mediator.Send(new TourQuery());
            return Ok(ApiResponse<TourQueryResponse>.SuccessResponse(tours, null));
        }
        [HttpPost("tour")]
        public async Task<IActionResult> CreateTour(UpsertTourCommand request)
        {
            await _mediator.Send(request);
            return Ok(ApiResponse<UpsertTourCommandResponse>.SuccessResponse(null, null));
        }

        [HttpGet("tour-types")]
        public async Task<IActionResult> TourTypes()
        {
            var tourTypes = await _mediator.Send(new TourTypesQuery());
            return Ok(ApiResponse<TourTypesQueryResponse>.SuccessResponse(tourTypes, null));
        }
        [HttpPost("tour-types")]
        public async Task<IActionResult> CreateTourTypes(AddTourTypeCommand request)
        {
            await _mediator.Send(request);
            return Ok(ApiResponse<AddTourTypeCommandResponse>.SuccessResponse(null, null));
        }

        [HttpPost("tour-points")]
        public async Task<IActionResult> UpsertTourPoints(UpsertTourPointCommand request)
        {
            await _mediator.Send(request);
            return Ok(ApiResponse<UpsertTourPointCommandResponse>.SuccessResponse(null, null));
        }

        [HttpGet("tour-points")]
        public async Task<IActionResult> TourPoints()
        {
            var tourPoints = await _mediator.Send(new TourPointsQuery());
            return Ok(ApiResponse<TourPointsQueryResponse>.SuccessResponse(tourPoints, null));
        }
    }
}