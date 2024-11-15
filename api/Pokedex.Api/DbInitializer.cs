﻿using Microsoft.EntityFrameworkCore;
using Pokedex.Api.Models;

namespace Pokedex.Api;

public static class DbInitializer
{
    private static readonly List<Pokemon> InitPokemons =
    [
        new Pokemon("Bulbasaur", new Statistics { Health = 10, Stamina = 20, Strength = 5 }),
        new Pokemon("Squirtle", new Statistics { Health = 5, Stamina = 10, Strength = 8 }),
        new Pokemon("Charmander", new Statistics { Health = 20, Stamina = 5, Strength = 3 }),
    ];
    public static async Task Initialize(PokedexDbContext dbContext)
    {
        if (await dbContext.Pokemons.AnyAsync())
        {
            return;
        }

        await dbContext.Pokemons.AddRangeAsync(InitPokemons);
        await dbContext.SaveChangesAsync();
    }
    
}
