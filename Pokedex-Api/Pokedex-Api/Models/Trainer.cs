namespace Pokedex_Api.Models;

public class Trainer(string name)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;

    public IList<Pokemon> Pokemons { get; set; } = [];
}
