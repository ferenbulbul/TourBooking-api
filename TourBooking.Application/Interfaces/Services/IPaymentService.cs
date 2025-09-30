using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourBooking.Application.DTOs;
using TourBooking.Domain.Entities;

namespace TourBooking.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<InitCheckoutFormDto> InitCheckoutFormAsync(BookingEntity booking, AppUser user);
        Task<PaymentResultDto> RetrieveCheckoutFormAsync(string token);
    }

}
