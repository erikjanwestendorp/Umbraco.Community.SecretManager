using Umbraco.Cms.Core.Composing;
using Umbraco.Community.SecretManager.Compose;
using Umbraco.Community.SecretManager.Site.WebhookPayloadProviders;
using Umbraco.Community.SecretManager.Webhooks;

namespace Umbraco.Community.SecretManager.Site;

public class SiteComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.ConfigureSecretManager();
        builder.WithCollectionBuilder<WebhookPayloadProviderCollectionBuilder>().Add<TeamsSecretsExpiringProvider>();
    }
}