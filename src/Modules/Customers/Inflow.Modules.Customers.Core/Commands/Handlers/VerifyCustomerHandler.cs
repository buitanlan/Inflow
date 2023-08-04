using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Modules.Customers.Core.Events;
using Inflow.Modules.Customers.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Messaging;
using Inflow.Shared.Abstractions.Time;

namespace Inflow.Modules.Customers.Core.Commands.Handlers;

internal sealed class VerifyCustomerHandler(
    ICustomerRepository customerRepository,
    IMessageBroker messageBroker,
    IClock clock,
    ILogger<VerifyCustomerHandler> logger) : ICommandHandler<VerifyCustomer>
{
    public async Task HandleAsync(VerifyCustomer command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetAsync(command.CustomerId);
        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        customer.Verify(clock.CurrentDate());
        await customerRepository.UpdateAsync(customer);
        await messageBroker.PublishAsync(new CustomerVerified(command.CustomerId), cancellationToken);
        logger.LogInformation($"Verified a customer with ID: '{command.CustomerId}'.");
    }
}
