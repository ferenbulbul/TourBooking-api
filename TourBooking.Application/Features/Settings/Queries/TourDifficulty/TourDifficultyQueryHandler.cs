using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class TourDifficultyQueryHandler
        : IRequestHandler<TourDifficultyQuery, TourDifficultyQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TourDifficultyQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TourDifficultyQueryResponse> Handle(
            TourDifficultyQuery request,
            CancellationToken cancellationToken
        )
        {
            var a = await _unitOfWork.GetRepository<TourDifficultyEntity>().GetAllAsync();
            return new TourDifficultyQueryResponse
            {
                TourDifficulties = a.Select(x => new TourDifficultyDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsActive = x.IsActive
                    })
                    .ToList()
            };
        }
    }
}
