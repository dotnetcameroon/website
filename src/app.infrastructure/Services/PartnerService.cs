using System.Linq.Expressions;
using app.business.Services;
using app.domain.Models.EventAggregate.Entities;
using app.infrastructure.Persistence.Repositories.Base;
using app.shared.Utilities;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace app.infrastructure.Services;
public class PartnerService(IRepository<Partner, Guid> repository) : IPartnerService
{
    private readonly IRepository<Partner,Guid> _repository = repository;

    public async Task<ErrorOr<Partner>> CreateAsync(Partner partner, CancellationToken cancellationToken = default)
    {
        partner = await _repository.AddAsync(partner, cancellationToken);
        return partner;
    }

    public async Task<ErrorOr<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }

    public async Task<ErrorOr<Partner[]>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.Table.ToArrayAsync(cancellationToken);
    }

    public async Task<ErrorOr<Partner>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var partner = await _repository.GetAsync(id, cancellationToken);
        if(partner is null)
        {
            return Error.NotFound("Partner.NotFound", "Partner not found");
        }

        return partner;
    }

    public async Task<ErrorOr<Partner>> GetAsync(Expression<Func<Partner, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var partner = await _repository.GetAsync(predicate, cancellationToken);
        if(partner is null)
        {
            return Error.NotFound("Partner.NotFound", "Partner not found");
        }

        return partner;
    }

    public Task<ErrorOr<PagedList<Partner>>> GetPagedAsync(int page = 1, int size = 5, CancellationToken cancellationToken = default)
    {
        //TODO: Implement this method based on the example on PartnerService
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<Partner>> UpdateAsync(Partner partner, CancellationToken cancellationToken = default)
    {
        return await _repository.UpdateAsync(partner, cancellationToken);
    }
}
