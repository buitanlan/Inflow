using Inflow.Modules.Payments.Core.Withdrawals.Domain.Entities;
using Inflow.Modules.Payments.Core.Withdrawals.Domain.Repositories;
using Inflow.Modules.Payments.Core.Withdrawals.Events;
using Inflow.Modules.Payments.Core.Withdrawals.Exceptions;
using Microsoft.Extensions.Logging;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Events;
using Inflow.Shared.Abstractions.Messaging;
using Inflow.Shared.Abstractions.Time;

namespace Inflow.Modules.Payments.Core.Withdrawals.Commands.Handlers;

internal sealed class CompleteWithdrawalHandler(
        IWithdrawalRepository withdrawalRepository,
        IClock clock,
        IMessageBroker messageBroker,
        ILogger<CompleteWithdrawalHandler> logger)
    : ICommandHandler<CompleteWithdrawal>
{
    public async Task HandleAsync(CompleteWithdrawal command, CancellationToken cancellationToken = default)
    {
        var withdrawal = await withdrawalRepository.GetAsync(command.WithdrawalId);
        if (withdrawal is null)
        {
            throw new WithdrawalNotFoundException(command.WithdrawalId);
        }
            
        logger.LogInformation($"Started processing a withdrawal with ID: '{command.WithdrawalId}'...");
        var (isCompleted, @event) = TryComplete(withdrawal, command.Secret);
        var now = clock.CurrentDate();
        if (isCompleted)
        {
            withdrawal.Complete(now);
        }
        else
        {
            withdrawal.Reject(now);
        }
            
        await withdrawalRepository.UpdateAsync(withdrawal);
        await messageBroker.PublishAsync(@event, cancellationToken);
        logger.LogInformation($"{(isCompleted ? "Completed" : "Rejected")} " +
                               $"a withdrawal with ID: '{command.WithdrawalId}'.");
    }

    private static (bool isCompleted, IEvent @event) TryComplete(Withdrawal withdrawal, string secret)
    {
        // This could be refactored to an application service with checksum validation etc.
        return secret == "secret"
            ? (true, new WithdrawalCompleted(withdrawal.Id, withdrawal.Account.CustomerId,
                withdrawal.Account.Currency, withdrawal.Amount))
            : (false, new WithdrawalRejected(withdrawal.Id, withdrawal.Account.CustomerId,
                withdrawal.Account.Currency, withdrawal.Amount));
    }
}
