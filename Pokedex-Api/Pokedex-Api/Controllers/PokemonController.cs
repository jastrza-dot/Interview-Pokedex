using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokedex_Api.Models;

namespace Pokedex_Api.Controllers;

[ApiController]
[Route("pokemon/")]
public class PokemonController(IDbContextFactory<PokedexDbContext> dbContextFactory) : ControllerBase
{
    [HttpGet("{id}")]
    [TranslateResultToActionResult]
    public async Task<Result<Pokemon>> Get(Guid id, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var pokemon = await dbContext
            .Pokemons
            .Include(p => p.Statistics)
            .Where(pokemon => pokemon.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (pokemon == null)
        {
            return Result.NotFound();
        }

        return pokemon;
    }
    
    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<IEnumerable<Pokemon>> GetAll(CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext
            .Pokemons
            .Include(p => p.Statistics)
            .ToListAsync(cancellationToken);
    }
    
    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<Guid>> Create(
        Pokemon pokemon,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var alreadyExists = 
            await dbContext.Pokemons.Where(poke => poke.Id == pokemon.Id).AnyAsync(cancellationToken: cancellationToken);

        if (alreadyExists)
        {
            return Result.Invalid(new ValidationError($"Pokemon with Id {pokemon.Id} already exists"));
        }

        if (pokemon.Name.Length < 3)
        {
            return Result.Invalid(new ValidationError("Pokemon name to short"));
        }
        dbContext.Pokemons.Add(pokemon);
        return Result.Success(pokemon.Id);
    }
}
