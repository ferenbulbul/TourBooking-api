using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Shared.Localization;

namespace TourBooking.Application.Features
{
    public class ToggleFavoriteCommandHandler : IRequestHandler<ToggleFavoriteCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public ToggleFavoriteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            ToggleFavoriteCommand request,
            CancellationToken cancellationToken
        )
        {
            var existing = await _unitOfWork.ToggleFavoriteAsync(request.CustomerId, request.TourPointId);


            if (existing == null)
            {
                var entity=new FavoriteEntity
                {
                    CustomerId = request.CustomerId,
                    TourPointId = request.TourPointId,
                    CreatedDate = DateTime.UtcNow
                };
                await _unitOfWork.GetRepository<FavoriteEntity>().AddAsync(entity);
            }
            else
            {
                await _unitOfWork.GetRepository<FavoriteEntity>().HardDeleteAsync(existing);

            }
        }
    }
}
