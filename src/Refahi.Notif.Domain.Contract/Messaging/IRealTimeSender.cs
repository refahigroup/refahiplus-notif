namespace Refahi.Notif.Domain.Contract.Messaging
{
    public interface IRealTimeSender
    {
        Task SendAsync(string[] connectionIds, string type, string message);
        Task SendToUserAsync(long userId, string type, string message);
    }


}
