using MediatR;
using TourBooking.Application.DTOs;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

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
            var difficultyEntities = await _unitOfWork.TourDifficulties();
            if (difficultyEntities == null || !difficultyEntities.Any())
            {
                throw new NotFoundException("Tur zorluk tipi  bulunamadı.");
            }

            var dtos = difficultyEntities.Select(tt => new TourDifficultyDto
            {
                Id = tt.Id,
                Translations = tt
                    .Translations.Select(ttr => new TranslationDto
                    {
                        Title = ttr.Title,
                        Description = ttr.Description,
                        LanguageId = ttr.LanguageId
                    })
                    .ToList()
            });
            var response = new TourDifficultyQueryResponse { TourDifficulties = dtos };

            return response;
        }
    }
}
