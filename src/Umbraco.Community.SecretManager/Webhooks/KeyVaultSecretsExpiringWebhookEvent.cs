using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Sync;
using Umbraco.Cms.Core.Webhooks;
using Umbraco.Community.SecretManager.Notifications;

namespace Umbraco.Community.SecretManager.Webhooks;

[WebhookEvent(AppConstants.WebhookEvents.SecretsExpiringName, UmbConstants.WebhookEvents.Types.Other)]
public sealed class KeyVaultSecretsExpiringWebhookEvent(
    IWebhookFiringService firingService,
    IWebhookService webhookService,
    IOptionsMonitor<WebhookSettings> webhookSettings,
    IServerRoleAccessor serverRoleAccessor,
    ILogger<KeyVaultSecretsExpiringWebhookEvent> logger,
    WebhookPayloadProviderCollection webhookPayloadProviderCollection)
    : WebhookEventBase<KeyVaultSecretsExpiringNotification>(firingService, webhookService, webhookSettings,
        serverRoleAccessor)
{
    public override string Alias => AppConstants.WebhookEvents.SecretsExpiringAlias;

    public override object? ConvertNotificationToRequestPayload(KeyVaultSecretsExpiringNotification notification) => new
    {

        Results = notification.Secrets.Select(result => new
        {
            result.Name,
            result.Expire,
        })
    };

    public override async Task ProcessWebhooks(KeyVaultSecretsExpiringNotification notification,
        IEnumerable<IWebhook> webhooks,
        CancellationToken cancellationToken)
    {
        foreach (var webhook in webhooks)
        {
            if (!webhook.Enabled)
            {
                continue;
            }

            if (!Uri.TryCreate(webhook.Url, UriKind.Absolute, out var endPoint))
            {
                logger.LogError("Invalid webhook URL configured: {Url}", webhook.Url);
                continue;
            }

            var ctx = new WebhookContext(endPoint, Alias, notification, webhook);

            var provider = webhookPayloadProviderCollection.FirstOrDefault(x => x.CanHandle(ctx));

            var payload = provider is null
                ? ConvertNotificationToRequestPayload(notification)
                : provider.BuildPayload(ctx);

            await WebhookFiringService.FireAsync(webhook, Alias, payload, cancellationToken);
        }
    }
}