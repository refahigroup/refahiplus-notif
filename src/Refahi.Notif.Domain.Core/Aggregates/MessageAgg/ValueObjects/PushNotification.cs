using Refahi.Notif.Messages.NotifCenter.Enums;
using System.ComponentModel.DataAnnotations;

namespace Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects
{

    public class PushNotification
    {

        public string Subject { get; private init; }
        public string Body { get; private init; }

        public string? Url { get; private set; }
        public string? Data { get; private set; }
        public PushNotificationStatus Status { get; private set; }

        public List<PushNotificationJob> Jobs { get; private set; }
        public DateTime? DueTime { get; private init; }
        public string[]? ValidatorUrl { get; private init; }

        public record PushNotificationJob(DeviceType Type, string Address, int RetryCount, DateTime? SendTime, DateTime? DeliveredTime, DateTime? ClickedTime, PushNotificationStatus Status, string? Result);

        public PushNotification() { }
        public PushNotification(Tuple<DeviceType, string>[] addresses, string subject, string body, string url, string data)
        {
            Body = body;
            Subject = subject;
            Url = url;
            Data = data;
            Jobs = new List<PushNotificationJob>();

            Status = PushNotificationStatus.Created;

            if (addresses.Any(w => w.Item1 == DeviceType.Firebase))
                Jobs.AddRange(addresses.Where(p => p.Item1 == DeviceType.Firebase).Select(w =>
                    new PushNotificationJob(DeviceType.Firebase, w.Item2, 0, null, null, null, Status, null)).ToList());

            if (addresses.Any(w => w.Item1 == DeviceType.APN))
                Jobs.AddRange(addresses.Where(w => w.Item1 == DeviceType.APN).Select(w =>
                    new PushNotificationJob(DeviceType.APN, w.Item2, 0, null, null, null, Status, null)));
        }
        private PushNotification(PushNotificationStatus status, string subject, string body, string? url, string? data, DateTime? dueTime, string[]? validatorUrl, List<PushNotificationJob> jobs)
        {
            Body = body;
            Subject = subject;
            Url = url;
            Data = data;
            Jobs = jobs;
            DueTime = dueTime;
            ValidatorUrl = validatorUrl;
            Status = status;
        }

        public PushNotification(Tuple<DeviceType, string>[] addresses, string subject, string body, string url, string data, DateTime? dueTime, string[]? validatorUrl)
        {
            Body = body;
            Subject = subject;
            Url = url;
            Data = data;
            Jobs = new List<PushNotificationJob>();

            Status = PushNotificationStatus.Created;
            DueTime = dueTime;
            ValidatorUrl = validatorUrl;

            if (addresses.Any(w => w.Item1 == DeviceType.Firebase))
                Jobs.AddRange(addresses.Where(p => p.Item1 == DeviceType.Firebase).Select(w =>
                    new PushNotificationJob(DeviceType.Firebase, w.Item2, 0, null, null, null, Status, null)).ToList());

            if (addresses.Any(w => w.Item1 == DeviceType.APN))
                Jobs.AddRange(addresses.Where(w => w.Item1 == DeviceType.APN).Select(w =>
                    new PushNotificationJob(DeviceType.APN, w.Item2, 0, null, null, null, Status, null)));
        }

        internal PushNotification Enqueued()
        {
            return new PushNotification(PushNotificationStatus.Pending, Subject, Body, Url, Data, DueTime, ValidatorUrl, Jobs.Select(w => w with { Status = PushNotificationStatus.Pending }).ToList());
        }
        internal PushNotification Send(string address, string result)
        {
            return new PushNotification(PushNotificationStatus.Sended, Subject, Body, Url, Data, DueTime, ValidatorUrl,
                Jobs.Select(w => w.Address == address ? w with { Status = PushNotificationStatus.Sended, Result = result, SendTime = DateTime.Now } : w).ToList());
        }

        internal PushNotification Delivered(string messageId)
        {
            return new PushNotification(PushNotificationStatus.Delivered, Subject, Body, Url, Data, DueTime, ValidatorUrl,
                Jobs.Select(w => w.Result != null && w.Result.Contains(messageId) ? w with { Status = PushNotificationStatus.Delivered, DeliveredTime = DateTime.Now } : w).ToList());
        }

        internal PushNotification Clicked(string messageId)
        {
            return new PushNotification(PushNotificationStatus.Clicked, Subject, Body, Url, Data, DueTime, ValidatorUrl,
                Jobs.Select(w => w.Result != null && w.Result.Contains(messageId) ? w with { Status = PushNotificationStatus.Clicked, ClickedTime = DateTime.Now } : w).ToList());
        }


        internal PushNotification Retry(string address)
        {
            return new PushNotification(Status, Subject, Body, Url, Data, DueTime, ValidatorUrl,
                Jobs.Select(w => w.Address == address ? w with { RetryCount = w.RetryCount + 1 } : w).ToList());
        }
        internal PushNotification ValidatorDeny()
        {
            return new PushNotification(PushNotificationStatus.InvalidDeny, Subject, Body, Url, Data, DueTime, ValidatorUrl,
                Jobs.Select(w => w with { Status = PushNotificationStatus.InvalidDeny }).ToList());
        }
        internal PushNotification Empty()
        {
            return new PushNotification(PushNotificationStatus.Empty, Subject, Body, Url ?? "", Data ?? "", DueTime, ValidatorUrl,
                Jobs);
        }
    }

    public enum PushNotificationStatus
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
        [Display(Name = "عدم آدرس فعال")]
        Empty = 7,
    }
}
