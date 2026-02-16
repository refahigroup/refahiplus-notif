using Microsoft.EntityFrameworkCore;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;
using Refahi.Notif.Infrastructure.Persistence.Contract;

namespace Refahi.Notif.Infrastructure.Persistence.Repositories
{

    public class NotificationEventRepository : BaseRepository<NotificationEvent, long>, INotificationEventRepository
    {
        public NotificationEventRepository(IDbContext context) : base(context, context.NotificationEvents.AsNoTracking().AsQueryable())
        {
        }

        public Task<List<NotificationEvent>> GetListByNotificationIdAsync(Guid messageId)
        {
            return _context.NotificationEvents.Where(p => p.MessageId == messageId).ToListAsync();
        }
    }
}
