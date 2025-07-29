using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class VehicleBrandsQueryHandler
        : IRequestHandler<VehicleBrandsQuery, VehicleBrandsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleBrandsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<VehicleBrandsQueryResponse> Handle(
            VehicleBrandsQuery request,
            CancellationToken cancellationToken
        )
        {
            var a = await _unitOfWork.GetRepository<VehicleBrand>().GetAllAsync();
            return new VehicleBrandsQueryResponse
            {
                VehicleBrands = a.Select(x => new VehicleBrandDto
                    {
                        Id = x.Id,                        
                        
                    })
                    .ToList()
            };
        }
    }
}
