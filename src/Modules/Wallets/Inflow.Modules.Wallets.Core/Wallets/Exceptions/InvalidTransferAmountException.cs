using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Wallets.Core.Wallets.Exceptions;

public class InvalidTransferAmountException(decimal amount) : InflowException($"Transfer has invalid amount: '{amount}'.");
