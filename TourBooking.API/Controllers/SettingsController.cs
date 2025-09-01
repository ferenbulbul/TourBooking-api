using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features;
using TourBooking.Application.Features.Settings.Commands;
using TourBooking.Application.Features.Settings.Queries;
using TourBooking.Domain.Entities;

namespace TourBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly SettingsOptions _opt;

        public SettingsController(IMediator mediator, IOptions<SettingsOptions> opt)
        {
            _mediator = mediator;
            _opt = opt.Value;
        }

        #region Vehicle Types

        [HttpGet("vehicle-types")]
        public async Task<IActionResult> VehicleTypes()
        {
            var vehicleTypes = await _mediator.Send(new VehicleTypesQuery());
            return Ok(ApiResponse<VehicleTypesQueryResponse>.SuccessResponse(vehicleTypes, null));
        }

        [HttpPost("vehicle-types")]
        public async Task<IActionResult> AddVehicleType(AddVehicleTypeCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
        }

        #endregion

        #region Vehicle Brands

        [HttpGet("vehicle-brands")]
        public async Task<IActionResult> VehicleBrands()
        {
            var vehicleBrands = await _mediator.Send(new VehicleBrandsQuery());
            return Ok(ApiResponse<VehicleBrandsQueryResponse>.SuccessResponse(vehicleBrands, null));
        }

        [HttpPost("vehicle-brands")]
        public async Task<IActionResult> AddVehicleBrand(AddVehicleBrandCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
        }
        #endregion

        #region Vehicle Class

        [HttpGet("vehicle-class")]
        public async Task<IActionResult> VehicleClasses()
        {
            var vehicleClasses = await _mediator.Send(new VehicleClassQuery());
            return Ok(ApiResponse<VehicleClassQueryResponse>.SuccessResponse(vehicleClasses, null));
        }

        [HttpPost("vehicle-class")]
        public async Task<IActionResult> UpsertVehicleClass(UpsertVehicleClassCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<UpsertVehicleClassCommandResponse>.SuccessResponse(null, null));
        }

        #endregion

        #region Seat Types

        [HttpGet("seat-type")]
        public async Task<IActionResult> SeatTypes()
        {
            var seatTypes = await _mediator.Send(new SeatTypeQuery());
            return Ok(ApiResponse<SeatTypeQueryResponse>.SuccessResponse(seatTypes, null));
        }

        [HttpPost("seat-type")]
        public async Task<IActionResult> UpsertSeatType(UpsertSeatTypeCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<UpsertCityCommandResponse>.SuccessResponse(null, null));
        }
        #endregion

        #region Legroom

        [HttpGet("legroom")]
        public async Task<IActionResult> Legrooms()
        {
            var legrooms = await _mediator.Send(new LegroomQuery());
            return Ok(ApiResponse<LegroomQueryResponse>.SuccessResponse(legrooms, null));
        }

        [HttpPost("legroom")]
        public async Task<IActionResult> UpsertLegroom(UpsertLegroomCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<UpsertLegroomCommandResponse>.SuccessResponse(null, null));
        }
        #endregion

        #region Languages

        [HttpGet("languages")]
        public async Task<IActionResult> Languages()
        {
            var languages = await _mediator.Send(new LanguagesQuery());
            return Ok(ApiResponse<LanguagesQueryResponse>.SuccessResponse(languages, null));
        }

        [HttpPost("languages")]
        public async Task<IActionResult> AddLanguage(AddLanguageCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
        }

        [HttpPatch("languages")]
        public async Task<IActionResult> UpdateLanguage(UpdateLanguageCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
        }

        #endregion

        #region Tour Difficulties

        [HttpGet("tour-difficulties")]
        public async Task<IActionResult> TourDifficulties()
        {
            var tourDifficulties = await _mediator.Send(new TourDifficultyQuery());
            return Ok(
                ApiResponse<TourDifficultyQueryResponse>.SuccessResponse(tourDifficulties, null)
            );
        }

        [HttpPost("tour-difficulties")]
        public async Task<IActionResult> AddTourDifficulty(AddTourDifficultyCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
        }

        [HttpPatch("tour-difficulties")]
        public async Task<IActionResult> UpdateTourDifficulty(UpdateTourDifficultyCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
        }

        #endregion


        [HttpGet("maps-key")]
        public async Task<IActionResult> MapsKey()
        {
            var key = _opt.MapsKey;
            return Ok(
                ApiResponse<string>.SuccessResponse(key, null)
            );
        }
    }
}
