using FluentValidation;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Core.Exceptions;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Validation
{

    public sealed class SendMessageRequestValidation : AbstractValidator<SendMessageRequest>
    {
        public SendMessageRequestValidation()
        {
            RuleFor(x => x)
                .Must(x => x.Sms != null || x.PushNotification != null || x.Email != null || x.Telegram != null)
                .WithMessage(Errors.OneSendTypeMustDefined);

            RuleFor(x => x.DueTime)
                .GreaterThan(DateTime.Now)
                .When(x => x.DueTime.HasValue)
                .WithMessage(Errors.DueTimeNotValid);

            #region Sms
            RuleFor(x => x.Sms)
                .Must(x => x.PhoneNumbers.Any())
                .When(x => x.Sms != null)
                .WithMessage(Errors.SmsRequestNotValid);

            RuleForEach(x => x.Sms.PhoneNumbers).Length(11).When(x => x.Sms != null).WithMessage(Errors.SmsRequestNotValid);
            #endregion

            #region Email
            RuleFor(x => x.Email)
                .Must(x => x.Addresses.Any())
                .When(x => x.Email != null)
                .WithMessage(Errors.EmailRequestNotValid);

            RuleForEach(x => x.Email.Addresses)
                .EmailAddress()
                .When(x => x.Email != null)
                .WithMessage(Errors.EmailRequestNotValid);
            #endregion

            #region PushNotification
            RuleFor(x => x.PushNotification)
                .Must(x => x.Addresses.Any())
                .When(x => x.PushNotification != null)
                .WithMessage(Errors.PushRequestNotValid);

            //RuleFor(x => x.PushNotification)
            //    .Must(x => x.Addresses.Count(w=>w.DeviceType == Messages.NotifCenter.Enums.DeviceType.APN) < 2)
            //    .When(x => x.PushNotification != null)
            //    .WithMessage(Errors.MaxiOSPushIsOne);

            RuleFor(x => x.PushNotification)
                .Must(x => !x.Addresses.GroupBy(w => w.Address).Any(q => q.Count() > 1))
                .When(x => x.PushNotification != null)
                .WithMessage(Errors.PushNotificationDuplicateAddress);
            #endregion
        }
    }
}
