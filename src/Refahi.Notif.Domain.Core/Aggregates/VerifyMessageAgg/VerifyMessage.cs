using Refahi.Notif.Domain.Core.Aggregates._Common;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Domain.Core.Aggregates.VerifyMessageAgg
{
    public class VerifyMessage : AggregateRoot<Guid>
    {
        public string PhoneNumber { get; private init; }
        public VerifySmsTemplate Template { get; private init; }
        public DateTime SendTime { get; private init; }
        public string IdInProvider { get; private init; }
        public bool IsAudio { get; private set; }
        public DateTime? DeliverTime { get; private set; }
        public SmsStatus Status { get; private set; }

        public VerifyMessage(Guid id, string phoneNumber, VerifySmsTemplate template, DateTime sendTime, string? idInProvider, SmsStatus status, bool isAudio)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            Template = template;
            SendTime = sendTime;
            IdInProvider = idInProvider;
            Status = status;
            IsAudio = isAudio;
        }

        public void Delivered()
        {
            Status = SmsStatus.Delivered;
            DeliverTime = DateTime.Now;
        }

        public void UnDelivered()
        {
            Status = SmsStatus.UnDelivered;
        }
    }
}
