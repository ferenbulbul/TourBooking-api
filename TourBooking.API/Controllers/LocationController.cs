using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features;
using TourBooking.Application.Features.Settings.Queries;

namespace TourBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : BaseController
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region City
        [HttpGet("city")]
        public async Task<IActionResult> Cities()
        {
            var cities = await _mediator.Send(new CityQuery());
            return Ok(ApiResponse<CityQueryResponse>.SuccessResponse(cities, null));
        }

        [HttpPost("city")]
        public async Task<IActionResult> UpsertCity(UpsertCityCommand request)
        {
            await _mediator.Send(request);
            return Ok(ApiResponse<UpsertCityCommandResponse>.SuccessResponse(null, null));
        }
        #endregion

        #region Country
        [HttpGet("country")]
        public async Task<IActionResult> Countries()
        {
            var countries = await _mediator.Send(new CountryQuery());
            return Ok(ApiResponse<CountryQueryResponse>.SuccessResponse(countries, null));
        }

        [HttpPost("country")]
        public async Task<IActionResult> UpsertCountry(UpsertCountryCommand request)
        {
            await _mediator.Send(request);
            return Ok(ApiResponse<UpsertCountryCommandResponse>.SuccessResponse(null, null));
        }
        #endregion

        #region Region
        [HttpGet("region")]
        public async Task<IActionResult> Regions()
        {
            var regions = await _mediator.Send(new RegionQuery());
            return Ok(ApiResponse<RegionQueryResponse>.SuccessResponse(regions, null));
        }

        [HttpPost("region")]
        public async Task<IActionResult> UpsertRegion(UpsertRegionCommand request)
        {
            await _mediator.Send(request);
            return Ok(ApiResponse<UpsertRegionCommandResponse>.SuccessResponse(null, null));
        }
        #endregion

        #region District
        [HttpGet("district")]
        public async Task<IActionResult> Districts()
        {
            var regions = await _mediator.Send(new DistrictQuery());
            return Ok(ApiResponse<DistrictQueryResponse>.SuccessResponse(regions, null));
        }

        [HttpPost("district")]
        public async Task<IActionResult> UpsertDistict(UpsertDistrictCommand request)
        {
            await _mediator.Send(request);
            return Ok(ApiResponse<UpsertDistrictCommandResponse>.SuccessResponse(null, null));
        }
        #endregion
    }
}
