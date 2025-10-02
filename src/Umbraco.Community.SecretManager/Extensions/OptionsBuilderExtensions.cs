using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Umbraco.Community.SecretManager.Configuration;

public static class OptionsBuilderExtensions
{
    public static (OptionsBuilder<T> Builder, T Instance)
        AddConfiguredOptions<T>(
            this IServiceCollection services,
            IConfiguration config,
            string sectionName) where T : class, new()
    {
        var builder = services.AddOptions<T>()
            .BindConfiguration(sectionName)
            .ValidateDataAnnotations();

        var instance = config.GetConfiguredInstance<T>(sectionName);
        return (builder, instance);
    }
}
