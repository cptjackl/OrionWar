namespace OrionWar.Game.Entities;

public class Fleet
{
    public required Guid FleetId { get; set; }
    public required Guid GameSessionId { get; set; }
    public required GameSession GameSession { get; set; }
    public required Guid OwnerId { get; set; }
    public required Player Owner { get; set; }
    public required Guid OriginId { get; set; }
    public required Star Origin { get; set; }
    public required Guid DestinationId { get; set; }
    public required Star Destination { get; set; }  
    public required int DepartureTick { get; set; }
    public required int ArrivalTick { get; set; }
    public required int Strength { get; set; }
}