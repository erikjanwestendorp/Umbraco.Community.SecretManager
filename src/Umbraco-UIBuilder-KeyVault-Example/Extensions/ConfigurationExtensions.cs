using Microsoft.Extensions.Configuration;

namespace Umbraco_UIBuilder_KeyVault_Example.Extensions;

internal static class ConfigurationExtensions
{
    public static T GetConfiguredInstance<T>(this IConfiguration configuration, string sectionName) where T : new()
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(sectionName, nameof(sectionName));

        var instance = new T();

        var section = configuration.GetSection(sectionName);
        section.Bind(instance);

        return instance;
    }
}
