using GameLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly GameLibraryService _service;

    public GamesController(GameLibraryService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var games = _service.GetAllGames();
        return Ok(games);
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string name)
    {
        var games = _service.SearchGames(name);
        return Ok(games);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] Game game)
    {
        if (_service.IsGameAlreadyAdded(game.Name))
        {
            return Conflict($"The game '{game.Name}' already exists.");
        }

        _service.AddGame(game);
        return CreatedAtAction(nameof(GetAll), new { name = game.Name }, game);
    }

    [Authorize]
    [HttpPut("{name}")]
    public IActionResult Update(string name, [FromBody] Game updatedGame)
    {
        var games = _service.SearchGames(name);
        if (games.Count == 0)
        {
            return NotFound($"Game not found with name: {name}");
        }

        var gameToUpdate = games[0];
        var result = _service.UpdateGame(gameToUpdate, updatedGame);

        if (!result)
        {
            return BadRequest("Could not update the game.");
        }

        return Ok(updatedGame);
    }

    [Authorize]
    [HttpDelete("{name}")]
    public IActionResult Delete(string name)
    {
        var games = _service.SearchGames(name);
        if (games.Count == 0)
        {
            return NotFound($"Game not found with name: {name}");
        }

        var gameToDelete = games[0];
        var result = _service.DeleteGame(gameToDelete);

        if (!result)
        {
            return BadRequest("Could not delete the game.");
        }

        return NoContent();
    }
}