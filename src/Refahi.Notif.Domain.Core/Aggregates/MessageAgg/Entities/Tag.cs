using Refahi.Notif.Domain.Core.Aggregates._Common;

namespace Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities
{
    public class Tag : Entity<long>
    {
        public Guid MessageId { get; private init; }
        public string Value { get; private init; }

        internal Tag(Guid messageId, string value)
        {
            MessageId = messageId;
            Value = value;
        }
    }
}
