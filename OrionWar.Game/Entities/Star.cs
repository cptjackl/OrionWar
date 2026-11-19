namespace OrionWar.Game.Entities;

public class Star
{
    public required Guid StarId { get; set; }
    
    public required Guid GameSessionId { get; set; }
    public required GameSession GameSession { get; set; }

    public required Guid? OwnerId { get; set; }    
    public Player? Owner { get; set; }
    
    public bool IsOwned { get { return Owner != null; } }
    
    //Constant Properties
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public int NaturalResources { get; set; }

   //Infrastructure Levels
    public int Economy { get; set; }
    public int Industry { get; set; }
    public int Science { get; set; }
    public bool WarpGate { get; set; }

    //Current Military Strength
    public int MilitaryStrength { get; set; }
}
