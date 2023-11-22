using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Payments.Shared.Exceptions;

internal class InvalidIbanException(string iban) : InflowException($"IBAN: '{iban}' is invalid.")
{
    public string Iban { get; } = iban;
}