namespace Pokedex_Api.Models;

public class Statistics
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Health { get; set; }
    public int Strength { get; set; }
    public int Stamina { get; set; }
}
