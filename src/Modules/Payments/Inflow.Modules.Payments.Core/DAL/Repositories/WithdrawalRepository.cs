using Inflow.Modules.Payments.Core.Withdrawals.Domain.Entities;
using Inflow.Modules.Payments.Core.Withdrawals.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Payments.Core.DAL.Repositories;

internal class WithdrawalRepository(PaymentsDbContext context) : IWithdrawalRepository
{
    private readonly DbSet<Withdrawal> _withdrawals = context.Withdrawals;

    public  Task<Withdrawal> GetAsync(Guid id)
        => _withdrawals
            .Include(x => x.Account)
            .SingleOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Withdrawal withdrawal)
    {
        await _withdrawals.AddAsync(withdrawal);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Withdrawal withdrawal)
    {
        _withdrawals.Update(withdrawal);
        await context.SaveChangesAsync();
    }
}
