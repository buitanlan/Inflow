using Inflow.Modules.Payments.Core.Deposits.Domain.Entities;
using Inflow.Modules.Payments.Core.Deposits.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Modules.Payments.Core.DAL.Repositories;

internal class DepositRepository(PaymentsDbContext context) : IDepositRepository
{
    private readonly DbSet<Deposit> _deposits = context.Deposits;

    public  Task<Deposit> GetAsync(Guid id)
        => _deposits
            .Include(x => x.Account)
            .SingleOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Deposit deposit)
    {
        await _deposits.AddAsync(deposit);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Deposit deposit)
    {
        _deposits.Update(deposit);
        await context.SaveChangesAsync();
    }
}
