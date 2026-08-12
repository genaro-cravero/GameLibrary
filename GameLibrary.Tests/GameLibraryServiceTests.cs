using Xunit;

namespace GameLibrary.Tests;

public class GameLibraryServiceTests
{
    private GameLibraryService CreateServiceWithSampleGames()
    {
        var service = CreateEmptyService();
        service.AddGame(new Game("Alan Wake", GameGenre.Action, 2010, true));
        service.AddGame(new Game("Alan Wake 2", GameGenre.Action, 2023, true));
        service.AddGame(new Game("Big Walk", GameGenre.Adventure, 2026, false));
        return service;
    }

    private GameLibraryService CreateEmptyService()
    {
        var repository = new InMemoryGameRepository();
        return new GameLibraryService(repository);
    }

    [Fact]
    public void AddGame_ShouldAddGameToRepository()
    {
        // Arrange
        var service = CreateEmptyService();
        var game = new Game("Alan Wake 2", GameGenre.Action, 2023, true);

        // Act
        service.AddGame(game);
        var games = service.GetAllGames();

        // Assert
        Assert.Single(games);
        Assert.Equal("Alan Wake 2", games[0].Name);
    }

    [Fact]
    public void SearchGames_WithMultipleMatches_ShouldReturnAllMatches()
    {
        // Arrange
        var service = CreateServiceWithSampleGames();

        // Act
        var results = service.SearchGames("Alan Wake");

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Contains(results, g => g.Name == "Alan Wake");
        Assert.Contains(results, g => g.Name == "Alan Wake 2");
    }

    [Fact]
    public void SearchGames_WithSingleMatch_ShouldReturnThatGame()
    {
        // Arrange
        var service = CreateServiceWithSampleGames();


        // Act
        var results = service.SearchGames("Big");

        // Assert
        Assert.Single(results);
        Assert.Equal("Big Walk", results[0].Name);
    }

    [Fact]
    public void SearchGames_WithNoMatches_ShouldReturnEmptyList()
    {
        // Arrange
        var service = CreateServiceWithSampleGames();

        // Act
        var results = service.SearchGames("Alan Big Walk");

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void SearchGames_WithEmptyKeyword_ShouldReturnNoGames()
    {
        // Arrange
        var service = CreateServiceWithSampleGames();

        // Act
        var results = service.SearchGames("");

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void SearchGames_IsCaseInsensitive()
    {
        // Arrange
        var service = CreateEmptyService();
        service.AddGame(new Game("Hollow Knight", GameGenre.MetroidVania, 2017, false));

        // Act
        var resultsLowerCase = service.SearchGames("hollow"); 
        var resultsUpperCase = service.SearchGames("HOLLOW");  
        var resultsMixedCase = service.SearchGames("HoLLoW"); 

        // Assert
        Assert.Single(resultsLowerCase);
        Assert.Single(resultsUpperCase);
        Assert.Single(resultsMixedCase);
        Assert.Equal("Hollow Knight", resultsLowerCase[0].Name);
    }

    [Fact]
    public void UpdateGame_ShouldUpdateGameProperties()
    {
        // Arrange
        var service = CreateEmptyService();
        
        var ogGame = new Game("Final Fantasy VII: Rexirth", GameGenre.RPG, 2007, false);
        service.AddGame(ogGame);

        var updatedGame = new Game("Final Fantasy VII: Rebirth", GameGenre.RPG, 2024, true);

        // Act
        var result = service.UpdateGame(ogGame, updatedGame);
        var games = service.GetAllGames();

        // Assert
        Assert.True(result);
        Assert.Equal(games[0].Name, updatedGame.Name);
        Assert.Equal(games[0].Genre, updatedGame.Genre);
        Assert.Equal(games[0].ReleaseYear, updatedGame.ReleaseYear);
        Assert.Equal(games[0].IsCompleted, updatedGame.IsCompleted);
    }
    [Fact]
    public void UpdateGame_WhenGameNotFound_ShouldReturnFalse()
    {
        // Arrange
        var service = CreateServiceWithSampleGames();
        var gameToUpdate = new Game("Fake Game", GameGenre.Other, 2000, false);

        var updatedGame = new Game("Final Fantasy VII: Rebirth", GameGenre.RPG, 2024, true);

        // Act
        var result = service.UpdateGame(gameToUpdate, updatedGame);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DeleteGame_ShouldRemoveGameFromRepository()
    {
        // Arrrange
        var service = CreateServiceWithSampleGames();
        var gameToDelete = service.GetAllGames()[0];
        var gameNameToDelete = gameToDelete.Name;
        var serviceCount = service.GetAllGames().Count;

        // Act
        var result = service.DeleteGame(gameToDelete);
        var newServiceCount = service.GetAllGames().Count;
        var newGames = service.SearchGames(gameNameToDelete);

        // Assert
        Assert.True(result);
        Assert.Equal(newServiceCount, serviceCount - 1);
        Assert.DoesNotContain(newGames, g => g.Name.Equals(gameNameToDelete));

    }

    [Fact]
    public void DeleteGame_WhenGameNotFound_ShouldReturnFalse()
    {
        // Arrange
        var service = CreateServiceWithSampleGames();
        var nonExistentGame = new Game("Fake Game", GameGenre.Other, 2000, false);

        // Act
        var result = service.DeleteGame(nonExistentGame);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsGameAlreadyAdded_WhenGameExists_ReturnsTrue()
    {
        // Arrange
        var service = CreateServiceWithSampleGames();

        // Act
        var result = service.IsGameAlreadyAdded("Alan Wake");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsGameAlreadyAdded_WhenGameDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var service = CreateServiceWithSampleGames();

        // Act
        var result = service.IsGameAlreadyAdded("Fake Game");

        // Assert
        Assert.False(result);
    }
}