namespace Refahi.Notif.Domain.Contract.Messaging
{
    public interface IEmailSender
    {
        Task Send(string[] addresses, string subject, string body, bool isHtml);
    }
}
