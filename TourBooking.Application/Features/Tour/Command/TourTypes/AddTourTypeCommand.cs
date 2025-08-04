using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features
{
    public class AddTourTypeCommand : IRequest
    {
        public Guid? Id { get; set; }
        public string? MainImageUrl { get; set; }
        public string? ThumbImageUrl { get; set; }
        public List<TranslationDto> translations { get; set; }
    }
}
