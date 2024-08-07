using Microsoft.EntityFrameworkCore;
using Pokedex_Api.Models;

namespace Pokedex_Api;

public class PokedexDbContext(DbContextOptions<PokedexDbContext> options) : DbContext(options)
{
    public DbSet<Pokemon> Pokemons { get; set; }

    public DbSet<Trainer> Trainers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pokemon>().HasOne(e => e.Statistics);

        base.OnModelCreating(modelBuilder);
    }
}
