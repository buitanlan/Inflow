using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Shared.Abstractions.Kernel.Exceptions;

public class InvalidNationalityException(string nationality) : InflowException($"Nationality: '{nationality}' is invalid.")
{
    public string Nationality { get; } = nationality;
}