using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Pokedex.Api.Extensions;

public static class ServiceCollectionExtensions
{
    private const string CorsPolicyName = "AllowAll";

    public static IServiceCollection AddApi(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContextFactory<PokedexDbContext>
        ((_, context) => context
            .UseInMemoryDatabase("InMemoryDbForTesting")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        return serviceCollection;
    }

    public static void AddCorsPolicy(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCors(options =>
        {
            options.AddPolicy(name: CorsPolicyName,
                policy =>
                {
                    policy.AllowAnyOrigin();
                });
        });
    }

    public static void UseCorsPolicy(this WebApplication app) => app.UseCors(CorsPolicyName);
}
