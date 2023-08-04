using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Modules.Customers.Core.Domain.ValueObjects;
using Inflow.Modules.Customers.Core.Events;
using Inflow.Modules.Customers.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Messaging;
using Inflow.Shared.Abstractions.Time;

namespace Inflow.Modules.Customers.Core.Commands.Handlers;

internal sealed class CompleteCustomerHandler(
    ICustomerRepository customerRepository,
    IMessageBroker messageBroker,
    IClock clock,
    ILogger<CompleteCustomerHandler> logger) : ICommandHandler<CompleteCustomer>
{

    public async Task HandleAsync(CompleteCustomer command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetAsync(command.CustomerId);
        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        if (!string.IsNullOrWhiteSpace(command.Name) && await customerRepository.ExistsAsync(command.Name))
        {
            throw new CustomerAlreadyExistsException(command.Name);
        }

        customer.Complete(command.Name, command.FullName, command.Address, command.Nationality,
            new Identity(command.IdentityType, command.IdentitySeries), clock.CurrentDate());
        await customerRepository.UpdateAsync(customer);
        await messageBroker.PublishAsync(new CustomerCompleted(customer.Id, customer.Name, customer.FullName,
            customer.Nationality), cancellationToken);
        logger.LogInformation($"Completed a customer with ID: '{command.CustomerId}'.");
    }
}
