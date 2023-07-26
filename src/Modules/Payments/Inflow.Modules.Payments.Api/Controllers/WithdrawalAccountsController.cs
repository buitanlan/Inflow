using Inflow.Shared.Abstractions.Contexts;
using Inflow.Shared.Abstractions.Dispatchers;
using Inflow.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inflow.Modules.Payments.Api.Controllers;


[ApiController]
[Route("withdrawals/accounts")]
internal class WithdrawalAccountsController(IDispatcher dispatcher, IContext context) : Controller
{
    [HttpGet]
    [Authorize]
    [SwaggerOperation("Browse withdrawal accounts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Paged<WithdrawalAccountDto>>> BrowseAsync(
        [FromQuery] BrowseWithdrawalAccounts query)
    {
        if (query.CustomerId.HasValue || context.Identity.IsUser())
        {
            // Customer cannot access the other withdrawal accounts
            query.CustomerId = context.Identity.IsUser() ? context.Identity.Id : query.CustomerId;
        }

        return Ok(await dispatcher.QueryAsync(query));
    }

    [HttpPost]
    [Authorize]
    [SwaggerOperation("Add withdrawal account")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Post(AddWithdrawalAccount command)
    {
        await dispatcher.SendAsync(command.Bind(x => x.CustomerId, context.Identity.Id));
        return NoContent();
    }
}
