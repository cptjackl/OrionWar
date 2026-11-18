namespace OrionWar.Game.Entities;

public class GameEvent
{
    public required Guid GameEventId { get; set; }
    public required Guid GameSessionId { get; set; }
    public required GameSession GameSession { get; set; }
    public int Tick { get; set; }
    public required string Description { get; set; }
}