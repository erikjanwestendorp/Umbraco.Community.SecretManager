using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Umbraco.Community.SecretManager.Common.Services;

namespace Umbraco.Community.SecretManager.Common.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.TryAddTransient<IKeyVaultService, KeyVaultService>();
        return services;
    }
}
