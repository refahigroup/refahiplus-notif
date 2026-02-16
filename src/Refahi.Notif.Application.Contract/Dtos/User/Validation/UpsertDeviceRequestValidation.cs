using FluentValidation;
using Refahi.Notif.Application.Contract.Dtos.User.Commands;
using Refahi.Notif.Domain.Core.Exceptions;

namespace Refahi.Notif.Application.Contract.Dtos.User.Validation
{
    public sealed class UpsertDeviceRequestValidation : AbstractValidator<UpsertDeviceRequest>
    {
        public UpsertDeviceRequestValidation()
        {
            RuleFor(x => x.NotificationToken)
                .NotEmpty()
                .WithMessage(Errors.FireBaseTokenRequired);
        }
    }
}
