using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace OrionWar.Api.Authentication;

internal static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/register", RegisterAsync)
            .WithName("Register")
            .WithSummary("Register an account")
            .WithDescription("Creates an account and returns access and refresh tokens.")
            .Produces<AuthTokensResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesValidationProblem();

        group.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("Sign in")
            .WithDescription("Validates account credentials and returns access and refresh tokens.")
            .Produces<AuthTokensResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesValidationProblem();

        group.MapPost("/refresh", RefreshAsync)
            .WithName("RefreshAuthentication")
            .WithSummary("Rotate authentication tokens")
            .WithDescription("Consumes a refresh token and returns a new access and refresh token pair.")
            .Produces<AuthTokensResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .ProducesValidationProblem();

        group.MapGet("/me", GetCurrentUserAsync)
            .WithName("GetCurrentUser")
            .WithSummary("Get the authenticated account")
            .WithDescription("Returns the identity represented by the supplied bearer token.")
            .RequireAuthorization()
            .Produces<AuthenticatedUserResponse>()
            .Produces(StatusCodes.Status401Unauthorized);

        return endpoints;
    }

    private static async Task<Results<
        Created<AuthTokensResponse>,
        Conflict<ProblemDetails>,
        ValidationProblem>> RegisterAsync(
        RegisterRequest request,
        IAuthService authService,
        CancellationToken cancellationToken)
    {
        var outcome = await authService.RegisterAsync(request, cancellationToken);
        if (outcome.Tokens is not null)
        {
            return TypedResults.Created("/api/auth/me", outcome.Tokens);
        }

        if (outcome.IsConflict)
        {
            return TypedResults.Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Account already exists",
                Detail = "An account with this email address already exists."
            });
        }

        return TypedResults.ValidationProblem(outcome.Errors);
    }

    private static async Task<Results<Ok<AuthTokensResponse>, UnauthorizedHttpResult>> LoginAsync(
        LoginRequest request,
        IAuthService authService,
        CancellationToken cancellationToken)
    {
        var tokens = await authService.LoginAsync(request, cancellationToken);
        return tokens is null
            ? TypedResults.Unauthorized()
            : TypedResults.Ok(tokens);
    }

    private static async Task<Results<Ok<AuthTokensResponse>, UnauthorizedHttpResult>> RefreshAsync(
        RefreshRequest request,
        IAuthService authService,
        CancellationToken cancellationToken)
    {
        var tokens = await authService.RefreshAsync(request, cancellationToken);
        return tokens is null
            ? TypedResults.Unauthorized()
            : TypedResults.Ok(tokens);
    }

    private static Task<Results<Ok<AuthenticatedUserResponse>, UnauthorizedHttpResult>> GetCurrentUserAsync(
        ClaimsPrincipal principal)
    {
        var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = principal.FindFirstValue(JwtRegisteredClaimNames.Email)
            ?? principal.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(email))
        {
            return Task.FromResult<Results<Ok<AuthenticatedUserResponse>, UnauthorizedHttpResult>>(
                TypedResults.Unauthorized());
        }

        return Task.FromResult<Results<Ok<AuthenticatedUserResponse>, UnauthorizedHttpResult>>(
            TypedResults.Ok(new AuthenticatedUserResponse(userId, email)));
    }
}
