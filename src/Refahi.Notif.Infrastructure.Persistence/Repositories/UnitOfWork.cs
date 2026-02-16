using Microsoft.EntityFrameworkCore;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Infrastructure.Persistence.Contract;

namespace Refahi.Notif.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public IMessageRepository MessageRepository { get; init; }
    public INotificationEventRepository NotificationEventRepository { get; init; }
    public IUserRepository UserRepository { get; init; }
    public IVerifyMessageRepository VerifyMessageRepository { get; init; }

    private readonly IDbContext _context;
    public UnitOfWork(IDbContext context, IMessageRepository messageRepository, IUserRepository userRepository, IVerifyMessageRepository verifyMessageRepository, INotificationEventRepository notificationEventRepository)
    {
        _context = context;
        MessageRepository = messageRepository;
        UserRepository = userRepository;
        VerifyMessageRepository = verifyMessageRepository;
        NotificationEventRepository = notificationEventRepository;
    }

    public async Task SaveAsync()
    {
        try
        {
            await ((DbContext)_context).SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
