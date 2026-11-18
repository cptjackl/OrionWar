using OrionWar.Game.Rules;

namespace OrionWar.Game.Entities;

public class Game
{
    public required GameSettings Settings { get; set; }
    public required Player[] Players { get; set; }
    public required Star[] Stars { get; set; }
    public required Fleet[] Fleets { get; set; }
    public required GameEvent[] EventLog { get; set; }
    public int Tick { get; set; }
}
