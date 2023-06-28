using Inflow.Modules.Customers.Core.Clients;
using Inflow.Modules.Customers.Core.Domain.Entities;
using Inflow.Modules.Customers.Core.Domain.Repositories;
using Inflow.Modules.Customers.Core.Exceptions;
using Inflow.Shared.Abstractions.Commands;
using Inflow.Shared.Abstractions.Kernel.ValueObjects;
using Inflow.Shared.Abstractions.Time;
using Microsoft.Extensions.Logging;

namespace Inflow.Modules.Customers.Core.Commands.Handlers;

internal sealed class CreateCustomerHandler(
    ICustomerRepository customerRepository,
    IUserApiClient userApiClient,
    ILogger<CreateCustomerHandler> logger,
    IClock clock) : ICommandHandler<CreateCustomer>
{

    public async Task HandleAsync(CreateCustomer command, CancellationToken cancellationToken)
    {
        _ = new Email(command.Email);

        var user = await userApiClient.GetAsync(command.Email);
        if (user is null)
        {
            throw new UserNotFoundException(command.Email);
        }

        if (user.Role is not "user")
        {
            return;
        }

        var customerId = user.UserId;
        if (await customerRepository.GetAsync(customerId) is not null)
        {
            throw new CustomerAlreadyExistsException(customerId);
        }
        var customer = new Customer(Guid.NewGuid(), command.Email, clock.CurrentDate());
        await customerRepository.AddAsync(customer);
        logger.LogInformation($"Created a customer with Id: {customer.Id}.");
    }
}
