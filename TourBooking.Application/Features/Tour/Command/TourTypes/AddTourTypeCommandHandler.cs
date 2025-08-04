using MediatR;
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
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.TourType(request.Id.Value);

                    if (existing != null)
                    {
                        foreach (var newTr in request.translations)
                        {
                            var existingTr = existing.Translations.FirstOrDefault(t =>
                                t.LanguageId == newTr.LanguageId
                            );

                            if (existingTr != null)
                            {
                                existingTr.Title = newTr.Title;
                                existingTr.Description = newTr.Description;
                            }
                            else
                            {
                                existing.Translations.Add(
                                    new TourTypeTranslation
                                    {
                                        Title = newTr.Title,
                                        Description = newTr.Description,
                                        LanguageId = newTr.LanguageId
                                    }
                                );
                            }
                        }
                        if (!string.IsNullOrEmpty(request.MainImageUrl))
                        {
                            existing.MainImageUrl = request.MainImageUrl;
                        }
                        if (!string.IsNullOrEmpty(request.ThumbImageUrl))
                        {
                            existing.ThumbImageUrl = request.ThumbImageUrl;
                        }

                        await _unitOfWork.GetRepository<TourTypeEnitity>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var vehicleType = new TourTypeEnitity
                    {
                        MainImageUrl = request.MainImageUrl,
                        ThumbImageUrl = request.ThumbImageUrl,
                        Translations = request
                            .translations.Select(t => new TourTypeTranslation
                            {
                                Title = t.Title,
                                Description = t.Description,
                                LanguageId = t.LanguageId,
                            })
                            .ToList()
                    };
                    await _unitOfWork.GetRepository<TourTypeEnitity>().AddAsync(vehicleType);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
