using Microsoft.EntityFrameworkCore;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;
using Refahi.Notif.Infrastructure.Persistence.Contract;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Infrastructure.Persistence.Repositories
{

    public class UserRepository : BaseRepository<User, long>, IUserRepository
    {
        private readonly DbSet<Device> _devicesSet;
        private readonly DbSet<Inbox> _inbox;
        private readonly DbSet<InboxMessage> _inboxMessages;
        private readonly IDbContext _context;
        public UserRepository(IDbContext context) : base(context, context.Users.Include(x => x.Devices).AsNoTracking().AsQueryable())
        {
            _context = context;
            _devicesSet = context.Devices;
            _inbox = context.Inbox;
            _inboxMessages = context.InboxMessages;
        }

        public Task<User?> GetByDeviceIdOrFirebaseTokenNotUserIdAsync(Guid id, string firebaseToken, long userId)
        {
            return _setIncludeRelated.FirstOrDefaultAsync(x => x.Id != userId && x.Devices.Any(e => e.Id == id || e.NotificationToken == firebaseToken));
        }

        public Task<string[]> GetDeviceFirebaseTokens(long userId)
        {
            return _devicesSet.Where(x => x.UserId == userId).Select(x => x.NotificationToken).ToArrayAsync();
        }

        public Task<UserModel> GetModelAsync(long id)
        {
            return _setIncludeRelated.Where(x => x.Id == id).Select(UserModel.UserExpression).FirstOrDefaultAsync();
        }

        public Task<bool> IsExistByEmailNotUserIdAsync(string email, long userId)
        {
            return _set.AnyAsync(x => x.Email == email && x.Id != userId);
        }

        public Task<bool> IsExistByPhoneNumberNotUserIdAsync(string phoneNumber, long userId)
        {
            return _set.AnyAsync(x => x.PhoneNumber == phoneNumber && x.Id != userId);
        }

        //override to handle related entity add and delete
        public Task UpdateWithRelatedEntities(User domain)
        {
            return UpdateWithRelatedEntities<Device, Guid>(domain, nameof(domain.Devices));
        }
        public Task<bool> DeleteDeviceByToken(string notificationToken)
        {
            var device = _devicesSet.FirstOrDefault(p => p.NotificationToken == notificationToken);
            if (device != null) _context.Devices.Remove(device);
            return Task.FromResult(true);
        }
        public async Task<List<InboxMessage>> GetUserInboxMessages(long userId)
        {
            var userInbox = await _inbox.Include(c => c.Messages.Where(p => !p.IsDeleted))
                .FirstOrDefaultAsync(c => c.UserId == userId);
            return userInbox == null ? new List<InboxMessage>() : userInbox.Messages;
        }
        public async Task<InboxMessage?> GetInboxMessage(Guid id)
        {
            return await _inboxMessages.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<InboxMessage>> GetUserInboxByAppMessages(long userId, AppName requestApp)
        {
            var userInbox = await _inbox.Include(c => c.Messages.Where(p => !p.IsDeleted))
                .FirstOrDefaultAsync(c => c.UserId == userId && c.AppName == requestApp);
            return userInbox == null ? new List<InboxMessage>() : userInbox.Messages;
        }
        public async Task<List<InboxMessage>> GetUserInboxByAppMessages(long userId, AppName requestApp, int pageSize, int pageNumber)
        {
            return await _inboxMessages.Where(p =>
                     p.Inbox.UserId == userId &&
                     p.Inbox.AppName == requestApp && !p.IsDeleted &&
                     (p.ExpiredDate == null || p.ExpiredDate >= DateTime.Now) &&
                     p.Status != InboxMessageStatus.Hide && p.Status != InboxMessageStatus.Done
                     )
                 .OrderByDescending(p => p.CreatedTime)
                 .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<int> GetUserUnreadInboxMessages(long userId, AppName requestApp)
        {
            return await _inboxMessages.Where(p =>
                p.Inbox.UserId == userId &&
                p.Inbox.AppName == requestApp && !p.IsDeleted &&
                (p.ExpiredDate == null || p.ExpiredDate >= DateTime.Now) &&
                p.Status != InboxMessageStatus.Hide && p.Status != InboxMessageStatus.Done
            ).CountAsync();
        }
        public async Task AddMessageToUserInbox(long userId, AppName appName, InboxMessage message)
        {
            var userInbox = await _inbox.Include(c => c.Messages).FirstOrDefaultAsync(c => c.UserId == userId && c.AppName == appName);
            if (userInbox == null)
            {
                if (_context.Users.Any(p => p.Id == userId))
                {
                    _context.Inbox.Add(new Inbox(userId, appName, message));
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                message.InboxId = userInbox.Id;
                _inboxMessages.Add(message);
            }
        }
        public async Task UpdateUserInboxMessageStatus(long userId, Guid messageId, InboxMessageStatus status)
        {
            var inboxMessage = await _inboxMessages
                .Include(c => c.Inbox)
                .FirstOrDefaultAsync(c => c.Id == messageId && c.Inbox.UserId == userId);
            if (inboxMessage is null) return;
            inboxMessage.SetStatus(status);
            _context.InboxMessages.Update(inboxMessage);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserInboxMessageStatus(long userId, AppName appName)
        {
            var inboxMessages = await _inboxMessages
                .Include(c => c.Inbox)
                .Where(c => c.ReadTime == null && c.Inbox.UserId == userId && c.Inbox.AppName == appName).ToListAsync();
            if (inboxMessages is null) return;
            foreach (InboxMessage inboxMessage in inboxMessages)
            {
                inboxMessage.ReadMessage();
                _context.InboxMessages.Update(inboxMessage);
            }

        }
    }
}
