using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourBooking.Application.DTOs;

namespace TourBooking.Application.Interfaces.Services
{
    public interface INetgsmSmsService
    {
        /// Tek numaraya SMS
        Task SendAsync(string phoneNumber, string message, CancellationToken ct = default);

        /// Çoklu SMS (aynı başlıkla)
        Task SendBatchAsync(IEnumerable<SmsMessageDto> items, CancellationToken ct = default);
    }
}