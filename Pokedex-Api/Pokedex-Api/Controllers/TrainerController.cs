using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokedex_Api.Models;

namespace Pokedex_Api.Controllers;

[ApiController]
[Route("trainer")]
public class TrainerController(IDbContextFactory<PokedexDbContext> dbContextFactory) : ControllerBase
{
    [HttpGet("{id}")]
    [TranslateResultToActionResult]
    public async Task<Result<Trainer>> Get(Guid id, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var trainer = await dbContext
            .Trainers
            .Where(trainer => trainer.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (trainer == null)
        {
            return Result.NotFound();
        }

        return trainer;
    }

    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<IEnumerable<Trainer>> GetAll(CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Trainers.ToListAsync(cancellationToken);
    }

    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<Guid>> Create(Trainer trainer, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var alreadyExists =
            await dbContext.Trainers.Where(t => t.Id == trainer.Id).AnyAsync(cancellationToken: cancellationToken);

        if (alreadyExists)
        {
            return Result.Invalid(new ValidationError($"Trainer with Id {trainer.Id} already exists"));
        }

        foreach (var trainerPokemon in trainer.Pokemons)
        {
            var pokemon = await dbContext
                .Pokemons
                .FirstOrDefaultAsync(pokemon => pokemon.Id == trainerPokemon.Id, cancellationToken: cancellationToken);

            if (pokemon != null)
            {
                return Result.Invalid(new ValidationError("Cannot create new trainer with already existing pokemon"));
            }
        }

        if (trainer.Pokemons.Any(t => t.Name.Length < 3))
        {
            return Result.Invalid(new ValidationError("Too short pokemon name"));
        }

        dbContext.Trainers.Add(trainer);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success(trainer.Id);
    }

    [HttpPost("{trainerId}/assignPokemon/{pokemonId}")]
    [TranslateResultToActionResult]
    public async Task<Result> AssignPokemon(Guid trainerId, Guid pokemonId, CancellationToken cancellationToken)
    {
        await using var dbContext = dbContextFactory.CreateDbContextAsync(cancellationToken).Result;

        var trainer =
            await dbContext.Trainers.Where(t => t.Id == trainerId).FirstOrDefaultAsync(cancellationToken: cancellationToken);


        if (trainer == null)
        {
            return Result.Invalid(new ValidationError($"Trainer with Id {trainerId} does not exists"));
        }

        var pokemon =
            await dbContext.Pokemons.Where(t => t.Id == pokemonId).FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (pokemon == null)
        {
            return Result.Invalid(new ValidationError($"Pokemon with Id {pokemonId} does not exists"));
        }

        if (pokemon.Name.Length > 3)
        {
            return Result.Invalid(new ValidationError("Invalid Pokemon Name"));
        }

        trainer.Pokemons.Add(pokemon);

        dbContext.Trainers.Update(trainer);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
