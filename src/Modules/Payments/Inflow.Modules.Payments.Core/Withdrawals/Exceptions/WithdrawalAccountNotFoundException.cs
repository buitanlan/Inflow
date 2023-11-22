using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Withdrawals.Exceptions;

public class WithdrawalAccountNotFoundException(Guid accountId, Guid customerId) : InflowException($"Withdrawal account with ID: '{accountId}' for customer with ID: '{customerId}' was not found.")
{
    public Guid AccountId { get; } = accountId;
    public Guid CustomerId { get; } = customerId;
}