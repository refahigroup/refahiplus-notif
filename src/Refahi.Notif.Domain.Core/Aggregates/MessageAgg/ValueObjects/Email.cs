
using Refahi.Notif.Domain.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects
{
    public class Email
    {
        public string[] Addresses { get; private init; }
        public string Subject { get; private init; }

        public string Body { get; private init; }
        public bool IsHtml { get; private init; }
        public int RetryCount { get; private init; }
        public DateTime? SendTime { get; private init; }
        public EmailStatus Status { get; private init; }
        public DateTime? DueTime { get; set; }

        public Email(string[] addresses, string subject, string body, bool isHtml)
        {
            Addresses = addresses;
            Body = body;
            Subject = subject;
            IsHtml = isHtml;
            Status = EmailStatus.Created;
            RetryCount = 0;
        }
        private Email(string[] addresses, string subject, string body, bool isHtml, EmailStatus status, int retryCount, DateTime? sendTime)
        {
            Addresses = addresses;
            Body = body;
            Subject = subject;
            IsHtml = isHtml;
            Status = status;
            RetryCount = retryCount;
            SendTime = sendTime;
        }

        internal Email Send()
        {
            if (Status != EmailStatus.Pending)
                throw new BussinessException(Errors.EmailStatusNotCorrect(Status));

            return new Email(Addresses, Subject, Body, IsHtml, EmailStatus.Sended, RetryCount, DateTime.Now);
        }

        internal Email Enqueued()
        {
            if (Status != EmailStatus.Created)
                throw new BussinessException(Errors.EmailStatusNotCorrect(Status));

            return new Email(Addresses, Subject, Body, IsHtml, EmailStatus.Pending, RetryCount, null);
        }
        internal Email Retry()
        {
            if (Status != EmailStatus.Pending)
                throw new BussinessException(Errors.EmailStatusNotCorrect(Status));

            return new Email(Addresses, Subject, Body, IsHtml, Status, RetryCount + 1, SendTime);
        }
    }

    public enum EmailStatus
    {
        [Display(Name = "در حال پردازش")]
        Created = 0,

        [Display(Name = "در انتظار ارسال")]
        Pending = 1,

        [Display(Name = "ارسال شده")]
        Sended = 2
    }
}
