using System.Net;
using System.Net.Http.Json;
using Pokedex.Api.Models;
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
                new Pokemon("Pikachu", new Statistics { Health = 10, Stamina = 10, Strength = 10 })
            ]
        };
        var newTrainerResponse = await _httpClient.PostAsJsonAsync("trainer", newTrainer);
        var trainerId = await newTrainerResponse.Content.ReadFromJsonAsync<Guid>();

        //Act
        var response = await _httpClient.GetAsync($"trainer/{trainerId}");

        //Assert
        Assert.True(response.IsSuccessStatusCode);

        var trainer = await response.Content.ReadFromJsonAsync<Trainer>();
        Assert.Equivalent(newTrainer, trainer);
    }

    [Fact]
    public async Task GetAllTrainerShouldReturnPreviouslyAddedTrainer()
    {
        //Arrange
        var newTrainer = new Trainer("Ash's brother");
        await _httpClient.PostAsJsonAsync("trainer", newTrainer);

        //Act
        var response = await _httpClient.GetAsync("trainer");

        //Assert
        var trainers = await response.Content.ReadFromJsonAsync<IEnumerable<Trainer>>();
        Assert.True(trainers.Any(t => t.Name.Equals(newTrainer.Name)));
    }

    [Fact]
    public async Task CreateTrainerShouldBeSuccessful()
    {
        //Arrange
        var newTrainer = new Trainer("Ash's sister");

        //Act
        var response = await _httpClient.PostAsJsonAsync("trainer", newTrainer);

        //Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task CreateTrainerShouldReturnErrorWhenTrainerAlreadyExists()
    {
        //Arrange
        var newTrainer = new Trainer("Duplicate");
        await _httpClient.PostAsJsonAsync("trainer", newTrainer);

        //Act
        var response = await _httpClient.PostAsJsonAsync("trainer", newTrainer);

        //Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(response.StatusCode, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTrainerShouldFailedWhenAnyOfPokemonsDontMeetRequirements()
    {
        //Arrange
        var newTrainer = new Trainer("Another")
        {
            Pokemons =
            [
                new Pokemon("Pi", new Statistics { Health = 10, Stamina = 10, Strength = 10 })
            ]
        };

        //Act
        var response = await _httpClient.PostAsJsonAsync("trainer", newTrainer);

        //Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(response.StatusCode, HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("Charmander")]
    public async Task AssignPokemonShouldBeSuccessful(string pokemonName)
    {
        //Arrange
        var trainer = new Trainer("Chase");
        var trainerResponse = await _httpClient.PostAsJsonAsync("trainer", trainer);
        var trainerId = await trainerResponse.Content.ReadFromJsonAsync<Guid>();

        var pokemon = new Pokemon(pokemonName, new Statistics { Health = 8, Stamina = 12, Strength = 8 });
        var pokemonResponse = await _httpClient.PostAsJsonAsync("pokemon", pokemon);
        var pokemonId = await pokemonResponse.Content.ReadFromJsonAsync<Guid>();

        //Act
        var assignResponse = await _httpClient.PostAsync($"trainer/{trainerId}/assignPokemon/{pokemonId}", null);

        //Assert
        Assert.True(assignResponse.IsSuccessStatusCode);
    }
}
