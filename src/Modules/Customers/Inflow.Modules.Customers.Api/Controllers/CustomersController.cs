using Inflow.Modules.Customers.Core.Commands;
using Inflow.Modules.Customers.Core.DTO;
using Inflow.Modules.Customers.Core.Queries;
using Inflow.Shared.Abstractions.Dispatchers;
using Microsoft.AspNetCore.Mvc;

namespace Inflow.Modules.Customers.Api.Controllers;

[ApiController]
[Route("[controller]")]
internal class CustomersController: Controller
{
    private readonly IDispatcher _dispatcher;

    public CustomersController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult<CustomerDetailsDto>> Get(Guid customerId)
    {
        var customer = await _dispatcher.QueryAsync(new GetCustomer { CustomerId = customerId });
        if (customer is null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult> Post(CreateCustomer command)
    {
        await _dispatcher.SendAsync(command);
        return NoContent();
    }
}