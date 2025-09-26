using Umbraco.Cms.Core.Composing;
using Umbraco.Community.SecretManager.Compose;

namespace Umbraco_UIBuilder_KeyVault_Example.Site;

public class SiteComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.ConfigureUmbracoUiBuilderKeyVaultExample();
    }
}