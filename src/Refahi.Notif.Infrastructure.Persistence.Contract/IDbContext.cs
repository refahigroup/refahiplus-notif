using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;
using Refahi.Notif.Domain.Core.Aggregates.VerifyMessageAgg;

namespace Refahi.Notif.Infrastructure.Persistence.Contract
{
    public interface IDbContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<VerifyMessage> VerifyMessages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Inbox> Inbox { get; set; }
        public DbSet<InboxMessage> InboxMessages { get; set; }
        public DbSet<SettingModel> Settings { get; set; }
        public DbSet<NotificationEvent> NotificationEvents { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        
    }
}
