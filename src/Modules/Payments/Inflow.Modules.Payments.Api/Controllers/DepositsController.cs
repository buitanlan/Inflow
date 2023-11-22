using Inflow.Modules.Payments.Core.Deposits.Commands;
using Inflow.Modules.Payments.Core.Deposits.DTO;
using Inflow.Modules.Payments.Core.Deposits.Queries;
using Inflow.Shared.Abstractions.Contexts;
using Inflow.Shared.Abstractions.Dispatchers;
using Inflow.Shared.Abstractions.Queries;
using Inflow.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inflow.Modules.Payments.Api.Controllers;

[ApiController]
[Route("[controller]")]
internal class DepositsController(IDispatcher dispatcher, IContext context) : Controller
{
    private const string Policy = "deposits";

    [HttpGet]
    [Authorize]
    [SwaggerOperation("Browse deposits")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Paged<DepositDto>>> BrowseAsync([FromQuery] BrowseDeposits query)
    {
        if (query.CustomerId.HasValue || context.Identity.IsUser())
        {
            // Customer cannot access the other deposits
            query.CustomerId = context.Identity.IsUser() ? context.Identity.Id : query.CustomerId;
        }

        return Ok(await dispatcher.QueryAsync<Paged<DepositDto>>(query));
    }

    [HttpPost]
    [Authorize]
    [SwaggerOperation("Start deposit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Post(StartDeposit command)
    {
        await dispatcher.SendAsync(command.Bind(x => x.CustomerId, context.Identity.Id));
        return NoContent();
    }

    // Acting as a webhook for 3rd party payments service
    [HttpPut("{depositId:guid}/complete")]
    [SwaggerOperation("Complete deposit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post(Guid depositId, CompleteDeposit command)
    {
        await dispatcher.SendAsync(command.Bind(x => x.DepositId, depositId));
        return NoContent();
    }
}
