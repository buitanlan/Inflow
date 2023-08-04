using Inflow.Modules.Payments.Core.Withdrawals.Domain.Repositories;
using Inflow.Modules.Payments.Core.Withdrawals.Events;
using Inflow.Modules.Payments.Core.Withdrawals.Exceptions;
using Inflow.Modules.Payments.Shared.Exceptions;
using Inflow.Modules.Payments.Shared.Repositories;
using Microsoft.Extensions.Logging;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Messaging;
using Inflow.Shared.Abstractions.Time;

namespace Inflow.Modules.Payments.Core.Withdrawals.Commands.Handlers;

internal sealed class StartWithdrawalHandler(
        ICustomerRepository customerRepository,
        IWithdrawalRepository withdrawalRepository,
        IWithdrawalAccountRepository withdrawalAccountRepository, IClock clock, IMessageBroker messageBroker,
        ILogger<StartWithdrawalHandler> logger)
    : ICommandHandler<StartWithdrawal>
{
    public async Task HandleAsync(StartWithdrawal command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetAsync(command.CustomerId);
        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        if (!customer.IsActive || !customer.IsVerified)
        {
            throw new CustomerNotActiveException(command.CustomerId);
        }
            
        var account = await withdrawalAccountRepository.GetAsync(command.CustomerId, command.Currency);
        if (account is null)
        {
            throw new WithdrawalAccountNotFoundException(command.AccountId, command.CustomerId);
        }
            
        var withdrawal = account.CreateWithdrawal(command.WithdrawalId, command.Amount, clock.CurrentDate());
        await withdrawalRepository.AddAsync(withdrawal);
        await messageBroker.PublishAsync(new WithdrawalStarted(command.WithdrawalId, command.CustomerId,
            command.Currency, command.Amount), cancellationToken);
        logger.LogInformation($"Started a withdrawal with ID: '{command.WithdrawalId}'.");
    }
}
