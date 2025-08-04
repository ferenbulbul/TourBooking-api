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
