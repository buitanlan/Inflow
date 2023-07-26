using Inflow.Modules.Payments.Core.DAL;
using Inflow.Modules.Payments.Core.Deposits.DTO;
using Inflow.Shared.Abstractions.Queries;

namespace Inflow.Modules.Payments.Core.Deposits.Queries.Handlers;

internal sealed class BrowseDepositsHandler(PaymentsDbContext dbContext) : IQueryHandler<BrowseDeposits, PagedQuery<DepositDto>>
{
    public Task<PagedQuery<DepositDto>> HandleAsync(BrowseDeposits query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
