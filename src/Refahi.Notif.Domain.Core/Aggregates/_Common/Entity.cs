namespace Refahi.Notif.Domain.Core.Aggregates._Common
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; protected init; }
    }
}
