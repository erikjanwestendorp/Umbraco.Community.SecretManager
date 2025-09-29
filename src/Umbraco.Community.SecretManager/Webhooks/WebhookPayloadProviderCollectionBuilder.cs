using Umbraco.Cms.Core.Composing;

namespace Umbraco.Community.SecretManager.Webhooks;

public class WebhookPayloadProviderCollectionBuilder : LazyCollectionBuilderBase<WebhookPayloadProviderCollectionBuilder, WebhookPayloadProviderCollection, IWebhookPayloadProvider>
{
    protected override WebhookPayloadProviderCollectionBuilder This => this;
}
