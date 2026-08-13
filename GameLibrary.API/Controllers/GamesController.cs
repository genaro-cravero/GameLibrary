using Microsoft.AspNetCore.Mvc;
using GameLibrary;

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

    [HttpPost]
    public IActionResult Create([FromBody] Game game)
    {
        if (_service.IsGameAlreadyAdded(game.Name))
        {
            return Conflict($"El juego '{game.Name}' ya existe.");
        }

        _service.AddGame(game);
        return CreatedAtAction(nameof(GetAll), new { name = game.Name }, game);
    }

    [HttpPut("{name}")]
    public IActionResult Update(string name, [FromBody] Game updatedGame)
    {
        var games = _service.SearchGames(name);
        if (games.Count == 0)
        {
            return NotFound($"No se encontró el juego con nombre: {name}");
        }

        var gameToUpdate = games[0];
        var result = _service.UpdateGame(gameToUpdate, updatedGame);

        if (!result)
        {
            return BadRequest("No se pudo actualizar el juego.");
        }

        return Ok(updatedGame);
    }

    [HttpDelete("{name}")]
    public IActionResult Delete(string name)
    {
        var games = _service.SearchGames(name);
        if (games.Count == 0)
        {
            return NotFound($"No se encontró el juego con nombre: {name}");
        }

        var gameToDelete = games[0];
        var result = _service.DeleteGame(gameToDelete);

        if (!result)
        {
            return BadRequest("No se pudo eliminar el juego.");
        }

        return NoContent();
    }
}