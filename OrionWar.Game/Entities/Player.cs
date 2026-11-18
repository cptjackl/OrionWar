namespace OrionWar.Game.Entities;

public class Player
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public bool IsAlive { get; set; }
    public required Player[] Allies { get; set; }

    //Technology Levels
    public int WeaponsLevel { get; set; }
    public int ManufacturingLevel { get; set; }
    public int RangeLevel { get; set; }
    public int BankingLevel { get; set; }
    public int ExplorationLevel { get; set; }
    public int TerraformingLevel { get; set; }
    public int ScanningLevel { get; set; }
    
}
