using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Pokedex_Api;

namespace Pokedex.Api.Tests;

public class PokedexWebApplicationFactory : WebApplicationFactory<IPokedexMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder webHostBuilder)
    {
        webHostBuilder.ConfigureServices(
            services =>
            {
                ReplaceDatabase(services);
            });
    }
    
    private static void ReplaceDatabase(IServiceCollection services)
    {
        var contextOptions = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PokedexDbContext>));
 
        if (contextOptions != null)
        {
            services.Remove(contextOptions);
        }
 
        var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
 
        if (dbConnectionDescriptor != null)
        {
            services.Remove(dbConnectionDescriptor);
        }
 
        services.AddDbContextFactory<PokedexDbContext>
        ((_, context) => context
            .UseInMemoryDatabase("InMemoryDbForTesting")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
    }
}
