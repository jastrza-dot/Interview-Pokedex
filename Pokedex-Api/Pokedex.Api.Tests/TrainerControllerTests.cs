using System.Net;
using System.Net.Http.Json;
using Ardalis.Result;
using Pokedex_Api.Models;
using Xunit;

namespace Pokedex.Api.Tests;

public class TrainerControllerTests(PokedexWebApplicationFactory factor) 
    : IClassFixture<PokedexWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factor.CreateClient();

    [Fact]
    public async Task GetTrainerShouldReturnErrorWhenTrainerNotExists()
    {
        //Act
        var response = await _httpClient.GetAsync($"trainer/{Guid.NewGuid()}");
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetTrainerShouldReturnTrainerAndAssignedPokemon()
    {
        //Arrange
        var newTrainer = new Trainer("Ash")
        {
            Pokemons =
            [
                new Pokemon("Pikachu", new Statistics() { Health = 10, Stamina = 10, Strength = 10 })
            ]
        };
        var newTrainerResponse = await _httpClient.PostAsJsonAsync("trainer",newTrainer);
        var trainerId = await newTrainerResponse.Content.ReadFromJsonAsync<Result<Guid>>();
        
        //Act
        var response = await _httpClient.GetAsync($"trainer/{trainerId?.Value}");
        
        //Assert
        Assert.True(response.IsSuccessStatusCode);

        var trainer = await response.Content.ReadFromJsonAsync<Result<Trainer>>();
        Assert.Equivalent(newTrainer, trainer?.Value);
    }
    
    [Fact]
    public async Task GetAllTrainerShouldReturnEmptyListWhenNoTrainerAdded()
    {
        //Act
        var response = await _httpClient.GetAsync($"trainer");
        
        //Assert
        Assert.True(response.IsSuccessStatusCode);
        var trainers = await response.Content.ReadFromJsonAsync<IEnumerable<Trainer>>();
        Assert.Empty(trainers!);
    }
    
    [Fact]
    public async Task GetAllTrainerShouldReturnPreviouslyAddedTrainer()
    {
        //Arrange
        var newTrainer = new Trainer("Ash")
        {
            Pokemons =
            [
                new Pokemon("Pikachu", new Statistics() { Health = 10, Stamina = 10, Strength = 10 })
            ]
        };
        await _httpClient.PostAsJsonAsync("trainer",newTrainer);        
        
        //Act
        var response = await _httpClient.GetAsync($"trainer");
        
        //Assert
        var trainers = await response.Content.ReadFromJsonAsync<IEnumerable<Trainer>>();
        Assert.Equal(trainers?.FirstOrDefault()?.Name, newTrainer.Name);
    }
    
    [Fact]
    public void CreateTrainerShouldReturnErrorWhenTrainerAlreadyExists()
    {
        //Arrange
        var response = _httpClient.GetAsync($"trainer/{Guid.NewGuid()}");
        
        //Act
        
        //Assert
    }
    
    [Fact]
    public void CreateTrainerShouldFailedWhenAnyOfPokemonsDontMeetRequirements()
    {
        //Arrange
        var response = _httpClient.GetAsync($"trainer/{Guid.NewGuid()}");
        
        //Act
        
        //Assert
    }
    
    [Fact]
    public async Task CreateTrainerShouldBeSuccessful()
    {
        //Arrange
        var newTrainer = new Trainer("Ash")
        {
            Pokemons =
            [
                new Pokemon("Pikachu", new Statistics() { Health = 10, Stamina = 10, Strength = 10 })
            ]
        };
        
        //Act
        var response = await _httpClient.PostAsJsonAsync("trainer",newTrainer);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
    }
}
