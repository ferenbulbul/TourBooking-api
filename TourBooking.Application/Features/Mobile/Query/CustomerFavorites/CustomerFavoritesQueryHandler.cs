using System.Globalization;
using MediatR;
using TourBooking.Application.DTOs.Mobile;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;

namespace TourBooking.Application.Features;

public class CustomerFavoritesQueryHandler
    : IRequestHandler<CustomerFavoritesQuery, CustomerFavoritesQueryResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerFavoritesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerFavoritesQueryResponse> Handle(
        CustomerFavoritesQuery request,
        CancellationToken cancellationToken
    )
    {
        var points = await _unitOfWork.CustomerFavorites(request.CustomerId);
        if (points == null || !points.Any())
        {
            return new CustomerFavoritesQueryResponse();
        }
        var response = new CustomerFavoritesQueryResponse { TourPoints = points };
        return response;
    }
}
