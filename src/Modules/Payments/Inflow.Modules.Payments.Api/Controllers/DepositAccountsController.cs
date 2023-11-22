using Inflow.Modules.Payments.Core.Deposits.DTO;
using Inflow.Modules.Payments.Core.Deposits.Queries;
using Inflow.Shared.Abstractions.Contexts;
using Inflow.Shared.Abstractions.Dispatchers;
using Inflow.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inflow.Modules.Payments.Api.Controllers;

[ApiController]
[Route("deposits/accounts")]
internal class DepositAccountsController(IDispatcher dispatcher, IContext context) : Controller
{
    [HttpGet]
    [Authorize]
    [SwaggerOperation("Browse deposit accounts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Paged<DepositAccountDto>>> BrowseAsync([FromQuery] BrowseDepositAccounts query)
    {
        if (query.CustomerId.HasValue || context.Identity.IsUser())
        {
            // Customer cannot access the other deposit accounts
            query.CustomerId = context.Identity.IsUser() ? context.Identity.Id : query.CustomerId;
        }

        return Ok(await dispatcher.QueryAsync(query));
    }
}
