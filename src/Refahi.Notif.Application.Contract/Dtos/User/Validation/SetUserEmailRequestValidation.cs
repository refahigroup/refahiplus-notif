using FluentValidation;
using Refahi.Notif.Application.Contract.Dtos.User.Commands;
using Refahi.Notif.Domain.Core.Exceptions;

namespace Refahi.Notif.Application.Contract.Dtos.User.Validation
{
    public sealed class SetUserEmailRequestValidation : AbstractValidator<SetUserEmailRequest>
    {
        public SetUserEmailRequestValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage(Errors.EmailNotValid);
        }
    }
}
