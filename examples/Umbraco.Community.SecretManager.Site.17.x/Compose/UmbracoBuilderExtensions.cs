using Azure.Security.KeyVault.Secrets;
using Umbraco.Community.SecretManager.Compose;
using Umbraco.Community.SecretManager.Site.WebhookPayloadProviders;
using Umbraco.Community.SecretManager.UIBuilder.Compose;
using Umbraco.Community.SecretManager.Webhooks;

namespace Umbraco.Community.SecretManager.Site.Compose;

public static class UmbracoBuilderExtensions
{
    public static IUmbracoBuilder Configure(this IUmbracoBuilder builder, SecretClient secretClient)
    {
        builder.ConfigureSecretManager(secretClient);
        builder.ConfigureSecretManagerUI();

        builder.WithCollectionBuilder<WebhookPayloadProviderCollectionBuilder>().Add<TeamsSecretsExpiringProvider>();
        return builder;
    }
}