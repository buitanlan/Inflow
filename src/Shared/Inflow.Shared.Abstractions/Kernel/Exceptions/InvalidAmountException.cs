using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Shared.Abstractions.Kernel.Exceptions;

public class InvalidAmountException(decimal amount) : InflowException($"Amount: '{amount}' is invalid.")
{
    public decimal Amount { get; } = amount;
}