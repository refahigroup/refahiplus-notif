using FluentValidation;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Core.Exceptions;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Validation
{

    public sealed class SendMessageToUserRequestValidation : AbstractValidator<SendMessageToUserRequest>
    {
        public SendMessageToUserRequestValidation()
        {
            RuleFor(x => x)
                .Must(x => x.Sms != null || x.PushNotification != null || x.Email != null || x.Notification != null || x.Telegram != null)
                .WithMessage(Errors.OneSendTypeMustDefined);

            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage(Errors.UserIdNotValid);

            RuleFor(x => x.DueTime)
                .GreaterThan(DateTime.Now)
                .When(x => x.DueTime.HasValue)
                .WithMessage(Errors.DueTimeNotValid);


            #region Email
            RuleFor(x => x.Email)
                .Must(x => !string.IsNullOrEmpty(x.Body))
                .When(x => x.Email != null)
                .WithMessage(Errors.EmailRequestNotValid);

            #endregion

            #region PushNotification
            RuleFor(x => x.PushNotification)
                .Must(y => !string.IsNullOrEmpty(y.Body) && !string.IsNullOrEmpty(y.Subject))
                .When(x => x.PushNotification != null)
                .WithMessage(Errors.PushRequestNotValid);
            #endregion

            #region Notification
            RuleFor(x => x.Notification)
                .Must(y => !string.IsNullOrEmpty(y.Body) && !string.IsNullOrEmpty(y.Subject))
                .When(x => x.Notification != null)
                .WithMessage(Errors.NotificationRequestNotValid);

            #endregion

            #region AppName
            RuleFor(x => x)
                .Must(y => y.AppName != null)
                .When(x => x.Notification != null || x.PushNotification != null)
                .WithMessage(Errors.AppNameIsRequired);

            #endregion
        }
    }
}
