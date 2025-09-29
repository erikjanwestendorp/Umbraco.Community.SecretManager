using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Umbraco.Community.SecretManager.Site.Configuration;
using Umbraco.Community.SecretManager.Site.Extensions;

namespace Umbraco.Community.SecretManager.Site.Compose;

public static class WebApplicationBuilderExtensions
{
    public static SecretClient ConfigureKeyVault(this WebApplicationBuilder builder)
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
        

        //builder.Configuration.AddAzureKeyVault(secretClient, new AzureKeyVaultConfigurationOptions { });

        return secretClient;
    }
}
