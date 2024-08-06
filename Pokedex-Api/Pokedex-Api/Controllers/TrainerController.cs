using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokedex_Api.Models;

namespace Pokedex_Api.Controllers;

[ApiController]
[Route("trainer")]
public class TrainerController(IDbContextFactory<PokedexDbContext> dbContextFactory) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<Result<Trainer>> Get(Guid id, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var trainer = await dbContext.Trainers.Where(trainer => trainer.Id == id).FirstOrDefaultAsync(cancellationToken);

        if (trainer == null)
        {
            return Result.NotFound();
        }

        return trainer;
    }
    
    [HttpGet]
    public async Task<IEnumerable<Trainer>> GetAll(CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Trainers.ToListAsync(cancellationToken);
    }
    
    [HttpPost]
    public async Task<Result> Create(
        Trainer trainer,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var alreadyExists = 
            await dbContext.Trainers.Where(t => t.Id == trainer.Id).AnyAsync(cancellationToken: cancellationToken);

        if (alreadyExists)
        {
            return Result.Invalid(new ValidationError($"Trainer with Id {trainer.Id} already exists"));
        }

        if (trainer.Pokemons.Any(t => t.Name.Length <= 3))
        {
            return Result.Invalid(new ValidationError("Too short pokemon name"));
        }

        // var trackedPokemons = await dbContext.Pokemons.AsTracking().ToListAsync(cancellationToken: cancellationToken);
        
        dbContext.Trainers.Add(trainer);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    
    // [HttpPost]
    // public async Task<Result> AssignPokemon(
    //     Guid trainerId,
    //     Guid pokemonId,
    //     CancellationToken cancellationToken)
    // {
    //     await using var dbContext = dbContextFactory.CreateDbContextAsync(cancellationToken).Result;
    //
    //     var trainer = 
    //         await dbContext.Trainers.Where(t => t.Id == trainerId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
    //
    //     if (trainer == null)
    //     {
    //         return Result.Invalid(new ValidationError($"Trainer with Id {trainerId} not exists"));
    //     }
    //
    //     return Result.Success();
    // }
}

// Code duplication in Create Trainer and Create Pokemon
// Racing in SaveChanges without  await
