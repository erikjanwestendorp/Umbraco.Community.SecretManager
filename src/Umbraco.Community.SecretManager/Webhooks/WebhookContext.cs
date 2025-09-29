using Umbraco.Cms.Core.Models;

namespace Umbraco.Community.SecretManager.Webhooks;

public sealed record WebhookContext(
    Uri Endpoint,
    string EventAlias,
    object Notification,
    IWebhook Webhook
);
