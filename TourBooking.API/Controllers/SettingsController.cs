using MediatR;
using Microsoft.AspNetCore.Mvc;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features.Settings.Commands;
using TourBooking.Application.Features.Settings.Queries;
using TourBooking.Application.Features.Vehicle.Commands.AddVehicleBrand;
using TourBooking.Application.Features.Vehicle.Commands.AddVehicleType;
using TourBooking.Application.Features.Vehicle.Queries.VehicleBrands;
using TourBooking.Application.Features.Vehicles.Queries.Vehicles;

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

        [HttpPatch("vehicle-types")]
        public async Task<IActionResult> UpdateVehicleType(UpdateVehicleTypeCommand command)
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
    }
}
