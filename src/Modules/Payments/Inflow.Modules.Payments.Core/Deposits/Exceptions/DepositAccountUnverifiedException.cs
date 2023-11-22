using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Deposits.Exceptions;

public class DepositAccountUnverifiedException(Guid accountId, Guid customerId) : InflowException($"Deposit account with ID: '{accountId}' for customer with ID: '{customerId}' is unverified.")
{
    public Guid AccountId { get; } = accountId;
    public Guid CustomerId { get; } = customerId;
}