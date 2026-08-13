using GameLibrary;

namespace GameLibrary.ConsoleApp;

public class GameConsoleUI
{
    private readonly GameLibraryService _service;

    public GameConsoleUI(GameLibraryService service)
    {
        _service = service;
    }

    public void Run()
    {
        Utilities.PrintColoredLine("=== GAME LIBRARY ===", ConsoleColor.Black, ConsoleColor.DarkMagenta);

        string? option = null;
        do
        {
            option = GetChosenOption();
            CheckForOption(option);
        }
        while (option != "0");
    }

    private void WriteOptions()
    {
        Console.WriteLine("1) Ver juegos");
        Console.WriteLine("2) Agregar juego");
        Console.WriteLine("3) Buscar juego");
        Console.WriteLine("4) Actualizar datos de juego");
        Console.WriteLine("5) Eliminar juego");
        Console.WriteLine("0) Salir");
    }

    private string? GetChosenOption()
    {
        WriteOptions();
        Console.Write("Elegí una opción: ");
        return Console.ReadLine();
    }

    private void CheckForOption(string? option)
    {
        Console.WriteLine();
        switch (option)
        {
            case "1":
                ShowGames();
                break;
            case "2":
                AddGame();
                break;
            case "3":
                SearchGame();
                break;
            case "4":
                UpdateGame();
                break;
            case "5":
                DeleteGame();
                break;
            case "0":
                Console.WriteLine("Hasta luego!");
                break;
            default:
                Console.WriteLine("Opción no válida.");
                break;
        }
        Console.WriteLine();
    }

    private void ShowGames()
    {
        var i = 0;
        foreach (var game in _service.GetAllGames())
        {
            var bgColor = i % 2 == 0 ? ConsoleColor.DarkGray : ConsoleColor.Gray;
            ShowSingleGame(game, "", font: ConsoleColor.Black, bgColor: bgColor);
            i++;
        }
    }

    private void ShowSingleGame(Game game, string prefix = "", ConsoleColor font = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
    {
        Utilities.PrintColoredLine($"{prefix}{game.Name} ||| Género: {game.Genre} ||| Lo completaste = {(game.IsCompleted ? "si" : "no")}", font, bgColor);
    }

    private void AddGame()
    {
        Console.WriteLine("Agregando juego...");

        Game? newGame = GetNewGameFromUser();
        if (newGame == null)
        {
            Console.WriteLine("Ocurrió un error creando el juego.");
            return;
        }

        _service.AddGame(newGame);
    }

    private Game? GetNewGameFromUser()
    {
        string? gameName = GetGameName();
        if (gameName == null) return null;
        GameGenre gameGenre = GetGameGenre();
        int gameYear = GetGameYear();
        bool gameCompleted = GetGameCompleted();

        return new Game(gameName, gameGenre, gameYear, gameCompleted);
    }

    private string? GetGameName()
    {
        Console.Write("Ingrese el nombre del juego: ");
        string name = Console.ReadLine() ?? "";
        name = Utilities.FirstCharToUpper(name);

        if (_service.IsGameAlreadyAdded(name))
        {
            Console.WriteLine("Este juego ya fue agregado.");
            return null;
        }

        return name;
    }

    private GameGenre GetGameGenre()
    {
        string genres = "";
        var genresList = Enum.GetValues<GameGenre>();

        for (int i = 0; i < genresList.Length; i++)
        {
            var genreName = Utilities.FirstCharToUpper(genresList[i].ToString());
            var separator = i >= (genresList.Length - 1) ? "." : ",\n";
            genres += "~ " + genreName + separator;
        }

        bool correctlyGenred;
        GameGenre genre;
        do
        {
            Console.WriteLine($"Ingrese el género del juego: \n{genres}");
            string genreInput = Console.ReadLine() ?? "";
            correctlyGenred = Enum.TryParse(genreInput, true, out GameGenre parsedGenre);
            correctlyGenred &= Enum.IsDefined(parsedGenre);
            genre = correctlyGenred ? parsedGenre : GameGenre.Other;

            if (!correctlyGenred)
            {
                Console.WriteLine("Género inválido, intente nuevamente");
            }
        } while (!correctlyGenred);

        return genre;
    }

    private int GetGameYear()
    {
        Console.Write("Ingrese el año de lanzamiento del juego: ");
        var strYear = Console.ReadLine();
        int releaseYear = int.TryParse(strYear, out int year) ? year : 0;

        while (releaseYear < 1950)
        {
            Console.WriteLine("Año inválido, intente nuevamente");
            Console.Write("Ingrese el año de lanzamiento del juego: ");
            releaseYear = int.TryParse(Console.ReadLine(), out year) ? year : 0;
        }

        return releaseYear;
    }

    private bool GetGameCompleted()
    {
        return ChooseYesOrNo("¿Has terminado el juego?");
    }

    private bool ChooseYesOrNo(string questionToRepeat = "")
    {
        Console.Write($"{questionToRepeat} (s/n): ");
        string yesOrNo = Console.ReadLine()?.ToLower() ?? "";
        while (yesOrNo != "s" && yesOrNo != "n")
        {
            Console.WriteLine("Respuesta inválida, intente nuevamente");
            Console.Write($"{questionToRepeat} (s/n): ");
            yesOrNo = Console.ReadLine()?.ToLower() ?? "";
        }
        return yesOrNo == "s";
    }

    private void SearchGame()
    {
        var gamesFound = GetSearchedGame();
        if (gamesFound.Count <= 0)
        {
            Console.WriteLine("No se encontraron coincidencias.");
            return;
        }
        foreach (var game in gamesFound)
        {
            ShowSingleGame(game);
        }
    }

    private List<Game> GetSearchedGame()
    {
        Console.Write("Ingrese el nombre del juego o palabra clave a buscar: ");
        var name = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.Write("Ingreso inválido, intente nuevamente: ");
            name = Console.ReadLine();
        }
        return _service.SearchGames(name);
    }

