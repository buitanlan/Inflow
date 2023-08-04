using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Withdrawals.Exceptions;

public class WithdrawalAccountUnverifiedException(Guid accountId, Guid customerId) : InflowException($"Withdrawal account with ID: '{accountId}' for customer with ID: '{customerId}' is unverified.")
{
    public Guid AccountId { get; } = accountId;
    public Guid CustomerId { get; } = customerId;
}