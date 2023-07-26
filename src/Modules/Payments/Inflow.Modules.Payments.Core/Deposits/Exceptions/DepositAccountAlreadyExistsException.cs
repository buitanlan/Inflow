using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Deposits.Exceptions;

public class DepositAccountAlreadyExistsException(Guid customerId, string currency) : InflowException($"Deposit account for customer with ID: '{customerId}', currency: '{currency}' already exists.")
{
    public Guid CustomerId { get; } = customerId;
    public string Currency { get; } = currency;
}