using System;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class LanguagesQueryResponse
    {
        public IEnumerable<LanguageDto> Languages { get; set; }
    }
}
