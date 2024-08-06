using System.Net;
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
    public void GetTrainerShouldOk()
    {
        //Arrange
        var response = _httpClient.GetAsync($"trainer/{Guid.NewGuid()}");
        
        //Act
        
        //Assert
    }
    
    [Fact]
    public void GetAllTrainerShouldReturnEmptyListWhenNoTrainerAdded()
    {
        //Arrange
        var response = _httpClient.GetAsync($"trainer/{Guid.NewGuid()}");
        
        //Act
        
        //Assert
    }
    
    [Fact]
    public void GetAllTrainerShouldReturnPreviouslyAddedTrainer()
    {
        //Arrange
        var response = _httpClient.GetAsync($"trainer/{Guid.NewGuid()}");
        
        //Act
        
        //Assert
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
    public void CreateTrainerShouldBeSuccessful()
    {
        //Arrange
        var response = _httpClient.GetAsync($"trainer/{Guid.NewGuid()}");
        
        //Act
        
        //Assert
    }
}
