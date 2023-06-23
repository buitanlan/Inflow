using Inflow.Modules.Users.Core.Commands;
using Inflow.Modules.Users.Core.DTO;
using Inflow.Modules.Users.Core.Queries;
using Inflow.Modules.Users.Core.Services;
using Inflow.Shared.Abstractions.Contexts;
using Inflow.Shared.Abstractions.Dispatchers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inflow.Modules.Users.Api.Controllers;


internal class AccountController(
    IDispatcher dispatcher,
    IContext context,
    IUserRequestStorage userRequestStorage,
    CookieOptions cookieOptions) : BaseController
{
    private const string AccessTokenCookie = "__access-token";

    [HttpGet]
    [Authorize]
    [SwaggerOperation("Get account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDetailsDto>> GetAsync()
        => OkOrNotFound(await dispatcher.QueryAsync(new GetUser {UserId = context.Identity.Id}));

    [HttpPost("sign-up")]
    [SwaggerOperation("Sign up")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> SignUpAsync(SignUp command)
    {
        await dispatcher.SendAsync(command);
        return NoContent();
    }

    [HttpPost("sign-in")]
    [SwaggerOperation("Sign in")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDetailsDto>> SignInAsync(SignIn command)
    {
        await dispatcher.SendAsync(command);
        var jwt = userRequestStorage.GetToken(command.Id);
        var user = await dispatcher.QueryAsync(new GetUser {UserId = jwt.UserId});
        AddCookie(AccessTokenCookie, jwt.AccessToken);
        return Ok(user);
    }

    [Authorize]
    [HttpDelete("sign-out")]
    [SwaggerOperation("Sign out")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> SignOutAsync()
    {
        await dispatcher.SendAsync(new SignOut(context.Identity.Id));
        DeleteCookie(AccessTokenCookie);
        return NoContent();
    }

    private void AddCookie(string key, string value) => Response.Cookies.Append(key, value, cookieOptions);

    private void DeleteCookie(string key) => Response.Cookies.Delete(key, cookieOptions);
}
