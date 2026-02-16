using Refahi.Notif.Domain.Core.Aggregates._Common;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;

public class Inbox : Entity<Guid>
{
    public long UserId { get; init; }
    public AppName AppName { get; init; }
    public List<InboxMessage> Messages { get; init; } = new List<InboxMessage>();
    public Inbox()
    {

    }
    public Inbox(long userId, AppName appName, InboxMessage message)
    {
        Id = new Guid();
        UserId = userId;
        AppName = appName;
        message.InboxId = Id;
        Messages = new List<InboxMessage>()
        {
            message
        };
    }
    public void AddMessage(InboxMessage message)
    {
        Messages.Add(message);
    }
    public void ReadMessage(Guid messageId)
    {
        var message = Messages.FirstOrDefault(c => c.Id == messageId);
        message?.ReadMessage();
    }
}