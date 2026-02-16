using Microsoft.EntityFrameworkCore;
using Refahi.Notif.Domain.Core.Aggregates._Common;
using Refahi.Notif.Infrastructure.Persistence.Contract;
using System.Linq.Expressions;

namespace Refahi.Notif.Infrastructure.Persistence.Repositories;

public class BaseRepository<TDomain, Tkey>
    where TDomain : AggregateRoot<Tkey>
{

    protected readonly DbSet<TDomain> _set;
    protected readonly IDbContext _context;
    protected readonly IQueryable<TDomain> _setIncludeRelated;
    public BaseRepository(IDbContext context, IQueryable<TDomain> all)
    {
        _context = context;
        _set = ((DbContext)context).Set<TDomain>();
        _setIncludeRelated = all;
    }

    public async Task<TDomain> AddAsync(TDomain domain)
    {
        return (await _set.AddAsync(domain)).Entity;
    }

    public void Delete(TDomain domain)
    {
        _set.Remove(domain);
    }

    public Task<TDomain> GetAsync(Tkey id)
    {
        return _setIncludeRelated.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public virtual void Update(TDomain domain)
    {
        _set.Update(domain);
    }

    public virtual async Task UpdateWithRelatedEntities<TProperty, TPropertyKey>(TDomain domain, string propertyName)
        where TProperty : Entity<TPropertyKey>
    {
        var domainInDb = await ParentFindAndSetValues(domain.Id, domain);

        UpdateRelatedProperty<TProperty, TPropertyKey>(propertyName, domain, domainInDb);
    }

    private async Task<TDomain> GetWithRelatedAsNoTracking(Tkey id)
    {
        return await _setIncludeRelated.AsNoTracking().FirstAsync(x => x.Id.Equals(id));
    }
    private async Task<TDomain> ParentFindAndSetValues(Tkey id, TDomain domain)
    {
        //fetch from db
        var domainInDb = await GetWithRelatedAsNoTracking(id);

        //update entity
        ((DbContext)_context).Entry(domainInDb).CurrentValues.SetValues(domain);

        return domainInDb;
    }
    private void UpdateRelatedProperty<TProperty, TPropertyKey>(string propertyName, TDomain domain, TDomain domainInDb)
        where TProperty : Entity<TPropertyKey>
    {
        var context = (DbContext)_context;

        var relatesInDb = (List<TProperty>)domainInDb.GetType().GetProperty(propertyName).GetValue(domainInDb, null);

        var relatesInEntity = (List<TProperty>)domain.GetType().GetProperty(propertyName).GetValue(domain, null);
        foreach (var relatedInDb in relatesInDb)
        {
            var relatedEntity = relatesInEntity.SingleOrDefault(i => i.Id.Equals(relatedInDb.Id));
            if (relatedEntity != null)
                context.Entry(relatedInDb).CurrentValues.SetValues(relatedEntity);
            else
            {
                var inChangeTracker = context.ChangeTracker.Entries().FirstOrDefault(x => x.Entity.GetType().Equals(relatedInDb.GetType()) && x.Property("Id").OriginalValue.Equals(relatedInDb.Id));

                if (inChangeTracker != null)
                    context.Remove(inChangeTracker.Entity);
                else
                    context.Remove(relatedInDb);
            }
        }

        foreach (var device in relatesInEntity)
        {
            if (relatesInDb.All(i => !i.Id.Equals(device.Id)))
            {
                context.Add(device);
            }
        }
    }

    public Task<bool> IsExistAsync(Expression<Func<TDomain, bool>> predicate)
    {
        return _set.Where(predicate).AnyAsync();
    }
}
