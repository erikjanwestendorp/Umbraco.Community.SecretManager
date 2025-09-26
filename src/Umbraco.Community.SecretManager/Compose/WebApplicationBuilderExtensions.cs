using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Umbraco.Community.SecretManager.Configuration;
using Umbraco.Community.SecretManager.Extensions;

namespace Umbraco.Community.SecretManager.Compose;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureKeyVault(this WebApplicationBuilder builder)
    {
        var keyVaultSettings = builder.Configuration.GetConfiguredInstance<KeyVaultOptions>(SectionKeys.KeyVault);


        if (string.IsNullOrWhiteSpace(keyVaultSettings.Endpoint) || !Uri.TryCreate(keyVaultSettings.Endpoint, UriKind.Absolute, out var validUri))
        {
            throw new InvalidOperationException($"Key Vault endpoint is not configured. Please set the '{SectionKeys.KeyVault}:{nameof(KeyVaultOptions.Endpoint)}' configuration value.");
        }

        TokenCredential credential = builder.Environment.IsDevelopment()
            ? new AzureCliCredential()
            : new DefaultAzureCredential();

        var secretClient = new SecretClient(new Uri(keyVaultSettings.Endpoint), credential);
        builder.Services.AddSingleton(_ => secretClient);

        //builder.Configuration.AddAzureKeyVault(secretClient, new AzureKeyVaultConfigurationOptions { });

        return builder;
    }
}
