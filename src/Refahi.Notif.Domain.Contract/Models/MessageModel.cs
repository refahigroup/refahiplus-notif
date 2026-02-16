using Refahi.Notif.Domain.Core.Aggregates.MessageAgg;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects;
using Refahi.Notif.Messages.NotifCenter.Enums;
using System.Linq.Expressions;

namespace Refahi.Notif.Domain.Contract.Models
{
    public class MessageModel
    {
        public static Func<Message, MessageModel> MessageMapper = x => new MessageModel
        {
            Id = x.Id,
            DueTime = x.DueTime,
            ValidatorUrl = x.ValidatorUrl,
            Email = x.Email == null ? null : new EmailModel(x.Email),
            PushNotification = x.PushNotification == null ? null : new PushNotificationModel(x.PushNotification),
            Notification = x.Notification == null ? null : new NotificationModel(x.Notification),
            Sms = x.Sms == null ? null : new SmsModel(x.Sms),
            UserId = x.UserId,
            Tags = x.Tags.Select(q => q.Value)
        };

        public static Expression<Func<Message, MessageModel>> MessageExpression = x => new MessageModel
        {
            Id = x.Id,
            DueTime = x.DueTime,
            ValidatorUrl = x.ValidatorUrl,
            Email = x.Email == null ? null : new EmailModel(x.Email),
            PushNotification = x.PushNotification == null ? null : new PushNotificationModel(x.PushNotification),
            Notification = x.Notification == null ? null : new NotificationModel(x.Notification),
            Sms = x.Sms == null ? null : new SmsModel(x.Sms),
            UserId = x.UserId,
            Tags = x.Tags.Select(q => q.Value)
        };
        public Guid Id { get; set; }
        public long? UserId { get; set; }
        public DateTime? DueTime { get; set; }
        public string? ValidatorUrl { get; set; }
        public IEnumerable<string>? Tags { get; set; }
        public SmsModel? Sms { get; set; }
        public EmailModel? Email { get; set; }
        public PushNotificationModel? PushNotification { get; set; }
        public NotificationModel? Notification { get; set; }
    }

    public class SmsModel
    {
        public SmsModel(Sms domain)
        {
            PhoneNumbers = domain.PhoneNumbers;
            Body = domain.Body;
            Status = domain.Status;
            RetryCount = domain.RetryCount;
            SendTime = domain.SendTime;
            DeliverTime = domain.DeliverTime;
            IdInProvider = domain.IdInProvider;
            Sender = domain.Sender;
            Gateway = domain.Gateway;
        }
        public string[] PhoneNumbers { get; set; }
        public string Body { get; set; }
        public SmsStatus Status { get; set; }
        public int RetryCount { get; set; }

        public DateTime? SendTime { get; set; }
        public DateTime? DeliverTime { get; set; }

        public string? IdInProvider { get; set; }
        public string? Sender { get; set; }
        public SmsGateway Gateway { get; set; }
    }

    public class EmailModel
    {
        public EmailModel(Email domain)
        {
            Addresses = domain.Addresses;
            Subject = domain.Subject;
            Body = domain.Body;
            IsHtml = domain.IsHtml;
            RetryCount = domain.RetryCount;
            SendTime = domain.SendTime;
            Status = domain.Status;
        }
        public string[] Addresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public int RetryCount { get; set; }
        public DateTime? SendTime { get; set; }
        public EmailStatus Status { get; set; }

    }
    public class NotificationModel
    {
        public NotificationModel(Notification domain)
        {
            Subject = domain.Subject;
            Body = domain.Body;
            Link = domain.Link;
            Icon = domain.Icon;
            Status = domain.Status;
        }
        public NotificationStatus Status { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
        public string? Link { get; set; }
        public string? Icon { get; set; }
    }
    public class PushNotificationModel
    {
        public PushNotificationModel(PushNotification domain)
        {
            Subject = domain.Subject;
            Body = domain.Body;
            Url = domain.Url ?? "";
            Data = domain.Data ?? "";
            Status = domain.Status;
            Jobs = domain.Jobs.Select(w => new PushNotificationJobModel(w)).ToList();
        }

        public PushNotificationStatus Status { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Url { get; set; }
        public string Data { get; set; }
        public List<PushNotificationJobModel> Jobs { get; set; }
    }
    public class PushNotificationJobModel
    {
        public PushNotificationJobModel(PushNotification.PushNotificationJob domain)
        {
            Address = domain.Address;
            Result = domain.Result ?? "";
            RetryCount = domain.RetryCount;
            SendTime = domain.SendTime;
            DeliveredTime = domain.DeliveredTime;
            ClickedTime = domain.ClickedTime;
            Status = domain.Status;
        }
        public string Address { get; set; }

        public string Result { get; set; }
        public int RetryCount { get; set; }
        public DateTime? SendTime { get; set; }
        public DateTime? DeliveredTime { get; set; }
        public DateTime? ClickedTime { get; set; }
        public PushNotificationStatus Status { get; set; }
    }
}
