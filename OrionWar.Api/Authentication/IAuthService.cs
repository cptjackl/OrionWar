namespace OrionWar.Api.Authentication;

internal interface IAuthService
{
    Task<RegisterOutcome> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);

    Task<AuthTokensResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    Task<AuthTokensResponse?> RefreshAsync(RefreshRequest request, CancellationToken cancellationToken);
}
