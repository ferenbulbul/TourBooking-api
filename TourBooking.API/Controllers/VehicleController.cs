using MediatR;
using Microsoft.AspNetCore.Mvc;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Features.Vehicle.Commands.AddVehicleBrand;
using TourBooking.Application.Features.Vehicle.Commands.AddVehicleType;
using TourBooking.Application.Features.Vehicle.Queries.VehicleBrands;
using TourBooking.Application.Features.Vehicles.Queries.Vehicles;

namespace TourBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : BaseController
    {
        private readonly IMediator _mediator;

        public VehicleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<IActionResult> Vehicles()
        {
            return Ok(new { id = 1, name = 2 });
        }

        [HttpGet("types")]
        public async Task<IActionResult> VehicleTypes()
        {
            var vehicleTypes = await _mediator.Send(new VehicleTypesQuery());
            return Ok(ApiResponse<VehicleTypesQueryResponse>.SuccessResponse(vehicleTypes, null));
        }

        [HttpPost("types")]
        public async Task<IActionResult> AddVehicleType(AddVehicleTypeCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
        }
        [HttpPatch("types")]
        public async Task<IActionResult> UpdateVehicleType(UpdateVehicleTypeCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
        }

        #region Vehicle Brands

        [HttpGet("brands")]
        public async Task<IActionResult> VehicleBrands()
        {
            var vehicleBrands = await _mediator.Send(new VehicleBrandsQuery());
            return Ok(ApiResponse<VehicleBrandsQueryResponse>.SuccessResponse(vehicleBrands, null));
        }

        [HttpPost("brands")]
        public async Task<IActionResult> AddVehicleBrand(AddVehicleBrandCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
        }

        [HttpPatch("brands")]
        public async Task<IActionResult> UpdateVehicleBrand(UpdateVehicleBrandCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.SuccessResponse(null, null));
        }
        #endregion
    }
}
