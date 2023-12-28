using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Wallets.Core.Wallets.Exceptions;

public class InvalidTransferCurrencyException(string currency) : InflowException($"Transfer has invalid currency: '{currency}'.");
