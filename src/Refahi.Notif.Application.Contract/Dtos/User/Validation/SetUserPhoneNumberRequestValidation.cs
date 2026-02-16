using FluentValidation;
using Refahi.Notif.Application.Contract.Dtos.User.Commands;
using Refahi.Notif.Domain.Core.Exceptions;

namespace Refahi.Notif.Application.Contract.Dtos.User.Validation
{
    public sealed class SetUserPhoneNumberRequestValidation : AbstractValidator<SetUserPhoneNumberRequest>
    {
        public SetUserPhoneNumberRequestValidation()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Length(11)
                .WithMessage(Errors.PhoneNumberNotValid);
        }
    }
}
