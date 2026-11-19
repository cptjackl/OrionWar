using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OrionWar.Game.Entities;

namespace OrionWar.Api;

public class GameDbContext : IdentityDbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
    {
    }
    public DbSet<GameSession> Games { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Star> Stars { get; set; }
    public DbSet<Fleet> Fleets { get; set; }
    public DbSet<GameEvent> GameEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Star>().Ignore(star => star.IsOwned);
        modelBuilder.Entity<GameSession>().OwnsOne(gameSession => gameSession.Settings);
    }
}