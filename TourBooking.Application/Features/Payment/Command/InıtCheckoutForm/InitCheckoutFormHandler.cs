using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TourBooking.Application.DTOs;
using TourBooking.Application.DTOs.Comman;
using TourBooking.Application.Expactions;
using TourBooking.Application.Features;
using TourBooking.Application.Features.Payment.Command;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

public class InitCheckoutFormHandler
    : IRequestHandler<InitCheckoutFormCommand, InitCheckoutFormCommandResponse>
{
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public InitCheckoutFormHandler(IPaymentService paymentService, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<InitCheckoutFormCommandResponse> Handle(InitCheckoutFormCommand request, CancellationToken cancellationToken)
    {
        //Booking var mı kontrol et
        var booking = await _unitOfWork.GetRepository<BookingEntity>().GetByIdAsync(request.BookingId);
        if (booking == null)
            throw new BusinessRuleValidationException("Siparişiniz bulunamadı");

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
            throw new BusinessRuleValidationException("Kullanıcı bulunamadı");

        //Init ödeme
        var dto = await _paymentService.InitCheckoutFormAsync(booking,user);

        //Payment DB’ye kaydet
        var payment = new PaymentEntity
        {
            BookingId = booking.Id,
            ConversationId = dto.ConversationId,
            Token = dto.PaymentPageUrl.Split("token=").LastOrDefault() ?? string.Empty,
            Amount = booking.TotalPrice,
            Status = PaymentStatus.Pending,
            RawResponse = System.Text.Json.JsonSerializer.Serialize(dto)
        };

        await _unitOfWork.GetRepository<PaymentEntity>().AddAsync(payment);

        var response = new InitCheckoutFormCommandResponse
        {
            ConversationId = dto.ConversationId,
            PaymentPageUrl = dto.PaymentPageUrl,
            TokenExpireTime = dto.TokenExpireTime
        };
        return response;
    }
}
