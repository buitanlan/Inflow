using Inflow.Modules.Payments.Shared.Clients.DTO;

namespace Inflow.Modules.Payments.Shared.Clients;

internal interface ICustomerApiClient
{
    Task<CustomerDto> GetAsync(Guid customerId);
}
