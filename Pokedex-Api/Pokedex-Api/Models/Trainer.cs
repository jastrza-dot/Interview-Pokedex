using Ardalis.Result;

namespace Pokedex_Api.Models;

public class Trainer
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public IList<Pokemon> Pokemons { get; set; } = [];

    public Trainer(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}
