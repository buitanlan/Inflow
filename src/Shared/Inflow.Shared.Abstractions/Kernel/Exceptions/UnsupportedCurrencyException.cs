using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Shared.Abstractions.Kernel.Exceptions;

public class UnsupportedCurrencyException(string currency) : InflowException($"Currency: '{currency}' is unsupported.")
{
    public string Currency { get; } = currency;
}