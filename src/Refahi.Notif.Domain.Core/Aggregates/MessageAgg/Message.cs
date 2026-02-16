using Refahi.Notif.Domain.Core.Aggregates._Common;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects;
using Refahi.Notif.Domain.Core.Exceptions;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Domain.Core.Aggregates.MessageAgg
{
    public class Message : AggregateRoot<Guid>
    {
        public DateTime CreateDate { get; private init; }
        public long? UserId { get; private init; }
        public long? Owner { get; private init; }
        public AppName? AppName { get; private init; }
        public DateTime? DueTime { get; private set; }
        public string? ValidatorUrl { get; private set; }
        public string? JobId { get; private set; }
        public Sms? Sms { get; private set; }
        public Email? Email { get; private set; }
        public PushNotification? PushNotification { get; private set; }
        public Notification? Notification { get; private set; }
        public Telegram? Telegram { get; private set; }
        public ICollection<Tag> Tags { get; private init; } = new List<Tag>();
        private Message()
        {
        }
        public Message(Guid id, DateTime? dueTime,
            string? validatorUrl,
            Sms? sms, Email? email, Telegram? telegram,
            PushNotification? pushNotification, ICollection<string>? tags)
        {
            Id = id;
            CreateDate = DateTime.Now;
            DueTime = dueTime;
            ValidatorUrl = validatorUrl;
            Sms = sms;
            Email = email;
            Telegram = telegram;
            PushNotification = pushNotification;
            tags?.ToList().ForEach(x => Tags.Add(new Tag(Id, x)));
        }
        public Message(Guid id, long? userId, AppName? appName, DateTime? dueTime,
            string? validatorUrl,
            Sms? sms, Email? email, Telegram? telegram,
            PushNotification? pushNotification, Notification? notification, ICollection<string>? tags)
        {
            Id = id;
            CreateDate = DateTime.Now;
            UserId = userId;
            AppName = appName;
            DueTime = dueTime;
            ValidatorUrl = validatorUrl;
            Sms = sms;
            Email = email;
            Telegram = telegram;
            PushNotification = pushNotification;
            Notification = notification;
            tags?.ToList().ForEach(x => Tags.Add(new Tag(Id, x)));
        }
        public Message(Guid id, long? userId, long? owner, AppName? appName, DateTime? dueTime,
            string? validatorUrl,
            Sms? sms, Email? email, Telegram? telegram,
            PushNotification? pushNotification, Notification? notification, ICollection<string>? tags)
        {
            Id = id;
            CreateDate = DateTime.Now;
            UserId = userId;
            AppName = appName;
            DueTime = dueTime;
            ValidatorUrl = validatorUrl;
            Owner = owner;
            Sms = sms;
            Email = email;
            Telegram = telegram;
            PushNotification = pushNotification;
            Notification = notification;

            tags?.ToList().ForEach(x => Tags.Add(new Tag(Id, x)));
        }
        public void Enqueued(string? jobId)
        {
            JobId = jobId;
            Sms = Sms?.Enqueued();
            Email = Email?.Enqueued();
            Telegram = Telegram?.Enqueued();
            PushNotification = PushNotification?.Enqueued();
            Notification = Notification?.Enqueued();
        }
        #region Sms
        public void SmsRetry()
        {
            if (Sms == null)
                throw new BussinessException(Errors.SmsNotFound);

            Sms = Sms.Retry();
        }

        public void SmsSend(string idInProvider)
        {
            if (Sms == null)
                throw new BussinessException(Errors.SmsNotFound);

            Sms = Sms.Send(idInProvider);
        }
        public void SmsDelivered()
        {
            if (Sms == null)
                throw new BussinessException(Errors.SmsNotFound);

            Sms = Sms.Delivered();
        }
        public void SmsInvalid()
        {
            if (Sms == null)
                throw new BussinessException(Errors.SmsNotFound);

            Sms = Sms.Invalid();
        }
        public void SmsValidatorDeny()
        {
            if (Sms == null)
                throw new BussinessException(Errors.SmsNotFound);

            Sms = Sms.ValidatorDeny();
        }

        public void SmsUnDelivered()
        {
            if (Sms == null)
                throw new BussinessException(Errors.SmsNotFound);

            Sms = Sms.UnDelivered();
        }
        #endregion

        #region Email

        public void EmailRetry()
        {
            if (Email == null)
                throw new BussinessException(Errors.EmailNotFound);

            Email = Email.Retry();
        }
        public void EmailSend()
        {
            if (Email == null)
                throw new BussinessException(Errors.EmailNotFound);

            Email = Email.Send();
        }
        #endregion

        #region Telegram

        public void TelegramRetry()
        {
            if (Telegram == null)
                throw new BussinessException(Errors.TelegramNotFound);

            Telegram = Telegram.Retry();
        }
        public void TelegramSend(string result)
        {
            if (Telegram == null)
                throw new BussinessException(Errors.TelegramNotFound);

            Telegram = Telegram.Send(result);
        }

        public void TelegramValidatorDeny()
        {
            if (Telegram == null)
                throw new BussinessException(Errors.TelegramNotFound);

            Telegram = Telegram.ValidatorDeny();
        }
        #endregion


        #region PushNotification

        public void PushNotificationRetry(string address)
        {
            if (PushNotification == null)
                throw new BussinessException(Errors.PushNotificationNotFound);

            PushNotification = PushNotification.Retry(address);
        }
        public void PushNotificationSend(string address, MessageSendingResult result)
        {
            if (PushNotification == null)
                throw new BussinessException(Errors.PushNotificationNotFound);

            PushNotification = PushNotification.Send(address, result.Message);
        }
        public void PushNotificationDelivered(string messageId)
        {
            if (PushNotification == null)
                throw new BussinessException(Errors.PushNotificationNotFound);

            PushNotification = PushNotification.Delivered(messageId);
        }
        public void PushNotificationClicked(string messageId)
        {
            if (PushNotification == null)
                throw new BussinessException(Errors.PushNotificationNotFound);
            PushNotification = PushNotification.Clicked(messageId);
        }

        public void PushNotificationValidatorDeny()
        {
            if (PushNotification == null)
                throw new BussinessException(Errors.PushNotificationNotFound);

            PushNotification = PushNotification.ValidatorDeny();
        }
        public void PushNotificationEmpty()
        {
            if (PushNotification == null)
                throw new BussinessException(Errors.PushNotificationNotFound);

            PushNotification = PushNotification.Empty();
        }
        #endregion

        public void NotificationValidatorDeny()
        {
            if (Notification == null)
                throw new BussinessException(Errors.NotificationNotFound);

            Notification = Notification.ValidatorDeny();
        }
        public void NotificationSend()
        {
            if (Notification == null)
                throw new BussinessException(Errors.NotificationNotFound);

            Notification = Notification.Send();
        }

    }
}
