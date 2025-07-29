using System;
using MediatR;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features
{
    public class UpsertRegionCommand : IRequest
    {
        public Guid? Id { get; set; }
        public Guid CountryId { get; set; }
        public List<TranslationDto> Translations { get; set; }
    }
}
