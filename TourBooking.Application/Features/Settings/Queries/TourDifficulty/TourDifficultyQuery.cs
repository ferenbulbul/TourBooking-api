using System;
using MediatR;

namespace TourBooking.Application.Features.Settings.Queries
{
    public class TourDifficultyQuery : IRequest<TourDifficultyQueryResponse> { }
}
