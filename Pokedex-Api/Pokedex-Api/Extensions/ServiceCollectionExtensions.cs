using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Pokedex_Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContextFactory<PokedexDbContext>
        ((_, context) => context
            .UseInMemoryDatabase("InMemoryDbForTesting")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
        return serviceCollection;
    }
}
