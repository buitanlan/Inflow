using Inflow.Modules.Customers.Core.Commands;
using Inflow.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Inflow.Modules.Customers.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController: Controller
{
    private readonly ICommandDispatcher _commandDispatcher;

    public CustomersController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost]
    public async Task<ActionResult> Post(CreateCustomer command)
    {
        await _commandDispatcher.SendAsync(command);
        return NoContent();
    }
}