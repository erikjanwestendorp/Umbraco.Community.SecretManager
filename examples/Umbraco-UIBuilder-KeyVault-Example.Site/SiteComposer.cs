using Umbraco.Cms.Core.Composing;
using Umbraco_UIBuilder_KeyVault_Example.Compose;

namespace Umbraco_UIBuilder_KeyVault_Example.Site;

public class SiteComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.ConfigureUmbracoUiBuilderKeyVaultExample();
    }
}