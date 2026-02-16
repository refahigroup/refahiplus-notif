using FluentValidation;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Core.Exceptions;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Validation
{

    public sealed class SendVerifySmsRequestValidation : AbstractValidator<SendVerifySmsRequest>
    {
        public SendVerifySmsRequestValidation()
        {

            RuleFor(x => x.ExpireTime)
                .GreaterThan(DateTime.Now)
                .WithMessage(Errors.ExpireTimeShouldGreatherThanNow);

            RuleFor(x => x.PhoneNumber)
                .Length(11)
                .WithMessage(Errors.PhoneNumberShould11Character);

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(Errors.CodeRequired);

        }
    }
}
