using Inflow.Shared.Abstractions.Commands;

internal record AddFunds(Guid WalletId, string Currency, decimal Amount, string TransferName = null,
    string TransferMetadata = null) : ICommand
{
    public Guid TransferId { get; init; } = Guid.NewGuid();
}
