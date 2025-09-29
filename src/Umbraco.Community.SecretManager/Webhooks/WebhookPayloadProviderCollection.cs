using Umbraco.Cms.Core.Composing;

namespace Umbraco.Community.SecretManager.Webhooks;

public class WebhookPayloadProviderCollection(Func<IEnumerable<IWebhookPayloadProvider>> items) : BuilderCollectionBase<IWebhookPayloadProvider>(items);