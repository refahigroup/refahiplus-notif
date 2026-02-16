using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Refahi.Notif.Application.Contract.Dtos.Inbox;
using Refahi.Notif.Application.Service.Message.Common;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Service.Inbox.Common
{
    public class InboxMessageService : IInboxMessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBus _bus;
        private readonly TimeSpan _expireTime = TimeSpan.FromHours(6);
        private const string InboxUnreadMessagesCountCacheKey = "InboxUnreadMessagesCount";
        private readonly IDistributedCache _cache;

        public InboxMessageService(ILogger<MessageService> logger, IUnitOfWork unitOfWork, IBus bus, IDistributedCache cache)
        {
            _bus = bus;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }
        public async Task RefreshUserUnreadInboxMessageCount(long userId, AppName appName)
        {
            var userInboxCacheKey = GetKeyName(userId, appName);
            var inboxCache = await _cache.GetAsync(userInboxCacheKey);
            var current = inboxCache is null ? new InboxCacheHolder() : inboxCache.FromByteArray<InboxCacheHolder>();
            current.UnreadMessagesCount = await _unitOfWork.UserRepository.GetUserUnreadInboxMessages(userId, appName);
            await _cache.RemoveAsync(userInboxCacheKey);
            await _cache.SetAsync(userInboxCacheKey, current.ToByteArray(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _expireTime
            });
        }
        public async Task ResetUserUnreadInboxMessageCount(long userId, AppName appName)
        {
            var userInboxCacheKey = GetKeyName(userId, appName);
            var current = new InboxCacheHolder() { UnreadMessagesCount = 0 };
            await _cache.RemoveAsync(userInboxCacheKey);
            await _cache.SetAsync(userInboxCacheKey, current.ToByteArray(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _expireTime
            });
        }
        public async Task IncreaseUserUnreadInboxMessageCount(long userId, AppName appName)
        {
            var userInboxCacheKey = GetKeyName(userId, appName);
            var inboxCache = await _cache.GetAsync(userInboxCacheKey);
            var current = inboxCache is null ? new InboxCacheHolder() : inboxCache.FromByteArray<InboxCacheHolder>();
            current.UnreadMessagesCount += 1;
            await _cache.RemoveAsync(userInboxCacheKey);
            await _cache.SetAsync(userInboxCacheKey, current.ToByteArray(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _expireTime
            });
        }
        public async Task<int> GetUserUnreadInboxMessageCount(long userId, AppName appName)
        {
            var userInboxCacheKey = GetKeyName(userId, appName);
            var inboxCache = await _cache.GetAsync(userInboxCacheKey);
            if (inboxCache == null)
            {
                var count = await _unitOfWork.UserRepository.GetUserUnreadInboxMessages(userId, appName);
                await _cache.SetAsync(userInboxCacheKey, new InboxCacheHolder() { UnreadMessagesCount = count }.ToByteArray(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _expireTime
                });
                return count;
            }
            var current = inboxCache.FromByteArray<InboxCacheHolder>();
            if (current != null)
                return current.UnreadMessagesCount;
            return 0;
        }

        private string GetKeyName(long userId, AppName appName) =>
            $"{InboxUnreadMessagesCountCacheKey}_{userId}_{Enum.GetName(appName)}";
    }
}
