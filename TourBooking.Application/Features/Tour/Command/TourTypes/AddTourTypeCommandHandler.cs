using System;
using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features
{
    public class AddTourTypeCommandHandler : IRequestHandler<AddTourTypeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddTourTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddTourTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tourType = new TourTypeEnitity
                {
                    MainImageUrl = request.MainImageUrl,
                    ThumbImageUrl = request.ThumbImageUrl,
                    Translations = request.translations.Select(t => new TourTranslation
                    {
                        Title = t.Title,
                        Description = t.Description,
                        LanguageId = t.LanguageId,
                    }).ToList()
                };
                await _unitOfWork.GetRepository<TourTypeEnitity>().AddAsync(tourType);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

    }
}
