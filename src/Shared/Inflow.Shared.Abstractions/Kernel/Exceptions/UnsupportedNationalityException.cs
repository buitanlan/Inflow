using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Shared.Abstractions.Kernel.Exceptions;

public class UnsupportedNationalityException(string nationality) : InflowException($"Nationality: '{nationality}' is unsupported.")
{
    public string Nationality { get; } = nationality;
}