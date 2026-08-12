using Microsoft.EntityFrameworkCore;

namespace GameLibrary;
public class GameDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dataDirectory = "data";
        Directory.CreateDirectory(dataDirectory);

        var dbPath = Path.Combine(dataDirectory, "games.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Id).ValueGeneratedOnAdd();
            entity.Property(g => g.Name).IsRequired();
            entity.Property(g => g.Genre).IsRequired();
            entity.Property(g => g.ReleaseYear).IsRequired();
            entity.Property(g => g.IsCompleted).IsRequired();
        });
    }
}
