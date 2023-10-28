using Inflow.Modules.Payments.Core.Deposits.Events;
using Inflow.Modules.Payments.Core.Deposits.Exceptions;
using Inflow.Modules.Payments.Core.Deposits.Repositories;
using Inflow.Modules.Payments.Shared.Exceptions;
using Inflow.Modules.Payments.Shared.Repositories;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Messaging;
using Inflow.Shared.Abstractions.Time;
using Microsoft.Extensions.Logging;

namespace Inflow.Modules.Payments.Core.Deposits.Commands.Handlers;

internal sealed class StartDepositHandler(
    IDepositRepository depositRepository,
    ICustomerRepository customerRepository,
    IDepositAccountRepository depositAccountRepository,
    IClock clock,
    IMessageBroker messageBroker,
    ILogger<StartDepositHandler> logger): ICommandHandler<StartDeposit>
{
    public async Task HandleAsync(StartDeposit command, CancellationToken cancellationToken)
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

       var account = await depositAccountRepository.GetAsync(command.AccountId, command.Currency);
       if (account is null)
       {
           throw new DepositAccountNotFoundException(command.AccountId, command.AccountId);
       }

       var deposit = account.CreateDeposit(command.DepositId, command.Amount, clock.CurrentDate());
       await depositRepository.AddAsync(deposit);
       await messageBroker.PublishAsync(new DepositStarted(command.DepositId, command.CustomerId,
           command.Currency, command.Amount), cancellationToken);
       logger.LogInformation($"Started a deposit with ID: '{command.DepositId}'.");    }
}
