using System.ComponentModel.DataAnnotations;

namespace OrionWar.Api.Authentication;

/// <summary>Credentials and account details for registering a new account.</summary>
public sealed record RegisterRequest
{
    /// <summary>The account email address.</summary>
    [Required, EmailAddress, MaxLength(256)]
    public required string Email { get; init; }

    /// <summary>A password containing at least 12 characters.</summary>
    [Required, StringLength(128, MinimumLength = 12)]
    public required string Password { get; init; }
}

/// <summary>Credentials used to sign in to an existing account.</summary>
public sealed record LoginRequest
{
    /// <summary>The account email address.</summary>
    [Required, EmailAddress, MaxLength(256)]
    public required string Email { get; init; }

    /// <summary>The account password.</summary>
    [Required, StringLength(128, MinimumLength = 1)]
    public required string Password { get; init; }
}

/// <summary>A refresh token used to rotate an authentication session.</summary>
public sealed record RefreshRequest
{
    /// <summary>The refresh token issued during registration, login, or a previous refresh.</summary>
    [Required, StringLength(128, MinimumLength = 32)]
    public required string RefreshToken { get; init; }
}

/// <summary>Access and refresh credentials returned after authentication.</summary>
public sealed record AuthTokensResponse(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt);

/// <summary>The identity of the authenticated account.</summary>
public sealed record AuthenticatedUserResponse(string UserId, string Email);

internal sealed record RegisterOutcome(
    AuthTokensResponse? Tokens,
    Dictionary<string, string[]> Errors,
    bool IsConflict);
