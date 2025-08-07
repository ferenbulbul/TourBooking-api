using MediatR;
using Microsoft.AspNetCore.Mvc;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features;
using TourBooking.Application.Features.Mobile.Query.TourPointDetails;

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
        public async Task<IActionResult> TourPointByTourTypeId([FromQuery] Guid request)
        {
            var tourPoints = await _mediator.Send(
                new MobileTourPointByTourTypeQuery() { CagetoryId = request }
            );
            return Ok(
                ApiResponse<MobileTourPointByTourTypeQueryResponse>.SuccessResponse(
                    tourPoints,
                    null
                )
            );
        }

        [HttpGet("tour-points-by-query")]
        public async Task<IActionResult> TourPointBySearch([FromQuery] string query)
        {
            var tourPoints = await _mediator.Send(
                new MobileTourPointBySearchQuery() { Query = query }
            );
            return Ok(
                ApiResponse<MobileTourPointBySearchQueryResponse>.SuccessResponse(tourPoints, null)
            );
        }

        [HttpGet("regions")]
        public async Task<IActionResult> Regions()
        {
            var regions = await _mediator.Send(new MobileRegionQuery());
            return Ok(ApiResponse<MobileRegionQueryResponse>.SuccessResponse(regions, null));
        }

        [HttpGet("cities")]
        public async Task<IActionResult> Cities([FromQuery] Guid regionId)
        {
            var cities = await _mediator.Send(new MobileCityQuery { RegionId = regionId });
            return Ok(ApiResponse<MobileCityQueryResponse>.SuccessResponse(cities, null));
        }

        [HttpGet("districts")]
        public async Task<IActionResult> Districts([FromQuery] Guid cityId)
        {
            var districts = await _mediator.Send(new MobileDistrictQuery { CityId = cityId });
            return Ok(ApiResponse<MobileDistrictQueryResponse>.SuccessResponse(districts, null));
        }

        [HttpPost("detailed-search")]
        public async Task<IActionResult> DetailedSearch(MobileDetailedSearchQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(
                ApiResponse<MobileDetailedSearchQueryResponse>.SuccessResponse(response, null)
            );
        }

        [HttpGet("tour-point-details")]
        public async Task<IActionResult> TourPointDetails([FromQuery] Guid tourPointId)
        {
            var tourPoint = await _mediator.Send(
                new MobileTourPointDetailsQuery { TourPointId = tourPointId }
            );
            return Ok(
                ApiResponse<MobileTourPointDetailsQueryResponse>.SuccessResponse(tourPoint, null)
            );
        }
    }
}
