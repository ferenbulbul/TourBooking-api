using MediatR;
using Microsoft.AspNetCore.Mvc;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features.Settings.Commands;
using TourBooking.Application.Features.Settings.Queries;

namespace TourBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : BaseController
    {
        private readonly IMediator _mediator;

        public SettingsController(IMediator mediator)
        {
            _mediator = mediator;
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

        [HttpPatch("vehicle-brands")]
        public async Task<IActionResult> UpdateVehicleBrand(UpdateVehicleBrandCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
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
    }
}
