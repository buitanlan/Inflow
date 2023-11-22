using Inflow.Modules.Payments.Core.Deposits.Domain.Entities;

namespace Inflow.Modules.Payments.Core.Deposits.Repositories;

internal interface IDepositRepository
{
    Task<Deposit> GetAsync(Guid id);
    Task AddAsync(Deposit deposit);
    Task UpdateAsync(Deposit deposit);
}
