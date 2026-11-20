using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrionWar.Api.Persistence;

namespace OrionWar.Api.Authentication;

internal sealed class AuthService(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    GameDbContext dbContext,
    AuthTokenService tokenService) : IAuthService
{
    public async Task<RegisterOutcome> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email
        };
        var createResult = await userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);

            var errors = createResult.Errors
                .GroupBy(error => error.Code, StringComparer.Ordinal)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.Description).ToArray(),
                    StringComparer.Ordinal);
            var isConflict = createResult.Errors.Any(error =>
                error.Code is "DuplicateEmail" or "DuplicateUserName");

            return new RegisterOutcome(null, errors, isConflict);
        }

        var tokens = await CreateTokenPairAsync(user, cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return new RegisterOutcome(tokens, new Dictionary<string, string[]>(), false);
    }

    public async Task<AuthTokensResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return null;
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            lockoutOnFailure: true);
        if (!signInResult.Succeeded)
        {
            return null;
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        var tokens = await CreateTokenPairAsync(user, cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return tokens;
    }

    public async Task<AuthTokensResponse?> RefreshAsync(
        RefreshRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var tokenHash = AuthTokenService.HashRefreshToken(request.RefreshToken);
        var now = DateTimeOffset.UtcNow;

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var storedToken = await dbContext.RefreshTokens
                .Include(token => token.User)
                .SingleOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);
            if (storedToken is null)
            {
                return null;
            }

            if (storedToken.RevokedAt is not null)
            {
                var activeTokens = await dbContext.RefreshTokens
                    .Where(token => token.UserId == storedToken.UserId && token.RevokedAt == null)
                    .ToListAsync(cancellationToken);
                foreach (var activeToken in activeTokens)
                {
                    activeToken.RevokedAt = now;
                }

                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return null;
            }

            if (storedToken.ExpiresAt <= now)
            {
                storedToken.RevokedAt = now;
                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return null;
            }

            storedToken.RevokedAt = now;
            var issuedTokens = await CreateTokenPairAsync(storedToken.User, cancellationToken);
            storedToken.ReplacedByTokenHash =
                AuthTokenService.HashRefreshToken(issuedTokens.RefreshToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return issuedTokens;
        }
        catch (DbUpdateConcurrencyException)
        {
            return null;
        }
    }

    private async Task<AuthTokensResponse> CreateTokenPairAsync(
        IdentityUser user,
        CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var accessToken = tokenService.CreateAccessToken(user, now);
        var refreshToken = AuthTokenService.CreateRefreshToken();
        var refreshTokenExpiresAt = now.AddDays(tokenService.RefreshTokenLifetimeDays);

        dbContext.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            User = user,
            TokenHash = AuthTokenService.HashRefreshToken(refreshToken),
            CreatedAt = now,
            ExpiresAt = refreshTokenExpiresAt
        });
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AuthTokensResponse(
            accessToken.Token,
            accessToken.ExpiresAt,
            refreshToken,
            refreshTokenExpiresAt);
    }
}
