namespace OrionWar.Game.Rules;

public class GameSettings
{
    //CoreGame Settings
    public VictoryCondition VictoryCondition { get; set; }
    public int TickIntervalSeconds { get; set; }
    public int PlayerCount { get; set; }
    
    //Starting Values for Player Stars
    public int StartingStars { get; set; }
    public int StartingEconomy { get; set; }
    public int StartingIndustry { get; set; }
    public int StartingScience { get; set; }
    public int StartingMilitary { get; set; }
    
    //Starting Technology Levels
    public int StartingWeapons { get; set; }
    public int StartingManufacturing { get; set; }
    public int StartingRange { get; set; }
    public int StartingBanking { get; set; }
    public int StartingExploration { get; set; }
    public int StartingTerraforming { get; set; }
    public int StartingScanning { get; set; }
}
