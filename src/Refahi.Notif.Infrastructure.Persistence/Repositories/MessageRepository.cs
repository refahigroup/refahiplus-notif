using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg;
using Refahi.Notif.Infrastructure.Persistence.Contract;
using Refahi.Notif.Messages.NotifCenter.Enums;

namespace Refahi.Notif.Infrastructure.Persistence.Repositories
{

    public class MessageRepository : BaseRepository<Message, Guid>, IMessageRepository
    {
        public MessageRepository(IDbContext context) : base(context, context.Messages.Include(x => x.Tags).AsNoTracking().AsQueryable())
        {
        }



        public Task<Message?> GetBySmsIdInProviderAsync(string idInProvider)
        {
            return _setIncludeRelated.FirstOrDefaultAsync(x => x.Sms.IdInProvider == idInProvider);
        }

        public Task<List<Message>> GetByTag(string tag)
        {
            return _setIncludeRelated.Where(x => x.Tags.Any(w => w.Value == tag)).ToListAsync();
        }
        public async Task DeleteByTag(string tag)
        {
            var context = (DbContext)_context;

            object[] paramItems = new object[]
            {
                new SqlParameter("@tagId", tag),
            };

            await context.Database.ExecuteSqlRawAsync("DELETE msg FROM dbo.Messages msg  \r\n" +
                                                       " INNER JOIN dbo.Tag t ON t.MessageId = msg.Id \r\n" +
                                                       " WHERE (\r\n" +
                                                       " ( msg.DueTime IS NOT NULL AND msg.DueTime>GETDATE() ) OR\r\n" +
                                                       " ( msg.Sms_DueTime IS NOT NULL AND msg.Sms_DueTime>GETDATE() ) OR\r\n" +
                                                       " ( msg.PushNotification_DueTime IS NOT NULL AND msg.PushNotification_DueTime>GETDATE() )\r\n" +
                                                       " )\r\n" +
                                                       " AND t.Value=@tagId", paramItems);
        }

        public async Task<List<MessageModel>> GetByUserIdSended(long userId)
        {
            var list = await _setIncludeRelated
                     .Where(x => x.UserId == userId && (!x.DueTime.HasValue || x.DueTime < DateTime.Now))
                     .AsNoTracking()
                     .ToListAsync();

            return list.Select(MessageModel.MessageMapper).ToList();
        }

        public Task<MessageModel> GetModelAsync(Guid id)
        {
            return _setIncludeRelated
                     .Where(x => x.Id == id)
                     .AsNoTracking()
                     .Select(MessageModel.MessageExpression)
                     .FirstOrDefaultAsync();
        }

        public Task<List<Guid>> SmsPendingRetried(int minRetryCount, DateTime? start, DateTime? end, Guid[]? ids)
        {
            return _set
                .Where(x => x.Sms.Status == SmsStatus.Pending && x.Sms.RetryCount >= minRetryCount)
                .Where(x => !start.HasValue || x.CreateDate >= start)
                .Where(x => !end.HasValue || x.CreateDate <= end)
                .Where(x => ids == null || ids.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<MessageModel>> WithPhoneNumber(string? phoneNumber)
        {
            var query = _setIncludeRelated.Where(x => !x.DueTime.HasValue && x.Sms.PhoneNumbers != null);

            if (string.IsNullOrEmpty(phoneNumber))
                query = query.OrderByDescending(x => x.CreateDate).Take(100);
            else
                query = query.Where(x => ((string)(object)x.Sms.PhoneNumbers).Contains(phoneNumber));

            var list = await query.AsNoTracking().ToListAsync();

            return list.Select(MessageModel.MessageMapper).ToList();
        }
    }
}
