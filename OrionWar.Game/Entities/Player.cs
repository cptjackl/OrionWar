namespace OrionWar.Game.Entities;

public class Player
{
    public required Guid PlayerId { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    
    public required Guid GameSessionId { get; set; }
    public required GameSession GameSession { get; set; }
    
    public bool IsAlive { get; set; }
    
    //Technology Levels
    public int WeaponsLevel { get; set; }
    public int ManufacturingLevel { get; set; }
    public int RangeLevel { get; set; }
    public int BankingLevel { get; set; }
    public int ExplorationLevel { get; set; }
    public int TerraformingLevel { get; set; }
    public int ScanningLevel { get; set; }
    
}
