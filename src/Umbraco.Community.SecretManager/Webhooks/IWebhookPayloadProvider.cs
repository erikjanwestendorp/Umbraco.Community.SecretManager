namespace Umbraco.Community.SecretManager.Webhooks;

public interface IWebhookPayloadProvider
{
    bool CanHandle(WebhookContext ctx);

    object BuildPayload(WebhookContext ctx);
}