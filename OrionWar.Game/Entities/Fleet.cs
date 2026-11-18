namespace OrionWar.Game.Entities;

public class Fleet
{
    public required Guid Id { get; set; }
    public required Player Owner { get; set; }
    public required FleetAction Action { get; set; } 
    public required FleetMovement[] Path { get; set; }
    public required int Ships { get; set; }
}
