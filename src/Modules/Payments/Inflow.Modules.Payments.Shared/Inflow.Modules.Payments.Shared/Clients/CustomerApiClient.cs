using Inflow.Modules.Payments.Shared.Clients.DTO;
using Inflow.Shared.Abstractions.Modules;

namespace Inflow.Modules.Payments.Shared.Clients;

internal class CustomerApiClient(IModuleClient moduleClient) : ICustomerApiClient
{
    public Task<CustomerDto> GetAsync(Guid customerId)
        => moduleClient.SendAsync<CustomerDto>("customers/get,", new { customerId });
}
