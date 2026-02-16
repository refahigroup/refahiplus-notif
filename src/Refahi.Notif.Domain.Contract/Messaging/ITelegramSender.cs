namespace Refahi.Notif.Domain.Contract.Messaging
{
    public interface ITelegramSender
    {
        Task<string> SendAsync(string chatId, string? body, string? fileName, Stream? fileData);
    }
}