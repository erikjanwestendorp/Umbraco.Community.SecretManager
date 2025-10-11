using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.SecretManager.Common.Extensions;
using Umbraco.Community.SecretManager.Configuration;
using Umbraco.Community.SecretManager.Recurring;
using Umbraco.Community.SecretManager.Webhooks;
using Umbraco.Extensions;

namespace Umbraco.Community.SecretManager.Compose;

public static class UmbracoBuilderExtensions
{
    public static IUmbracoBuilder ConfigureSecretManager(this IUmbracoBuilder builder, SecretClient secretClient, Action<OptionsBuilder<SecretManagerRecurringOptions>>? configure = null)
    {
        builder.Services.AddCommonServices();
        builder.WebhookEvents().Add<KeyVaultSecretsExpiringWebhookEvent>();
        builder.Services.AddRecurringBackgroundJob<KeyVaultExpiryCheckJob>();
        builder.WithCollectionBuilder<WebhookPayloadProviderCollectionBuilder>()
            .Add(() => builder.TypeLoader.GetTypes<IWebhookPayloadProvider>());


        var (optionsBuilder, options) = builder.Services.AddConfiguredOptions<SecretManagerRecurringOptions>(builder.Config, "SecretManager:Recurring");

        configure?.Invoke(optionsBuilder);

        builder.Services.AddSingleton(_ => secretClient);

        return builder;
    }
}
