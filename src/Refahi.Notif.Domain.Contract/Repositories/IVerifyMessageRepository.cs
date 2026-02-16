using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Core.Aggregates.VerifyMessageAgg;

namespace Refahi.Notif.Domain.Contract.Repositories
{
    public interface IVerifyMessageRepository
    {

        Task<VerifyMessage> AddAsync(VerifyMessage domain);
        Task<VerifyMessage> GetAsync(Guid id);
        Task<VerifyMessageModel> GetModelAsync(Guid id);
        Task<List<VerifyMessageModel>> GetModelsByPhoneNumberAsync(string phoneNumber);
        Task<VerifyMessage> GetByIdInProviderAsync(string idInProvider);
        void Delete(VerifyMessage domain);
        void Update(VerifyMessage VerifyMessage);
    }
}
