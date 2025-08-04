using MediatR;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Features
{
    public class UpsertSeatTypeCommandHandler : IRequestHandler<UpsertSeatTypeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertSeatTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            UpsertSeatTypeCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.SeatType(request.Id.Value);

                    if (existing != null)
                    {
                        foreach (var newTr in request.Translations)
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
                                    new SeatTypeTranslation
                                    {
                                        Title = newTr.Title,
                                        Description = newTr.Description,
                                        LanguageId = newTr.LanguageId
                                    }
                                );
                            }
                        }

                        await _unitOfWork.GetRepository<SeatTypeEntity>().UpdateAsync(existing);
                    }
                }
                else
                {
                    var seatType = new SeatTypeEntity
                    {
                        Translations = request
                            .Translations.Select(t => new SeatTypeTranslation
                            {
                                Title = t.Title,
                                Description = t.Description,
                                LanguageId = t.LanguageId,
                            })
                            .ToList()
                    };
                    await _unitOfWork.GetRepository<SeatTypeEntity>().AddAsync(seatType);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
