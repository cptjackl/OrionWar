using OrionWar.Game.Rules;

namespace OrionWar.Game.Entities;

public class GameSession
{
    public required Guid Id { get; set; }
    public required GameSettings Settings { get; set; }
    public required List<Player> Players { get; set; } = [];
    public required List<Star> Stars { get; set; } = [];
    public required List<Fleet> Fleets { get; set; } = [];
    public required List<GameEvent> EventLog { get; set; } = [];
    public int Tick { get; set; }
}
