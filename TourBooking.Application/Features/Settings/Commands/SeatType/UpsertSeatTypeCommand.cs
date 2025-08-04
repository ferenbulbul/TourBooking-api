using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features
{
    public class UpsertSeatTypeCommand : IRequest
    {
        public Guid? Id { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
