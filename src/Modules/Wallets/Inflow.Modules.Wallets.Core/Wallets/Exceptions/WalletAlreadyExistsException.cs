using Inflow.Shared.Abstractions.Exceptions;

namespace Inflow.Modules.Wallets.Core.Wallets.Exceptions;

public class WalletAlreadyExistsException(Guid ownerId, string currency) : InflowException($"Wallet for owner with ID: '{ownerId}', currency: '{currency}' already exists.");
