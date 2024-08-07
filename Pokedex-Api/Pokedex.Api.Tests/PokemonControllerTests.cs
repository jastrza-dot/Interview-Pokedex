using System.Net;
using System.Net.Http.Json;
using Pokedex_Api.Models;
using Xunit;

namespace Pokedex.Api.Tests;

public class PokemonControllerTests(PokedexWebApplicationFactory factory) : IClassFixture<PokedexWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task GetShouldReturnNotFoundWhenNoPokemonWithId()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        var result = await _httpClient.GetAsync($"pokemon/{id}");

        // Assert
        
        Assert.False(result.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
    
    [Fact]
    public async Task GetShouldReturnPokemon()
    {
        // Arrange
        var pokemon = new Pokemon("TestPokemon", new Statistics(){Health = 10, Stamina = 10, Strength = 10});
        var response = await _httpClient.PostAsJsonAsync("pokemon", pokemon);
        var pokemonId = await response.Content.ReadFromJsonAsync<Guid>();

        // Act
        var result = await _httpClient.GetAsync($"pokemon/{pokemonId}");
        
        // Assert
        
        Assert.True(result.IsSuccessStatusCode);
    }

    [Fact] public async Task GetAllShouldReturnEmptyListWhenNoPokemonInDb()
    {
        // Act
        var response = await _httpClient.GetAsync("pokemon");

        // Assert
        Assert.True(response.IsSuccessStatusCode);

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<Pokemon>>();
        
        Assert.Empty(result);
    }
    
    [Fact] public async Task GetAllShouldReturnPokemonsFromDb()
    {
        var response = await _httpClient.GetAsync("pokemon");

        // Assert
        Assert.True(response.IsSuccessStatusCode);

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<Pokemon>>();
        
        Assert.NotEmpty(result);
    }
    
    [Fact] public async Task CreatePokemonShouldReturnErrorWhenPokemonAlreadyExists()
    {
        // Arrange
        var allPokemonsResponse = await _httpClient.GetAsync("pokemon");
        var allPokemons = await allPokemonsResponse.Content.ReadFromJsonAsync<IEnumerable<Pokemon>>();

        // Act
        var response = await _httpClient.PostAsJsonAsync("pokemon", allPokemons.First());
        
        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact] public async Task CreatePokemonShouldReturnErrorWhenNameIsInvalid()
    {
        // Arrange
        var pokemon = new Pokemon("Te", new Statistics(){Health = 10, Stamina = 10, Strength = 10});

        // Act
        var response = await _httpClient.PostAsJsonAsync("pokemon", pokemon);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
    
    [Fact] public async Task CreatePokemonShouldOk()
    {
        // Arrange
        var pokemon = new Pokemon("TestPokemon", new Statistics(){Health = 10, Stamina = 10, Strength = 10});

        // Act
        var response = await _httpClient.PostAsJsonAsync("pokemon", pokemon);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
}
