using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.SecretManager.Common.Extensions;
using Umbraco.Community.SecretManager.UIBuilder.Configuration;
using Umbraco.Community.SecretManager.UIBuilder.Entities;
using Umbraco.Community.SecretManager.UIBuilder.Repositories;
using Umbraco.UIBuilder.Extensions;

namespace Umbraco.Community.SecretManager.UIBuilder.Compose;

public static class UmbracoBuilderExtensions
{
    // ReSharper disable once InconsistentNaming
    public static IUmbracoBuilder ConfigureSecretManagerUI(this IUmbracoBuilder builder)
    {
        var (optionsBuilder, options) = builder.Services.AddConfiguredOptions<SecretManagerUIBuilderOptions>(builder.Config, "SecretManager:UiBuilder");

        builder.Services.AddCommonServices();

        builder.AddUIBuilder(config =>
        {
            config.AddSectionAfter(UmbConstants.Applications.Settings, UIConstants.Applications.ConfigurationName, sectionConfig => sectionConfig
                .Tree(treeConfig =>
                    treeConfig.AddCollection<SecretDetail>(s => s.Name, UIConstants.Collections.SecretSingularName, UIConstants.Collections.SecretPluralName, UIConstants.Collections.SecretDescription, UIConstants.Icons.IconKey, UIConstants.Icons.CombinationLock,
                        collectionConfig => collectionConfig
                            .SetRepositoryType<SecretDetailRepository>()
                            .SetNameProperty(s => s.Name)
                            .ListView(listViewConfig => listViewConfig
                                .AddField(s => s.ExpirationPreview))
                            .Editor(editorConfig => editorConfig
                                .AddTab(UIConstants.Collections.SecretDetailTab, tabConfig => tabConfig
                                    .AddFieldset(UIConstants.Collections.SecretDetailFieldset, fieldsetConfig =>
                                        fieldsetConfig
                                            .AddField(f => f.Name).MakeReadOnly()
                                            .AddField(f => f.ExpirationPreview).SetLabel(nameof(SecretDetail.ExpirationDate)).MakeReadOnly()
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
