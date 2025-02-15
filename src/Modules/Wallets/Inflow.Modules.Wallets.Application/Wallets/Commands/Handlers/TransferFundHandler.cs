using Inflow.Modules.Wallets.Core.Wallets.Entities;
using Inflow.Modules.Wallets.Core.Wallets.Exceptions;
using Inflow.Modules.Wallets.Core.Wallets.Repositories;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Kernel.ValueObjects;
using Inflow.Shared.Abstractions.Messaging;
using Inflow.Shared.Abstractions.Time;
using Microsoft.Extensions.Logging;

namespace Inflow.Modules.Wallets.Application.Wallets.Commands.Handlers;

internal class TransferFundHandler(
    IWalletRepository walletRepository,
    IClock clock,
    IMessageBroker messageBroker,
    ILogger<TransferFundHandler> logger)
    : ICommandHandler<TransferFund>
{
    public async Task HandleAsync(TransferFund command, CancellationToken cancellationToken = default)
    {
        var amount = new Amount(command.Amount);
        var ownerWallet = await walletRepository.GetAsync(command.OwnerWalletId);
        if (ownerWallet is null || ownerWallet.OwnerId != command.OwnerId)
        {
            throw new WalletNotFoundException(command.OwnerWalletId);
        }

        if (ownerWallet.Currency != command.Currency)
        {
            throw new InvalidTransferCurrencyException(command.Currency);
        }

        var receiverWallet = await walletRepository.GetAsync(command.ReceiverWalletId);
        if (receiverWallet is null)
        {
            throw new WalletNotFoundException(command.ReceiverWalletId);
        }

        if (receiverWallet.Currency != command.Currency)
        {
            throw new InvalidTransferCurrencyException(command.Currency);
        }

        var now = clock.CurrentDate();
        var transfers = ownerWallet.TransferFunds(receiverWallet, amount, now);
        var outgoingTransfer = transfers.OfType<OutgoingTransfer>().Single();
        var incomingTransfer = transfers.OfType<IncomingTransfer>().Single();
        await walletRepository.UpdateAsync(ownerWallet);
        await walletRepository.UpdateAsync(receiverWallet);
        await messageBroker.PublishAsync(new IMessage[]
        {
            new FundsDeducted(ownerWallet.Id, ownerWallet.OwnerId, ownerWallet.Currency,
                outgoingTransfer.Amount, outgoingTransfer.Name, outgoingTransfer.Metadata),
            new FundsAdded(receiverWallet.Id, receiverWallet.OwnerId, receiverWallet.Currency,
                incomingTransfer.Amount, incomingTransfer.Name, incomingTransfer.Metadata)
        }, cancellationToken);
        logger.LogInformation($"Transferred {outgoingTransfer.Amount} {outgoingTransfer.Currency}" +
                               $"from wallet with ID: '{ownerWallet.Id}' to wallet with ID: '{receiverWallet.Id}'.");
    }
}
