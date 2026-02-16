using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Core.Aggregates.MessageAgg;
using System.Linq.Expressions;

namespace Refahi.Notif.Domain.Contract.Repositories
{
    public interface IMessageRepository
    {
        Task<bool> IsExistAsync(Expression<Func<Message, bool>> predicate);
        Task<List<Guid>> SmsPendingRetried(int minRetryCount, DateTime? start, DateTime? end, Guid[]? ids);
        Task<Message> AddAsync(Message domain);
        Task<Message> GetAsync(Guid id);
        Task<MessageModel> GetModelAsync(Guid id);
        Task<Message> GetBySmsIdInProviderAsync(string idInProvider);
        void Delete(Message domain);
        void Update(Message message);
        Task<List<MessageModel>> WithPhoneNumber(string phoneNumber);
        Task<List<MessageModel>> GetByUserIdSended(long userId);
        Task<List<Message>> GetByTag(string tag);
        Task DeleteByTag(string tag);
    }
}
