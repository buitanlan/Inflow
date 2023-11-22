using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Deposits.Exceptions;

public class DepositAccountNotFoundException(Guid accountId, Guid customerId) : InflowException($"Deposit account with ID: '{accountId}' for customer with ID: '{customerId}' was not found.")
{
    public Guid AccountId { get; } = accountId;
    public Guid CustomerId { get; } = customerId;
}