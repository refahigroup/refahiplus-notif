namespace Refahi.Notif.Domain.Contract.Repositories
{
    public interface IUnitOfWork
    {
        IMessageRepository MessageRepository { get; init; }
        INotificationEventRepository NotificationEventRepository { get; init; }
        IVerifyMessageRepository VerifyMessageRepository { get; init; }
        IUserRepository UserRepository { get; init; }
        Task SaveAsync();
    }
}
