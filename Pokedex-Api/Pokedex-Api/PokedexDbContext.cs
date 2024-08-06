using Microsoft.EntityFrameworkCore;
using Pokedex_Api.Models;

namespace Pokedex_Api;

public class PokedexDbContext : DbContext
{
    public PokedexDbContext(DbContextOptions<PokedexDbContext> options) : base(options) { }
    
    public DbSet<Pokemon> Pokemons { get; set; }

    public DbSet<Trainer> Trainers { get; set; }

    private List<Pokemon> InitPokemons =
    [
        new Pokemon("First"),
        new Pokemon("Second"),
        new Pokemon("3")
    ];
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pokemon>(e => e.HasData(InitPokemons));
        
        base.OnModelCreating(modelBuilder);
    }
}
