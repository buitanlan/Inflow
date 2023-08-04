using Inflow.Modules.Payments.Core.Withdrawals.Domain.Entities;
using Inflow.Modules.Payments.Core.Withdrawals.Domain.Repositories;
using Inflow.Modules.Payments.Core.Withdrawals.Events;
using Inflow.Modules.Payments.Core.Withdrawals.Exceptions;
using Inflow.Modules.Payments.Shared.Exceptions;
using Inflow.Modules.Payments.Shared.Repositories;
using Inflow.Modules.Payments.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Kernel.ValueObjects;
using Inflow.Shared.Abstractions.Messaging;
using Inflow.Shared.Abstractions.Time;

namespace Inflow.Modules.Payments.Core.Withdrawals.Commands.Handlers;

internal sealed class AddWithdrawalAccountHandler(
        IWithdrawalAccountRepository withdrawalAccountRepository,
        ICustomerRepository customerRepository,
        IMessageBroker messageBroker,
        IClock clock,
        ILogger logger)
    : ICommandHandler<AddWithdrawalAccount>
{
    public async Task HandleAsync(AddWithdrawalAccount command, CancellationToken cancellationToken = default)
    {
        _ = new Currency(command.Currency);
        _ = new Iban(command.Iban);
            
        var customer = await customerRepository.GetAsync(command.CustomerId);
        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        if (!customer.IsActive || !customer.IsVerified)
        {
            throw new CustomerNotActiveException(command.CustomerId);
        }
            
        if (await withdrawalAccountRepository.ExistsAsync(command.CustomerId, command.Currency))
        {
            throw new WithdrawalAccountAlreadyExistsException(command.CustomerId, command.Currency);
        }

        var account = new WithdrawalAccount(command.AccountId, command.CustomerId, command.Currency, command.Iban,
            clock.CurrentDate());
        await withdrawalAccountRepository.AddAsync(account);
        await messageBroker.PublishAsync(new WithdrawalAccountAdded(command.AccountId, command.CustomerId,
            command.Currency), cancellationToken);
        logger.LogInformation($"Added a withdrawal account with ID: '{command.AccountId}' " +
                               $"for customer with ID: '{command.CustomerId}'.");
    }
}
