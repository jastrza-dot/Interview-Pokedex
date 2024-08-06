namespace Pokedex_Api.Models;

public class Pokemon
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Pokemon(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}