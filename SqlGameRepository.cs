
namespace GameLibrary;

public class SqlGameRepository : IGameRepository
{
    private readonly GameDbContext _dbContext;

    public SqlGameRepository()
    {
        _dbContext = new GameDbContext();
        _dbContext.Database.EnsureCreated(); 
    }

    public List<Game> LoadGames()
    {
        return _dbContext.Games.ToList();
    }

    public void SaveGames(List<Game> games)
    {
        var existingGames = _dbContext.Games.ToList();

        var gamesToRemove = existingGames.Where(e => !games.Any(g => g.Name == e.Name)).ToList();
        _dbContext.Games.RemoveRange(gamesToRemove);

        // Update or add
        foreach (var game in games)
        {
            var existing = existingGames.FirstOrDefault(e => e.Name == game.Name);
            if (existing != null)
            {
                // Update
                _dbContext.Entry(existing).CurrentValues.SetValues(game);
            }
            else
            {
                // Add
                _dbContext.Games.Add(game);
            }
        }

        _dbContext.SaveChanges();
    }
}
