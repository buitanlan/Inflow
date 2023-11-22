using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Core.Withdrawals.Exceptions;

public class WithdrawalAccountAlreadyExistsException(Guid customerId, string currency) : InflowException($"Withdrawal account for customer with ID: '{customerId}', currency: '{currency}' already exists.")
{
    public Guid CustomerId { get; } = customerId;
    public string Currency { get; } = currency;
}