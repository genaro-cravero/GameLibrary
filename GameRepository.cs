using System.Text.Json;
namespace GameLibrary;

public class GameRepository : IGameRepository
{
    private readonly string _filePath;

    public GameRepository()
    {
        string dataDirectory = "data";
        Directory.CreateDirectory(dataDirectory);
        _filePath = Path.Combine(dataDirectory, "games.json");
    }

    public void SaveGames(List<Game> games)
    {
        string json = JsonSerializer.Serialize(games);
        File.WriteAllText(_filePath, json);
    }

    public List<Game> LoadGames()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            try
            {
                var games = JsonSerializer.Deserialize<List<Game>>(json);
                return games ?? new List<Game>();
            }
            catch (JsonException)
            {
                Utilities.PrintColoredLine("--Failed to load games from JSON.", ConsoleColor.Red);
            }
        }
        return new List<Game>();
    }


}
