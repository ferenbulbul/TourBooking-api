using MediatR;

namespace TourBooking.Application.Features;

public class CustomerFavoritesQuery : IRequest<CustomerFavoritesQueryResponse>
{
    public Guid CustomerId { get; set; }
 }
