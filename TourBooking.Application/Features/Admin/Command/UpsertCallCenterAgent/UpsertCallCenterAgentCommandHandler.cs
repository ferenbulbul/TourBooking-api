using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using TourBooking.Application.Expactions;
using TourBooking.Application.Interfaces.Repositories;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Enums;

namespace TourBooking.Application.Features.Settings.Commands
{
    public class UpsertCallCenterAgentCommandHandler : IRequestHandler<UpsertCallCenterAgentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        IStringLocalizer<UpsertCallCenterAgentCommandHandler> _localizer;

        public UpsertCallCenterAgentCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,
            IStringLocalizer<UpsertCallCenterAgentCommandHandler> localizer
        )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task Handle(
            UpsertCallCenterAgentCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                if (request.Id != Guid.Empty && request.Id != null)
                {
                    var existing = await _unitOfWork.CallCenterAgent(request.Id.Value);

                    if (existing != null)
                    {
                        existing.UpdatedDate = DateTime.Now;
                        existing.Email = request.Email;
                        existing.Firm = request.Firm;
                        existing.IsDeleted = !request.IsActive;

                        await _unitOfWork
                            .GetRepository<CallCenterAgentEntity>()
                            .UpdateAsync(existing);
                    }
                }
                else
                {
                    var emailExists = await _userManager.FindByEmailAsync(request.Email);
                    if (emailExists != null)
                    {
                        throw new BusinessRuleValidationException(_localizer["EmailAlreadyInUse"]);
                    }
                    
                    var newUser = new AppUser
                    {
                        FirstName = request.Email,
                        LastName = request.Email,
                        Email = request.Email,
                        UserName = request.Email,
                        EmailConfirmed = true,
                        UserType = UserType.CallCenterAgent,
                        PhoneNumber = "",
                    };
                    var result = await _userManager.CreateAsync(newUser, request.Password);
                    if (!result.Succeeded)
                    {
                        var errors = result.Errors.Select(e => e.Description).ToList();
                        throw new ValidationException(errors);
                    }

                    var callCenterAgent = new CallCenterAgentEntity
                    {
                        Id = newUser.Id,
                        CreatedDate = DateTime.Now,
                        Email = request.Email,
                        Firm = request.Firm,
                        IsDeleted = false,
                    };
                    await _unitOfWork
                        .GetRepository<CallCenterAgentEntity>()
                        .AddAsync(callCenterAgent);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
