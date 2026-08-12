
namespace GameLibrary
{
    public interface IGameRepository
    {
        void SaveGames(List<Game> games);
        List<Game> LoadGames();
    }
}
