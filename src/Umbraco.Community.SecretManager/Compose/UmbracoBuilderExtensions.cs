using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
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
