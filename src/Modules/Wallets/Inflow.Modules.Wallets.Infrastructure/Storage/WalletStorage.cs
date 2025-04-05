using System.Linq.Expressions;
using Inflow.Modules.Wallets.Application.Wallets.Storage;
using Inflow.Modules.Wallets.Core.Wallets.Entities;
using Inflow.Modules.Wallets.Infrastructure.EF;
using Inflow.Shared.Abstractions.Queries;
using Inflow.Shared.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Wallets.Infrastructure.Storage;

internal sealed class WalletStorage(WalletsDbContext dbContext): IWalletStorage
{
    public Task<Wallet> FindAsync(Expression<Func<Wallet, bool>> expression)
        =>  dbContext.Wallets
            .AsNoTracking()
            .AsQueryable()
            .Where(expression)
            .Include(x => x.Transfers)
            .SingleOrDefaultAsync();

    public Task<Paged<Wallet>> BrowseAsync(Expression<Func<Wallet, bool>> expression, IPagedQuery query)
        =>  dbContext.Wallets
            .AsNoTracking()
            .AsQueryable()
            .Where(expression)
            .OrderBy(x => x.CreatedAt)
            .PaginateAsync(query);
}
