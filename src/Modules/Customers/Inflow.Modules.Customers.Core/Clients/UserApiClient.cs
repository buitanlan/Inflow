using Inflow.Modules.Customers.Core.Clients.DTO;
using Inflow.Shared.Abstractions.Modules;

namespace Inflow.Modules.Customers.Core.Clients;

internal sealed class UserApiClient(IModuleClient moduleClient) : IUserApiClient
{
    public Task<UserDto> GetAsync(string email) => moduleClient.SendAsync<UserDto>("users/get", new { email });
}
