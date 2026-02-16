using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Domain.Contract.Repositories
{
    public interface IUserRepository
    {

        Task<User> AddAsync(User domain);
        Task<User> GetAsync(long id);
        void Update(User domain);
        Task<UserModel> GetModelAsync(long id);
        Task<User?> GetByDeviceIdOrFirebaseTokenNotUserIdAsync(Guid id, string firebaseToken, long userId);
        Task<string[]> GetDeviceFirebaseTokens(long userId);
        Task<bool> IsExistByEmailNotUserIdAsync(string email, long userId);
        Task<bool> IsExistByPhoneNumberNotUserIdAsync(string phoneNumber, long userId);
        void Delete(User domain);
        Task UpdateWithRelatedEntities(User domain);
        Task<bool> DeleteDeviceByToken(string notificationToken);
        Task<List<InboxMessage>> GetUserInboxMessages(long userId);
        Task<InboxMessage?> GetInboxMessage(Guid id);
        Task<List<InboxMessage>> GetUserInboxByAppMessages(long userId, AppName requestApp);
        Task<List<InboxMessage>> GetUserInboxByAppMessages(long userId, AppName requestApp, int pageSize, int pageNumber);
        Task<int> GetUserUnreadInboxMessages(long userId, AppName requestApp);
        Task AddMessageToUserInbox(long userId, AppName appName, InboxMessage message);
        Task UpdateUserInboxMessageStatus(long userId, Guid messageId, InboxMessageStatus status);
        Task UpdateUserInboxMessageStatus(long userId, AppName appName);

    }
}
