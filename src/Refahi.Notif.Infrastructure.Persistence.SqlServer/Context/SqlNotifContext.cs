using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.Entities;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg.ValueObjects;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg;
using Refahi.Notif.Domain.Core.Aggregates.UserAgg.Entities;
using Refahi.Notif.Domain.Core.Aggregates.VerifyMessageAgg;
using Refahi.Notif.Domain.Core.Utility;
using Refahi.Notif.Infrastructure.Persistence.Contract;

namespace Refahi.Notif.Infrastructure.Persistence.SqlServer.Context;

public class SqlNotifContext : DbContext, IDbContext
{

    public SqlNotifContext(DbContextOptions<SqlNotifContext> options)
       : base(options)
    {
    }




    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Message>()
           .OwnsOne(x => x.Sms)
           .Property(e => e.PhoneNumbers)
           .HasConversion<ArrayConverter>();

        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.PushNotification)
            .Property(e => e.Jobs)
            .HasConversion<JobArrayConverter>();
        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Notification)
            .Property(e => e.Subject).HasMaxLength(512);
        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Notification)
            .Property(e => e.Body).HasMaxLength(1024);
        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Notification)
            .Property(e => e.Link).HasMaxLength(512);
        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Notification)
            .Property(e => e.Icon).HasMaxLength(32);
        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Notification)
            .Property(e => e.ExpiredDate).HasColumnType("DateTime");

        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Telegram)
            .Property(e => e.Body).HasMaxLength(1024);
        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Telegram)
            .Property(e => e.FileName).HasMaxLength(128);
        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Telegram)
            .Property(e => e.FileId).HasMaxLength(128);
        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Telegram)
            .Property(e => e.ChatId).HasMaxLength(128);

        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Notification)
            .Property(e => e.ValidatorUrl)
            .HasMaxLength(512)
            .HasConversion<ArrayConverter>();

        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Notification);

        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Sms)
            .Property(e => e.ValidatorUrl)
            .HasMaxLength(512)
            .HasConversion<ArrayConverter>();
        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.PushNotification)
            .Property(e => e.ValidatorUrl)
            .HasMaxLength(512)
            .HasConversion<ArrayConverter>();

        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Telegram)
            .Property(e => e.ValidatorUrl)
            .HasMaxLength(512)
            .HasConversion<ArrayConverter>(); ;

        modelBuilder.Entity<Message>()
            .Property(e => e.ValidatorUrl)
            .HasMaxLength(512); ;


        modelBuilder.Entity<Message>()
            .OwnsOne(x => x.Email)
            .Property(e => e.Addresses)
            .HasConversion<ArrayConverter>();

        modelBuilder.Entity<NotificationEvent>()
            .Property(x => x.Id);
        modelBuilder.Entity<NotificationEvent>()
            .Property(e => e.EventDateTime)
            .HasColumnType("DateTime")
            .HasDefaultValueSql("GetDate()");
        modelBuilder.Entity<NotificationEvent>()
            .Property(e => e.FCMMessageId)
            .HasMaxLength(36);
        modelBuilder.Entity<NotificationEvent>()
            .Property(e => e.EventName)
            .HasMaxLength(64);
        modelBuilder.Entity<NotificationEvent>()
            .Property(e => e.UserAgent)
            .HasMaxLength(256);

        modelBuilder.Entity<User>()
          .Property(x => x.Id)
          .ValueGeneratedNever();


        modelBuilder.Entity<User>()
          .HasMany(x => x.Devices)
          .WithOne()
          .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<Device>()
            .HasIndex(x => x.NotificationToken)
            .IsUnique();

        modelBuilder.Entity<Device>()
            .Property(e => e.CreateDateTime)
            .HasColumnType("DateTime")
            .HasDefaultValueSql("GetDate()");
        modelBuilder.Entity<Device>()
            .Property(e => e.ModifiedDateTime)
            .HasColumnType("DateTime");

        modelBuilder.Entity<User>()
            .HasMany(x => x.Inboxes)
            .WithOne();
        modelBuilder.Entity<Inbox>()
            .HasMany(x => x.Messages)
            .WithOne(x => x.Inbox)
            .HasForeignKey(x => x.InboxId);
        modelBuilder.Entity<Inbox>()
            .HasIndex(x => new { x.UserId, x.AppName })
            .IsUnique();
        modelBuilder.Entity<Inbox>()
            .HasKey(x => x.Id);
        modelBuilder.Entity<InboxMessage>().Property(p => p.Title).HasMaxLength(512);
        modelBuilder.Entity<InboxMessage>().Property(p => p.Description).HasMaxLength(1024);
        modelBuilder.Entity<InboxMessage>().Property(p => p.Icon).HasMaxLength(32);
        modelBuilder.Entity<InboxMessage>().Property(p => p.Link).HasMaxLength(512);
        modelBuilder.Entity<InboxMessage>().Property(p => p.CreatedTime).HasColumnType("DateTime");
        modelBuilder.Entity<InboxMessage>().Property(p => p.ReadTime).HasColumnType("DateTime");
        modelBuilder.Entity<InboxMessage>().Property(p => p.DeliveredTime).HasColumnType("DateTime");
        modelBuilder.Entity<InboxMessage>().Property(p => p.ExpiredDate).HasColumnType("DateTime");
        modelBuilder.Entity<Device>()
            .HasIndex(x => x.NotificationToken)
            .IsUnique();


        modelBuilder.Entity<SettingModel>()
            .HasKey(x => x.Key);
    }
    //entities
    public DbSet<Message> Messages { get; set; }
    public DbSet<VerifyMessage> VerifyMessages { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Inbox> Inbox { get; set; }
    public DbSet<InboxMessage> InboxMessages { get; set; }
    public DbSet<SettingModel> Settings { get; set; }
    public DbSet<NotificationEvent> NotificationEvents { get; set; }
}

public class ArrayConverter : ValueConverter<string[], string>
{
    public ArrayConverter()
        : base(
              v => string.Join(",", v),
              v => v.Split(',', StringSplitOptions.None))
    {
    }
}

public class JobArrayConverter : ValueConverter<List<PushNotification.PushNotificationJob>, string>
{
    public JobArrayConverter()
        : base(
              v => string.Join("#-#", v.Select(q => q.Serilize()).ToArray()),
              v => v.Split("#-#", StringSplitOptions.None).Select(q => q.DeSerilize<PushNotification.PushNotificationJob>()).ToList())
    {
    }
}
