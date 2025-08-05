using MediatR;
using Microsoft.AspNetCore.Mvc;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features;

namespace TourBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MobileController : BaseController
    {
        private readonly IMediator _mediator;

        public MobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("tourTypes")]
        public async Task<IActionResult> TourTypes()
        {
            var tourTypes = await _mediator.Send(new MobileTourTypesQuery());
            return Ok(ApiResponse<MobileTourTypesQueryResponse>.SuccessResponse(tourTypes, null));
        }

        [HttpGet("highlightedTourPoints")]
        public async Task<IActionResult> HighlightedTourPoints()
        {
            var tourPoints = await _mediator.Send(new MobileHighlightedTourPointQuery());
            return Ok(
                ApiResponse<MobileHighlightedTourPointQueryResponse>.SuccessResponse(
                    tourPoints,
                    null
                )
            );
        }
        [HttpGet("tour-points-by-tour-type")]
        public async Task<IActionResult> TourPointByTourTypeId([FromQuery]Guid request)
        {
            var tourPoints = await _mediator.Send(new MobileTourPointByTourTypeQuery(){CagetoryId=request});
            return Ok(                                
                ApiResponse<MobileTourPointByTourTypeQueryResponse>.SuccessResponse(
                    tourPoints,
                    null
                )
            );
        }
    }
}
