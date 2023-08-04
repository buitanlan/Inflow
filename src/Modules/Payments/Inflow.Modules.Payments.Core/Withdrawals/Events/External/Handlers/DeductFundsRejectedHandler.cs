using Inflow.Modules.Payments.Core.Withdrawals.Domain.Repositories;
using Inflow.Modules.Payments.Core.Withdrawals.Exceptions;
using Inflow.Modules.Payments.Core.Withdrawals.Services;
using Microsoft.Extensions.Logging;
using Inflow.Shared.Abstractions.Events;
using Inflow.Shared.Abstractions.Messaging;
using Inflow.Shared.Abstractions.Time;

namespace Inflow.Modules.Payments.Core.Withdrawals.Events.External.Handlers;

internal sealed class DeductFundsRejectedHandler(IWithdrawalRepository withdrawalRepository,
        IMessageBroker messageBroker,
        IWithdrawalMetadataResolver metadataResolver, IClock clock, ILogger<FundsDeductedHandler> logger)
    : IEventHandler<DeductFundsRejected>
{
    private const string TransferName = "withdrawal";

    public async Task HandleAsync(DeductFundsRejected @event, CancellationToken cancellationToken = default)
    {
        if (@event.TransferName != TransferName)
        {
            return;
        }

        var withdrawalId = metadataResolver.TryResolveWithdrawalId(@event.TransferMetadata);
        if (!withdrawalId.HasValue)
        {
            return;
        }

        var withdrawal = await withdrawalRepository.GetAsync(withdrawalId.Value);
        if (withdrawal is null)
        {
            throw new WithdrawalNotFoundException(withdrawalId.Value);
        }
            
        withdrawal.Reject(clock.CurrentDate());
        await withdrawalRepository.UpdateAsync(withdrawal);
        await messageBroker.PublishAsync(new WithdrawalRejected(withdrawal.Id, withdrawal.Account.CustomerId,
            withdrawal.Currency, withdrawal.Amount), cancellationToken);
        logger.LogInformation($"Rejected withdrawal with ID: '{withdrawal.Id}'.");
    }
}
