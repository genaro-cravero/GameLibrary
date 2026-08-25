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
        Console.WriteLine("1) View Games");
        Console.WriteLine("2) Add Game");
        Console.WriteLine("3) Search Game");
        Console.WriteLine("4) Update Game Data");
        Console.WriteLine("5) Delete Game");
        Console.WriteLine("0) Exit");
    }

    private string? GetChosenOption()
    {
        WriteOptions();
        Console.Write("Choose an option: ");
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
                Console.WriteLine("Goodbye!");
                break;
            default:
                Console.WriteLine("Invalid option.");
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
        Utilities.PrintColoredLine($"{prefix}{game.Name} ||| Genre: {game.Genre} ||| Completed = {(game.IsCompleted ? "yes" : "no")}", font, bgColor);
    }

    private void AddGame()
    {
        Console.WriteLine("Adding game...");

        Game? newGame = GetNewGameFromUser();
        if (newGame == null)
        {
            Console.WriteLine("An error occurred creating the game.");
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
        Console.Write("Enter game name: ");
        string name = Console.ReadLine() ?? "";
        name = Utilities.FirstCharToUpper(name);

        if (_service.IsGameAlreadyAdded(name))
        {
            Console.WriteLine("This game has already been added.");
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
            Console.WriteLine($"Enter the game genre: \n{genres}");
            string genreInput = Console.ReadLine() ?? "";
            correctlyGenred = Enum.TryParse(genreInput, true, out GameGenre parsedGenre);
            correctlyGenred &= Enum.IsDefined(parsedGenre);
            genre = correctlyGenred ? parsedGenre : GameGenre.Other;

            if (!correctlyGenred)
            {
                Console.WriteLine("Invalid genre, try again");
            }
        } while (!correctlyGenred);

        return genre;
    }

    private int GetGameYear()
    {
        Console.Write("Enter the game release year: ");
        var strYear = Console.ReadLine();
        int releaseYear = int.TryParse(strYear, out int year) ? year : 0;

        while (releaseYear < 1950)
        {
            Console.WriteLine("Invalid year, try again");
            Console.Write("Enter the game release year: ");
            releaseYear = int.TryParse(Console.ReadLine(), out year) ? year : 0;
        }

        return releaseYear;
    }

    private bool GetGameCompleted()
    {
        return ChooseYesOrNo("Have you finished the game?");
    }

    private bool ChooseYesOrNo(string questionToRepeat = "")
    {
        Console.Write($"{questionToRepeat} (y/n): ");
        string yesOrNo = Console.ReadLine()?.ToLower() ?? "";
        while (yesOrNo != "y" && yesOrNo != "n")
        {
            Console.WriteLine("Invalid response, try again");
            Console.Write($"{questionToRepeat} (y/n): ");
            yesOrNo = Console.ReadLine()?.ToLower() ?? "";
        }
        return yesOrNo == "y";
    }

    private void SearchGame()
    {
        var gamesFound = GetSearchedGame();
        if (gamesFound.Count <= 0)
        {
            Console.WriteLine("No matches found.");
            return;
        }
        foreach (var game in gamesFound)
        {
            ShowSingleGame(game);
        }
    }

    private List<Game> GetSearchedGame()
    {
        Console.Write("Enter the game name or keyword to search: ");
        var name = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(name))
        {
            Console.Write("Invalid input, try again: ");
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
            Console.WriteLine($"What do you want to modify of '{targetGame.Name}'?");
            Console.WriteLine("1) Name");
            Console.WriteLine("2) Genre");
            Console.WriteLine("3) Release Year");
            Console.WriteLine("4) Completion Status");
            Console.WriteLine("0) Exit");

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
                        Console.WriteLine($"Name changed from '{prevName}' to '{gameName}' successfully!");
                    }
                    else
                    {
                        Console.WriteLine($"An error occurred while changing the name of '{targetGame.Name}'");
                    }
                    break;
                case "2":
                    GameGenre gameGenre = GetGameGenre();
                    var prevGenre = targetGame.Genre;
                    var updatedGameGenre = new Game(targetGame.Name, gameGenre, targetGame.ReleaseYear, targetGame.IsCompleted);
                    _service.UpdateGame(targetGame, updatedGameGenre);
                    targetGame = updatedGameGenre;
                    Console.WriteLine($"Genre changed from '{prevGenre}' to '{gameGenre}' successfully!");
                    break;
                case "3":
                    int gameYear = GetGameYear();
                    var prevRelease = targetGame.ReleaseYear;
                    var updatedGameYear = new Game(targetGame.Name, targetGame.Genre, gameYear, targetGame.IsCompleted);
                    _service.UpdateGame(targetGame, updatedGameYear);
                    targetGame = updatedGameYear;
                    Console.WriteLine($"Release Year changed from '{prevRelease}' to '{gameYear}' successfully!");
                    break;
                case "4":
                    bool gameCompleted = GetGameCompleted();
                    var prevCompleted = targetGame.IsCompleted;
                    var updatedGameCompleted = new Game(targetGame.Name, targetGame.Genre, targetGame.ReleaseYear, gameCompleted);
                    _service.UpdateGame(targetGame, updatedGameCompleted);
                    targetGame = updatedGameCompleted;
                    Console.WriteLine($"Completion Status changed from {prevCompleted} to {gameCompleted} successfully!");
                    break;
                case "0":
                    Console.WriteLine("Exiting Operation.");
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again.");
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
                Console.WriteLine("Too many matches. Please try something more specific.");
                gamesFound = GetSearchedGame();
            }
        }
        if (gamesFound.Count <= 0)
        {
            Console.WriteLine("No matches found");
            return null;
        }

        var targetGame = gamesFound[0];
        if (gamesFound.Count > 1)
        {
            int index = 1;
            Console.WriteLine($"Select one of the following options from 1 to {gamesFound.Count}:");
            foreach (var game in gamesFound)
            {
                ShowSingleGame(game, $"{index}) ");
                index++;
            }

            Console.Write("Selected Game Number: ");
            var selectionInput = Console.ReadLine();
            int selectedIndex = int.TryParse(selectionInput, out index) ? index : 0;

            while (selectedIndex <= 0 || selectedIndex > gamesFound.Count)
            {
                Console.WriteLine("Invalid number, please try again");
                Console.Write($"Select a game from 1 to {gamesFound.Count}: ");
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
        bool sureToDelete = ChooseYesOrNo($"Are you sure you want to delete {gameName}?");
        if (!sureToDelete)
        {
            Console.WriteLine("Operation cancelled!");
            return;
        }

        bool removed = _service.DeleteGame(targetGame);
        var str = removed ? $"{gameName} deleted successfully!" : $"Could not delete {gameName}";
        Console.WriteLine(str);
    }
}