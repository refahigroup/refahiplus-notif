using Axon.Messages.NotifCenter.Enums;
using Axon.NotifCenter.Domain.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axon.NotifCenter.Domain.Core.Aggregates.MessageAgg
{
    public class RealTimeMessage
    {
        public string[]? Addresses { get; private init; }
        public string Body { get; private init; }
        public string Type { get; private init; }

        public int RetryCount { get; private init; }
        public DateTime? SendTime { get; private set; }
        public RealTimeMessageStatus Status { get; private init; }
        public RealTimeMessage(string[] addresses, string type,string body)
        {
            Addresses = addresses;
            Type = type;
            Body = body;
            Status = RealTimeMessageStatus.Created;
        }
        private RealTimeMessage(string[] addresses, string type,string body, RealTimeMessageStatus status, int retryCount, DateTime? sendTime)
        {
            Addresses = addresses;
            Type = type;

            Body = body;
            Status = status;
            RetryCount = retryCount;
            SendTime = sendTime;
        }


        internal RealTimeMessage Enqueued()
        {
            if (Status != RealTimeMessageStatus.Created)
                throw new BussinessException(Errors.RealTimeMessageStatusStatusNotCorrect(Status));

            return new RealTimeMessage(Addresses, Type, Body,  RealTimeMessageStatus.Pending, RetryCount, null);
        }

        internal RealTimeMessage Send()
        {
            if (Status != RealTimeMessageStatus.Pending)
                throw new BussinessException(Errors.RealTimeMessageStatusStatusNotCorrect(Status));

            return new RealTimeMessage(Addresses,Type ,Body,  RealTimeMessageStatus.Sended, RetryCount, DateTime.Now);
        }


        internal RealTimeMessage Retry()
        {
            if (Status != RealTimeMessageStatus.Pending)
                throw new BussinessException(Errors.RealTimeMessageStatusStatusNotCorrect(Status));

            return new RealTimeMessage(Addresses, Type, Body,  Status, RetryCount + 1, SendTime);
        }
    }

    public enum RealTimeMessageStatus
    {
        [Display(Name = "در حال پردازش")]
        Created = 0,


        [Display(Name = "در انتظار ارسال")]
        Pending = 1,

        [Display(Name = "ارسال شده")]
        Sended = 2,

    }
}