    private void UpdateGame()
    {
        Game? targetGame = SelectOneGame();
        if (targetGame == null) return;

        string option = "";
        do
        {
            Console.WriteLine($"Qué quiere modificar de '{targetGame.Name}'?");
            Console.WriteLine("1) Nombre");
            Console.WriteLine("2) Género");
            Console.WriteLine("3) Año de lanzamiento");
            Console.WriteLine("4) Estado de finalización");
            Console.WriteLine("0) Finalizar operación");

            option = Console.ReadLine() ?? "";
            switch (option)
            {
                case "1":
                    string? gameName = GetGameName();
                    if (gameName != null)
                    {
                        var prevName = targetGame.Name;
                        var updatedGameName = new Game(gameName, targetGame.Genre, targetGame.ReleaseYear, targetGame.IsCompleted);
                        _service.UpdateGame(targetGame, updatedGameName);
                        targetGame = updatedGameName;
                        Console.WriteLine($"Nombre cambiado de '{prevName}' a '{gameName}' exitosamente!");
                    }
                    else
                    {
                        Console.WriteLine($"Ocurrió un error cambiando el nombre de '{targetGame.Name}'");
                    }
                    break;
                case "2":
                    GameGenre gameGenre = GetGameGenre();
                    var prevGenre = targetGame.Genre;
                    var updatedGameGenre = new Game(targetGame.Name, gameGenre, targetGame.ReleaseYear, targetGame.IsCompleted);
                    _service.UpdateGame(targetGame, updatedGameGenre);
                    targetGame = updatedGameGenre;
                    Console.WriteLine($"Género cambiado de '{prevGenre}' a '{gameGenre}' exitosamente!");
                    break;
                case "3":
                    int gameYear = GetGameYear();
                    var prevRelease = targetGame.ReleaseYear;
                    var updatedGameYear = new Game(targetGame.Name, targetGame.Genre, gameYear, targetGame.IsCompleted);
                    _service.UpdateGame(targetGame, updatedGameYear);
                    targetGame = updatedGameYear;
                    Console.WriteLine($"Año de lanzamiento cambiado de '{prevRelease}' a '{gameYear}' exitosamente!");
                    break;
                case "4":
                    bool gameCompleted = GetGameCompleted();
                    var prevCompleted = targetGame.IsCompleted;
                    var updatedGameCompleted = new Game(targetGame.Name, targetGame.Genre, targetGame.ReleaseYear, gameCompleted);
                    _service.UpdateGame(targetGame, updatedGameCompleted);
                    targetGame = updatedGameCompleted;
                    Console.WriteLine($"Estado de finalización cambiado de {prevCompleted} a {gameCompleted} exitosamente!");
                    break;
                case "0":
                    Console.WriteLine("Finalizando Operación.");
                    break;
                default:
                    Console.WriteLine("Opción inválida, intente nuevamente.");
                    break;
            }
        } while (option != "0");
    }

    private Game? SelectOneGame()
    {
        var gamesFound = GetSearchedGame();
        if (gamesFound.Count > 5)
        {
            while (gamesFound.Count > 5)
            {
                Console.WriteLine("Demasiadas coincidencias. Intente con algo más específico.");
                gamesFound = GetSearchedGame();
            }
        }
        if (gamesFound.Count <= 0)
        {
            Console.WriteLine("No se encontraron coincidencias");
            return null;
        }

        var targetGame = gamesFound[0];
        if (gamesFound.Count > 1)
        {
            int index = 1;
            Console.WriteLine($"Selecciona una de las siguientes opciones del 1 al {gamesFound.Count}:");
            foreach (var game in gamesFound)
            {
                ShowSingleGame(game, $"{index}) ");
                index++;
            }

            Console.Write("Nº de Juego seleccionado: ");
            var selectionInput = Console.ReadLine();
            int selectedIndex = int.TryParse(selectionInput, out index) ? index : 0;

            while (selectedIndex <= 0 || selectedIndex > gamesFound.Count)
            {
                Console.WriteLine("Número inválido, intente nuevamente");
                Console.Write($"Seleccione un juego del 1 al {gamesFound.Count}: ");
                selectedIndex = int.TryParse(Console.ReadLine(), out index) ? index : 0;
            }

            targetGame = gamesFound[selectedIndex - 1];
        }

        return targetGame;
    }

    private void DeleteGame()
    {
        Game? targetGame = SelectOneGame();
        if (targetGame == null) return;

        var gameName = targetGame.Name;
        bool sureToDelete = ChooseYesOrNo($"Estas seguro que quieres eliminar {gameName}?");
        if (!sureToDelete)
        {
            Console.WriteLine("Operación cancelada!");
            return;
        }

        bool removed = _service.DeleteGame(targetGame);
        var str = removed ? $"{gameName} removido exitosamente!" : $"No se pudo eliminar {gameName}";
        Console.WriteLine(str);
    }
}