using Microsoft.Extensions.DependencyInjection;
using GameLibrary;

namespace GameLibrary.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        bool useSqlPersistence = true;

        var services = new ServiceCollection();

        if (useSqlPersistence)
        {
            services.AddSingleton<IGameRepository, SqlGameRepository>();
        }
        else
        {
            services.AddSingleton<IGameRepository, GameRepository>();
        }

        services.AddSingleton<GameLibraryService>();
        services.AddSingleton<GameConsoleUI>();

        var serviceProvider = services.BuildServiceProvider();

        var ui = serviceProvider.GetRequiredService<GameConsoleUI>();
        ui.Run();
    }
}