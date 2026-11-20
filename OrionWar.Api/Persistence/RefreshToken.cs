using Microsoft.AspNetCore.Identity;

namespace OrionWar.Api.Persistence;

public sealed class RefreshToken
{
    public required Guid Id { get; set; }
    public required string UserId { get; set; }
    public required IdentityUser User { get; set; }
    public required string TokenHash { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }
    public string? ReplacedByTokenHash { get; set; }
}
