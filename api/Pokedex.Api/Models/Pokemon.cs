namespace Pokedex.Api.Models;

public class Pokemon
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public Statistics? Statistics { get; set; }
    
    public Pokemon()
    {
        
    }
    public Pokemon(string name, Statistics statistics)
    {
        Id = Guid.NewGuid();
        Name = name;
        Statistics = statistics;
    }
}