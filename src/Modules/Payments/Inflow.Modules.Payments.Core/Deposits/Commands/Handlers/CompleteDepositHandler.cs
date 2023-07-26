using Inflow.Modules.Payments.Core.Deposits.Domain.Entities;
using Inflow.Modules.Payments.Core.Deposits.Events;
using Inflow.Modules.Payments.Core.Deposits.Exceptions;
using Inflow.Modules.Payments.Core.Deposits.Repositories;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Events;
using Inflow.Shared.Abstractions.Time;
using Inflow.Shared.Infrastructure.Messaging;
using Microsoft.Extensions.Logging;

namespace Inflow.Modules.Payments.Core.Deposits.Commands.Handlers;

internal sealed class CompleteDepositHandler(
    IDepositRepository depositRepository,
    IClock clock,
    IMessageBroker messageBroker,
    ILogger<CompleteDepositHandler> logger) : ICommandHandler<CompleteDeposit>
{
    public async Task HandleAsync(CompleteDeposit command, CancellationToken cancellationToken)
    {
        var deposit = await depositRepository.GetAsync(command.DepositId);

        if (deposit is null)
        {
            throw new DepositNotFoundException(command.DepositId);
        }

        logger.LogInformation($"Started processing a deposit with ID: '{command.DepositId}'...");

        var (isCompleted, @event) = TryCompleted(deposit, command.Secret);
        var now = clock.CurrentDate();
        if (isCompleted)
        {
            deposit.Complete(now);
        }
        else
        {
            deposit.Reject(now);
        }

        await depositRepository.UpdateAsync(deposit);
        await messageBroker.PublishAsync(@event, cancellationToken);
        logger.LogInformation($"{(isCompleted ? "Completed" : "Rejected")} " +
                              $"a deposit with ID: '{command.DepositId}'.");
    }

    private static (bool IsCompleted, IEvent @event) TryCompleted(Deposit deposit, string secret)
        => secret == "secret"
            ? (true, new DepositCompleted(deposit.Id, deposit.Account.CustomerId,deposit.Account.Currency, deposit.Amount))
            : (false, new DepositRejected(deposit.Id, deposit.Account.CustomerId,deposit.Account.Currency, deposit.Amount));
}
