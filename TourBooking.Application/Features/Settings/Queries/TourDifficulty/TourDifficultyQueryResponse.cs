using System;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class TourDifficultyQueryResponse
    {
        public IEnumerable<TourDifficultyDto> TourDifficulties { get; set; }
    }
}
