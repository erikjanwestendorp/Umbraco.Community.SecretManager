using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;
using Umbraco.UIBuilder.Extensions;
using Umbraco_UIBuilder_KeyVault_Example.Entities;
using Umbraco_UIBuilder_KeyVault_Example.Recurring;
using Umbraco_UIBuilder_KeyVault_Example.Repositories;
using Umbraco_UIBuilder_KeyVault_Example.Services;
using Umbraco_UIBuilder_KeyVault_Example.Webhooks;

namespace Umbraco_UIBuilder_KeyVault_Example.Compose;

public static class UmbracoBuilderExtensions
{
    public static IUmbracoBuilder ConfigureUmbracoUiBuilderKeyVaultExample(this IUmbracoBuilder builder)
    {
        builder.Services.AddTransient<IKeyVaultService, KeyVaultService>();
        builder.WebhookEvents().Add<KeyVaultSecretsExpiringWebhookEvent>();
        builder.Services.AddRecurringBackgroundJob<KeyVaultExpiryCheckJob>();

        builder.AddUIBuilder(config =>
        {
            config.AddSectionAfter(UmbConstants.Applications.Settings, AppConstants.Applications.ConfigurationName, sectionConfig => sectionConfig
                .Tree(treeConfig =>
                    treeConfig.AddCollection<SecretDetail>(s => s.Name, AppConstants.Collections.SecretSingularName, AppConstants.Collections.SecretPluralName, AppConstants.Collections.SecretDescription, AppConstants.Icons.IconKey, AppConstants.Icons.CombinationLock,
                        collectionConfig => collectionConfig
                            .SetRepositoryType<SecretDetailRepository>()
                            .SetNameProperty(s => s.Name)
                            .ListView(listViewConfig => listViewConfig
                                .AddField(s => s.Expire))
                            .Editor(editorConfig => editorConfig
                                .AddTab(AppConstants.Collections.SecretDetailTab, tabConfig => tabConfig
                                    .AddFieldset(AppConstants.Collections.SecretDetailFieldset, fieldsetConfig =>
                                        fieldsetConfig
                                            .AddField(f => f.Name).MakeReadOnly()
                                            .AddField(f => f.Expire).MakeReadOnly()
                                            .AddField(f => f.CreatedOn).MakeReadOnly()
                                            .AddField(f => f.RecoveryLevel).MakeReadOnly()
                                            .AddField(f => f.Tags).MakeReadOnly())))
                            .DisableDelete()
                            .DisableCreate()
                            .DisableUpdate())));
        });

        return builder;
    }
}
