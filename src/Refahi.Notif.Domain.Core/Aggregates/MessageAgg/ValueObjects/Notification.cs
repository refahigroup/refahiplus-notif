
using Refahi.Notif.Domain.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects
{

    public class Notification
    {
        public string Subject { get; private init; }
        public string Body { get; private init; }
        public string? Link { get; private init; }
        public string? Icon { get; private init; }
        public DateTime? DueTime { get; private init; }
        public DateTime? ExpiredDate { get; set; }
        public string[]? ValidatorUrl { get; private init; }
        public NotificationStatus Status { get; private init; }
        public Notification()
        {
        }

        public Notification(string subject, string body, string? link, string? icon, DateTime? expiredDate)
        {
            Body = body;
            Subject = subject;
            Link = link;
            Icon = icon;
            ExpiredDate = expiredDate;
            Status = NotificationStatus.Created;
        }
        public Notification(NotificationStatus status, string subject, string body, string? link, string? icon, DateTime? expiredDate, DateTime? dueTime, string[]? validatorUrl)
        {
            Body = body;
            Subject = subject;
            Link = link;
            Icon = icon;
            ExpiredDate = expiredDate;
            Status = status;
            DueTime = dueTime;
            ValidatorUrl = validatorUrl;
        }

        public Notification(string subject, string body, string? link, string? icon, DateTime? expiredDate,
            DateTime? dueTime, string[]? validatorUrl)
        {
            Body = body;
            Subject = subject;
            Link = link;
            Icon = icon;
            ExpiredDate = expiredDate;
            DueTime = dueTime;
            ValidatorUrl = validatorUrl;
            Status = NotificationStatus.Created;
        }

        internal Notification Send()
        {
            return new Notification(NotificationStatus.Sended, Subject, Body, Link, Icon, ExpiredDate, DueTime, ValidatorUrl);
        }

        internal Notification Enqueued()
        {
            if (Status != NotificationStatus.Created)
                throw new BussinessException(Errors.NotificationStatusNotCorrect(Status));

            return new Notification(NotificationStatus.Pending, Subject, Body, Link, Icon, ExpiredDate, DueTime, ValidatorUrl);
        }
        public Notification ValidatorDeny()
        {
            return new Notification(NotificationStatus.InvalidDeny, Subject, Body, Link, Icon, ExpiredDate, DueTime, ValidatorUrl);
        }



    }
    public enum NotificationStatus
    {
        [Display(Name = "در حال پردازش")]
        Created = 0,
        [Display(Name = "در انتظار ارسال")]
        Pending = 1,

        [Display(Name = "ارسال شده")]
        Sended = 2,

        [Display(Name = "تحویل شده")]
        Delivered = 3,

        [Display(Name = "کلیک شده")]
        Clicked = 4,

        [Display(Name = "لغو شده")]
        InvalidDeny = 6,
    }
}
