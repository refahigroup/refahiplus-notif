using Microsoft.EntityFrameworkCore;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Domain.Contract.Repositories;
using Refahi.Notif.Domain.Core.Aggregates.VerifyMessageAgg;
using Refahi.Notif.Infrastructure.Persistence.Contract;

namespace Refahi.Notif.Infrastructure.Persistence.Repositories
{

    public class VerifyMessageRepository : BaseRepository<VerifyMessage, Guid>, IVerifyMessageRepository
    {
        public VerifyMessageRepository(IDbContext context) : base(context, context.VerifyMessages.AsQueryable())
        {
        }

        public Task<VerifyMessage> GetByIdInProviderAsync(string idInProvider)
        {
            return _setIncludeRelated.FirstOrDefaultAsync(x => x.IdInProvider == idInProvider);
        }



        public Task<VerifyMessageModel> GetModelAsync(Guid id)
        {
            return _setIncludeRelated
                     .Where(x => x.Id == id)
                     .AsNoTracking()
                     .Select(VerifyMessageModel.MessageExpression)
                     .FirstOrDefaultAsync();
        }

        public Task<List<VerifyMessageModel>> GetModelsByPhoneNumberAsync(string phoneNumber)
        {
            var q = _setIncludeRelated.AsQueryable();

            if (!string.IsNullOrEmpty(phoneNumber))
                q = q.Where(x => x.PhoneNumber.Contains(phoneNumber));

            return q
                .OrderByDescending(x => x.SendTime)
            .Select(VerifyMessageModel.MessageExpression)
            .Take(100)
            .ToListAsync();
        }
    }
}
