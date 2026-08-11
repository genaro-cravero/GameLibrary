
namespace GameLibrary
{
    internal interface IGameRepository
    {
        void SaveGames(List<Game> games);
        List<Game> LoadGames();
    }
}
