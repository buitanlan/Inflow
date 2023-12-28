using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Wallets.Core.Wallets.Exceptions;

internal class InsufficientWalletFundsException(Guid walletId) : InflowException(
    $"Insufficient funds for wallet with ID: '{walletId}'.");
