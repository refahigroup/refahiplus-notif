using Refahi.Notif.Domain.Core.Aggregates.VerifyMessageAgg;
using Refahi.Notif.Messages.NotifCenter.Enums;
using System.Linq.Expressions;

namespace Refahi.Notif.Domain.Contract.Models
{
    public class VerifyMessageModel
    {
        public static Func<VerifyMessage, VerifyMessageModel> MessageMapper = x => new VerifyMessageModel
        {
            Id = x.Id,
            Status = x.Status,
            Template = x.Template,
            DeliverTime = x.DeliverTime,
            IdInProvider = x.IdInProvider,
            PhoneNumber = x.PhoneNumber,
            SendTime = x.SendTime,
            IsAudio = x.IsAudio,
        };

        public static Expression<Func<VerifyMessage, VerifyMessageModel>> MessageExpression = x => new VerifyMessageModel
        {
            Id = x.Id,
            Status = x.Status,
            Template = x.Template,
            DeliverTime = x.DeliverTime,
            IdInProvider = x.IdInProvider,
            PhoneNumber = x.PhoneNumber,
            SendTime = x.SendTime,
            IsAudio = x.IsAudio
        };

        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public VerifySmsTemplate Template { get; set; }
        public DateTime SendTime { get; set; }
        public string IdInProvider { get; set; }
        public bool IsAudio { get; set; }
        public DateTime? DeliverTime { get; set; }
        public SmsStatus Status { get; set; }
    }
}
