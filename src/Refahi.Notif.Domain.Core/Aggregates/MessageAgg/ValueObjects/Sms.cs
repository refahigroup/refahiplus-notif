
using Refahi.Notif.Domain.Core.Exceptions;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects
{

    public class Sms
    {
        public string[] PhoneNumbers { get; private init; }
        public string Body { get; private init; }
        public SmsStatus Status { get; private init; }
        public DateTime? SendTime { get; private init; }
        public DateTime? DeliverTime { get; private init; }
        public int RetryCount { get; private init; }

        public string? IdInProvider { get; private set; }
        public string? Sender { get; private set; }
        public SmsGateway Gateway { get; private init; }
        public DateTime? DueTime { get; private init; }
        public string[]? ValidatorUrl { get; private init; }

        public Sms(string[] phoneNumbers, string body, SmsGateway gateway, string? sender)
        {
            PhoneNumbers = phoneNumbers;
            Body = body;
            Status = SmsStatus.Created;
            RetryCount = 0;
            Gateway = gateway;
            Sender = sender;
        }
        public Sms(string[] phoneNumbers, string body, SmsGateway gateway, string? sender, DateTime? dueTime, string[]? validatorUrl)
        {
            PhoneNumbers = phoneNumbers;
            Body = body;
            Status = SmsStatus.Created;
            RetryCount = 0;
            Gateway = gateway;
            Sender = sender;
            DueTime = dueTime;
            ValidatorUrl = validatorUrl;
        }
        private Sms(string[] phoneNumbers, string body, SmsStatus status, int retryCount, DateTime? sendTime, DateTime? deliverTime, string? idInProvider, string? sender, SmsGateway gateway, DateTime? dueTime, string[]? validatorUrl)
        {
            PhoneNumbers = phoneNumbers;
            Body = body;

            Status = status;
            RetryCount = retryCount;
            SendTime = sendTime;
            DeliverTime = deliverTime;
            IdInProvider = idInProvider;
            Sender = sender;
            Gateway = gateway;
            DueTime = dueTime;
            ValidatorUrl = validatorUrl;
        }
        internal Sms Enqueued()
        {
            if (Status != SmsStatus.Created)
                throw new BussinessException(Errors.SmsStatusNotCorrect(Status));

            return new Sms(PhoneNumbers, Body, SmsStatus.Pending, RetryCount, null, null, null, Sender, Gateway, DueTime, ValidatorUrl);
        }
        internal Sms Send(string idInProvider)
        {
            if (Status != SmsStatus.Pending)
                throw new BussinessException(Errors.SmsStatusNotCorrect(Status));

            return new Sms(PhoneNumbers, Body, SmsStatus.Sended, RetryCount, DateTime.Now, null, idInProvider, Sender, Gateway, DueTime, ValidatorUrl);
        }
        internal Sms Invalid()
        {
            if (Status != SmsStatus.Pending)
                throw new BussinessException(Errors.SmsStatusNotCorrect(Status));

            return new Sms(PhoneNumbers, Body, SmsStatus.Invalid, RetryCount, DateTime.Now, null, null, Sender, Gateway, DueTime, ValidatorUrl);
        }
        internal Sms Delivered()
        {
            if (Status != SmsStatus.Sended)
                throw new BussinessException(Errors.SmsStatusNotCorrect(Status));


            return new Sms(PhoneNumbers, Body, SmsStatus.Delivered, RetryCount, SendTime, DateTime.Now, IdInProvider, Sender, Gateway, DueTime, ValidatorUrl);
        }
        internal Sms ValidatorDeny()
        {
            return new Sms(PhoneNumbers, Body, SmsStatus.InvalidDeny, RetryCount, DateTime.Now, null, null, Sender, Gateway, DueTime, ValidatorUrl);
        }
        internal Sms UnDelivered()
        {
            if (Status != SmsStatus.Sended)
                throw new BussinessException(Errors.SmsStatusNotCorrect(Status));


            return new Sms(PhoneNumbers, Body, SmsStatus.UnDelivered, RetryCount, SendTime, DateTime.Now, IdInProvider, Sender, Gateway, DueTime, ValidatorUrl);
        }

        internal Sms Retry()
        {
            if (Status != SmsStatus.Pending)
                throw new BussinessException(Errors.SmsStatusNotCorrect(Status));

            return new Sms(PhoneNumbers, Body, Status, RetryCount + 1, SendTime, DeliverTime, null, Sender, Gateway, DueTime, ValidatorUrl);
        }
    }


}
