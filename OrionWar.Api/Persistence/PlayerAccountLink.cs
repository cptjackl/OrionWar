using Microsoft.AspNetCore.Identity;
using OrionWar.Game.Entities;

namespace OrionWar.Api.Persistence;

public sealed class PlayerAccountLink
{
    public required Guid PlayerId { get; set; }
    public required Player Player { get; set; }
    public required string UserId { get; set; }
    public required IdentityUser User { get; set; }
}
