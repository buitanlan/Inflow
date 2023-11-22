using Inflow.Modules.Payments.Core.Withdrawals.Domain.Entities;
using Inflow.Modules.Payments.Core.Withdrawals.Domain.Repositories;
using Inflow.Shared.Abstractions.Kernel.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Payments.Core.DAL.Repositories;

internal class WithdrawalAccountRepository(PaymentsDbContext context) : IWithdrawalAccountRepository
{
    private readonly DbSet<WithdrawalAccount> _withdrawalAccounts = context.WithdrawalAccounts;

    public Task<bool> ExistsAsync(Guid customerId, Currency currency)
        => _withdrawalAccounts.AnyAsync(x => x.CustomerId == customerId && x.Currency.Equals(currency));
        
    public  Task<WithdrawalAccount> GetAsync(Guid id)
        => _withdrawalAccounts.SingleOrDefaultAsync(x => x.Id == id);
        
    public Task<WithdrawalAccount> GetAsync(Guid customerId, Currency currency)
        => _withdrawalAccounts.SingleOrDefaultAsync(x => x.CustomerId == customerId && x.Currency.Equals(currency));

    public async Task AddAsync(WithdrawalAccount withdrawalAccount)
    {
        await _withdrawalAccounts.AddAsync(withdrawalAccount);
        await context.SaveChangesAsync();
    }
}
