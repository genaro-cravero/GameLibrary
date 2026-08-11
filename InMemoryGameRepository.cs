
namespace GameLibrary;

internal class InMemoryGameRepository : IGameRepository
{
    private List<Game> _games;

    public InMemoryGameRepository()
    {
        _games = new();
    }

    public List<Game> LoadGames()
    {
        return _games.ToList();
    }

    public void SaveGames(List<Game> games)
    {
        _games = new(games);
    }
}
