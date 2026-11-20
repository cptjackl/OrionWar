using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OrionWar.Game.Entities;
using OrionWar.Api.Persistence;

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
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PlayerAccountLink> PlayerAccountLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Star>().Ignore(star => star.IsOwned);
        modelBuilder.Entity<GameSession>().OwnsOne(gameSession => gameSession.Settings);
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasIndex(token => token.TokenHash).IsUnique();
            entity.Property(token => token.TokenHash).HasMaxLength(64).IsRequired();
            entity.Property(token => token.RevokedAt).IsConcurrencyToken();
            entity.HasOne(token => token.User)
                .WithMany()
                .HasForeignKey(token => token.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<PlayerAccountLink>(entity =>
        {
            entity.HasKey(link => link.PlayerId);
            entity.HasIndex(link => link.UserId);
            entity.HasOne(link => link.Player)
                .WithMany()
                .HasForeignKey(link => link.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(link => link.User)
                .WithMany()
                .HasForeignKey(link => link.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}