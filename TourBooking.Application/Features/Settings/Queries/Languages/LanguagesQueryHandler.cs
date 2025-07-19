using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class LanguagesQueryHandler : IRequestHandler<LanguagesQuery, LanguagesQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LanguagesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LanguagesQueryResponse> Handle(
            LanguagesQuery request,
            CancellationToken cancellationToken
        )
        {
            var a = await _unitOfWork.GetRepository<LanguageEntity>().GetAllAsync();
            return new LanguagesQueryResponse
            {
                Languages = a.Select(x => new LanguageDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        IsActive = x.IsActive
                    })
                    .ToList()
            };
        }
    }
}
