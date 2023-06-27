using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Shared.Abstractions.Kernel.Exceptions;

public class InvalidCurrencyException(string currency) : InflowException($"Currency: '{currency}' is invalid.")
{
    public string Currency { get; } = currency;
}