namespace OrionWar.Game.Entities;

public class Star
{
    public required Guid Id { get; set; }
    public Player? Owner { get; set; }
    
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
    public int Military { get; set; }

    public bool IsOwned { get { return Owner != null; } }
}
