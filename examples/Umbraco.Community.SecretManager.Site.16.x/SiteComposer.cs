using Umbraco.Cms.Core.Composing;
using Umbraco.Community.SecretManager.Compose;

namespace Umbraco.Community.SecretManager.Site;

public class SiteComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.ConfigureUmbracoUiBuilderKeyVaultExample();
    }
}