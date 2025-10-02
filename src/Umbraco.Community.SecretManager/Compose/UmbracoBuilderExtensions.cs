using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.SecretManager.Configuration;
using Umbraco.Community.SecretManager.Entities;
using Umbraco.Community.SecretManager.Recurring;
using Umbraco.Community.SecretManager.Repositories;
using Umbraco.Community.SecretManager.Services;
using Umbraco.Community.SecretManager.Webhooks;
using Umbraco.Extensions;
using Umbraco.UIBuilder.Extensions;

namespace Umbraco.Community.SecretManager.Compose;

public static class UmbracoBuilderExtensions
{
    public static IUmbracoBuilder ConfigureSecretManager(this IUmbracoBuilder builder, SecretClient secretClient, Action<OptionsBuilder<SecretManagerOptions>>? configure = null)
    {
        builder.Services.AddTransient<IKeyVaultService, KeyVaultService>();
        builder.WebhookEvents().Add<KeyVaultSecretsExpiringWebhookEvent>();
        builder.Services.AddRecurringBackgroundJob<KeyVaultExpiryCheckJob>();
        builder.WithCollectionBuilder<WebhookPayloadProviderCollectionBuilder>()
            .Add(() => builder.TypeLoader.GetTypes<IWebhookPayloadProvider>());


        var (optionsBuilder, options) = builder.Services.AddConfiguredOptions<SecretManagerOptions>(
            builder.Config, "SecretManager");

        configure?.Invoke(optionsBuilder);

        builder.Services.AddSingleton(_ => secretClient);

        if (options.EnableUmbracoUiBuilder)
        {
            ConfigureUiBuilder(builder);
        }

        return builder;
    }

    private static void ConfigureUiBuilder(IUmbracoBuilder builder)
    {
        builder.AddUIBuilder(config =>
        {
            config.AddSectionAfter(UmbConstants.Applications.Settings, AppConstants.Applications.ConfigurationName, sectionConfig => sectionConfig
                .Tree(treeConfig =>
                    treeConfig.AddCollection<SecretDetail>(s => s.Name, AppConstants.Collections.SecretSingularName, AppConstants.Collections.SecretPluralName, AppConstants.Collections.SecretDescription, AppConstants.Icons.IconKey, AppConstants.Icons.CombinationLock,
                        collectionConfig => collectionConfig
                            .SetRepositoryType<SecretDetailRepository>()
                            .SetNameProperty(s => s.Name)
                            .ListView(listViewConfig => listViewConfig
                                .AddField(s => s.ExpirationDate))
                            .Editor(editorConfig => editorConfig
                                .AddTab(AppConstants.Collections.SecretDetailTab, tabConfig => tabConfig
                                    .AddFieldset(AppConstants.Collections.SecretDetailFieldset, fieldsetConfig =>
                                        fieldsetConfig
                                            .AddField(f => f.Name).MakeReadOnly()
                                            .AddField(f => f.ExpirationDate).SetLabel(nameof(SecretDetail.ExpirationDate)).MakeReadOnly()
                                            .AddField(f => f.CreatedOn).MakeReadOnly()
                                            .AddField(f => f.RecoveryLevel).MakeReadOnly()
                                            .AddField(f => f.Tags).MakeReadOnly())))
                            .DisableDelete()
                            .DisableCreate()
                            .DisableUpdate())));
        });
    }
}
