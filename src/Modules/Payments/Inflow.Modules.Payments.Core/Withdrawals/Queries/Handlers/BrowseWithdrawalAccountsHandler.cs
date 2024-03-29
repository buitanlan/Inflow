﻿using Inflow.Modules.Payments.Core.DAL;
using Inflow.Modules.Payments.Core.Withdrawals.DTO;
using Inflow.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Inflow.Shared.Abstractions.Queries;
using Inflow.Shared.Infrastructure.Postgres;

namespace Inflow.Modules.Payments.Core.Withdrawals.Queries.Handlers;

internal sealed class BrowseWithdrawalAccountsHandler(PaymentsDbContext dbContext) : IQueryHandler<BrowseWithdrawalAccounts, Paged<WithdrawalAccountDto>>
{
    public Task<Paged<WithdrawalAccountDto>> HandleAsync(BrowseWithdrawalAccounts query,
        CancellationToken cancellationToken = default)
    {
        var accounts = dbContext.WithdrawalAccounts.AsQueryable();
            
        if (!string.IsNullOrWhiteSpace(query.Currency))
        {
            _ = new Currency(query.Currency);
            accounts = accounts.Where(x => x.Currency == query.Currency);
        }
            
        if (query.CustomerId.HasValue)
        {
            accounts = accounts.Where(x => x.CustomerId == query.CustomerId);
        }

        return accounts.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new WithdrawalAccountDto
            {
                AccountId = x.Id,
                CustomerId = x.CustomerId,
                Currency = x.Currency,
                Iban = x.Iban,
                CreatedAt = x.CreatedAt
            })
            .PaginateAsync(query, cancellationToken);
    }
}
