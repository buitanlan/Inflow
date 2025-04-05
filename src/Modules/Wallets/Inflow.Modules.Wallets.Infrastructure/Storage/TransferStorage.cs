using System.Linq.Expressions;
using Inflow.Modules.Wallets.Application.Wallets.Storage;
using Inflow.Modules.Wallets.Core.Wallets.Entities;
using Inflow.Modules.Wallets.Infrastructure.EF;
using Inflow.Shared.Abstractions.Queries;
using Inflow.Shared.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Wallets.Infrastructure.Storage;

internal sealed class TransferStorage(WalletsDbContext dbContext) : ITransferStorage
{
    public Task<Transfer> FindAsync(Expression<Func<Transfer, bool>> expression)
        => dbContext.Transfers
            .AsNoTracking()
            .AsQueryable()
            .Where(expression)
            .SingleOrDefaultAsync();

    public Task<Paged<Transfer>> BrowseAsync(Expression<Func<Transfer, bool>> expression, IPagedQuery query)
        => dbContext.Transfers
            .AsNoTracking()
            .AsQueryable()
            .Where(expression)
            .OrderBy(x => x.CreatedAt)
            .PaginateAsync(query);
}
