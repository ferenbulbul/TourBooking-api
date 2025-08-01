using System;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class LegroomQueryResponse
    {
        public IEnumerable<LegroomSpaceDto> Legrooms { get; set; }
    }
}
