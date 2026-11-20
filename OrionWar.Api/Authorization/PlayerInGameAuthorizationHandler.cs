using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OrionWar.Api.Persistence;

namespace OrionWar.Api.Authorization;

public sealed class PlayerInGameAuthorizationHandler(GameDbContext dbContext)
    : AuthorizationHandler<PlayerInGameRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PlayerInGameRequirement requirement)
    {
        var userId = context.User.FindFirstValue("sub")
            ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId) || context.Resource is not HttpContext httpContext)
        {
            return;
        }

        if (!Guid.TryParse(
                httpContext.Request.RouteValues[AuthorizationPolicies.GameSessionRouteValue]?.ToString(),
                out var gameSessionId))
        {
            return;
        }

        var isPlayerInGame = await dbContext.PlayerAccountLinks.AnyAsync(
            link => link.UserId == userId && link.Player.GameSessionId == gameSessionId,
            httpContext.RequestAborted);

        if (isPlayerInGame)
        {
            context.Succeed(requirement);
        }
    }
}
