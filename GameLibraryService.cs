namespace GameLibrary;

internal class GameLibraryService
{
    private readonly List<Game> _games = new();
    private readonly IGameRepository _repository;
    public List<Game> GetAllGames()
    {
        return _games.ToList();
    }

    public GameLibraryService(IGameRepository repository)
    {
        _repository = repository;
        _games.AddRange(_repository.LoadGames());
    }

    public void AddGame(Game newGame)
    {
        _games.Add(newGame);
        _repository.SaveGames(_games);
    }

    public bool DeleteGame(Game game)
    {
        var result = _games.Remove(game);
        if (result)
            _repository.SaveGames(_games);
        return result;
    }

    public bool IsGameAlreadyAdded(string name)
    {
        return _games.Any(game => game.Name == name);
    }


    public List<Game> SearchGames(string keyword)
    {
        return _games.Where(g => g.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public bool UpdateGame(Game gameToUpdate, Game updatedGame)
    {
        var index = _games.IndexOf(gameToUpdate);
        if (index != -1)
        {
            _games[index] = updatedGame;
            _repository.SaveGames(_games);
            return true;
        }
        return false;
    }


}